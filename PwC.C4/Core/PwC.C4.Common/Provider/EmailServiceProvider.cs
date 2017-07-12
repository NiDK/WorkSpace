using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PwC.C4.Common.Exceptions;
using PwC.C4.Common.Interface;
using PwC.C4.Common.Model;
using PwC.C4.Common.Model.Enum;
using PwC.C4.Common.Service;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Common.Provider
{
    public static class EmailServiceProvider
    {
        static LogWrapper log=new LogWrapper();
        public static Dictionary<string, List<EmailParameter>> GetEmailParametersFromTemplate(string tempCode, EmailTemplate temp = null,
            string group = null)
        {
            if (temp == null)
            {
                temp = EmailTemplateService.Instance().GetEmailTemplate(tempCode, group);
            }
            var key = string.Format("EmailParameter-G{0}_T{1}", group ?? "Default", tempCode);
            var paras =
                Preference.Get<Dictionary<string, List<EmailParameter>>>(key);
            if (paras != null) return paras;
            paras = new Dictionary<string, List<EmailParameter>>();
            const string regexStr = "<%(?<Name>.*?)%>";
            var regex = new Regex(regexStr, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var matchContent = regex.Match(temp.MailContent);
            var pcCodeList = new List<string>();
            while (matchContent.Success)
            {
                var paraCode = matchContent.Groups["Name"].Value;
                if (!pcCodeList.Contains(paraCode))
                {
                    pcCodeList.Add(paraCode);
                }
                matchContent = matchContent.NextMatch();
            }

            if (paras.ContainsKey("Content"))
            {
                paras["Content"] = EmailParameterService.Instance().GetEmailParameter(pcCodeList, group);
            }
            else
            {
                paras.Add("Content", EmailParameterService.Instance().GetEmailParameter(pcCodeList, group));
            }
            var matchSubject = regex.Match(temp.MailSubject);
            var psCodeList = new List<string>();
            while (matchSubject.Success)
            {
                var paraCode = matchSubject.Groups["Name"].Value;
                if (!psCodeList.Contains(paraCode))
                {
                    psCodeList.Add(paraCode);
                }
                matchSubject = matchSubject.NextMatch();
            }
            if (paras.ContainsKey("Subject"))
            {
                paras["Subject"] = EmailParameterService.Instance().GetEmailParameter(psCodeList, group);
            }
            else
            {
                paras.Add("Subject", EmailParameterService.Instance().GetEmailParameter(psCodeList, group));
            }
            Preference.Set(key, paras);
            return paras;
        }

        public static Dictionary<string, string> TranslateParameters(List<EmailParameter> parameters, object data,string userId = null,
            string group = null,Dictionary<string, string> dataMapping=null)
        {
            var result = new Dictionary<string, string>();
            parameters.ForEach(para =>
            {
                switch (para.ParameterType)
                {
                    case ParameterType.Normal:
                        result.Set(para.ParameterCode, para.Content);
                        break;
                    case ParameterType.Interface:
                    case ParameterType.Razor:
                        var str = "";
                        var assemblyInfo = para.Assembly.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                        if (assemblyInfo.Count() == 2)
                        {
                            IEmailParameter emailParameter = null;
                            var assembly = Assembly.Load(assemblyInfo[0]);
                            var types = assembly.GetTypes();
                            foreach (var type in types.Where(type => type.FullName == assemblyInfo[1]))
                            {
                                emailParameter = (IEmailParameter) Activator.CreateInstance(type);
                            }
                            if (emailParameter != null)
                            {
                                userId = userId ?? "EmptyStaff";
                                str = emailParameter.Generate(new EmailParameterTranslationModel()
                                {
                                    AppCode = AppSettings.Instance.GetAppCode(),
                                    UserId = userId,
                                    Group = group,
                                    Content = para.Content,
                                    Data = data
                                });
                            }
                        }
                        if (result.ContainsKey(para.ParameterCode))
                        {
                            result[para.ParameterCode] = str;
                        }
                        else
                        {
                            result.Add(para.ParameterCode, str);
                        }
                        break;
                    case ParameterType.Mapping:
                        if (dataMapping != null && dataMapping.ContainsKey(para.ParameterCode))
                        {
                            result.Set(para.ParameterCode, dataMapping[para.ParameterCode]);
                        }
                        break;

                }
            });
            return result;
        }

        public static EmailTemplate TranslateTemplate(string tempCode, object data, string userId = null,
            string group = null, Dictionary<string, string> dataMapping = null)
        {

            var temp = EmailTemplateService.Instance().GetEmailTemplate(tempCode, group);
            if (temp == null || temp.TemplateId == 0)
            {
                throw new EmailException("EmailTemplate:" + tempCode + " not exist");
            }
            //var content = temp.MailContent;
            //var subject = temp.MailSubject;
            var paras = GetEmailParametersFromTemplate(tempCode, temp, group);

            if (paras.ContainsKey("Content") && paras["Content"].Any())
            {
                var c = paras["Content"];
                var paraDicc = TranslateParameters(c, data, userId, group, dataMapping);
                temp.MailContent = paraDicc.Aggregate(temp.MailContent,
                    (current, dic) => current.Replace("<%" + dic.Key + "%>", dic.Value));
            }
            if (paras.ContainsKey("Subject") && paras["Subject"].Any())
            {
                var s = paras["Subject"];
                var paraDics = TranslateParameters(s, data, userId, group, dataMapping);
                temp.MailSubject = paraDics.Aggregate(temp.MailSubject,
                    (current, dic) => current.Replace("<%" + dic.Key + "%>", dic.Value));
            }

            return temp;
        }


        private static MailQueueModel BuildMailModel(string tempCode, string address, object data, string userId = null,
            string group = null, DateTime? sendDate = null, string ccAddress = null,
            Dictionary<string, string> dataMapping = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(address))
                {
                    var temp = TranslateTemplate(tempCode, data, userId, group, dataMapping);
                    var mail = new MailQueueModel()
                    {
                        AppCode = AppSettings.Instance.GetAppCode(),
                        Content = temp.MailContent,
                        ImmediateFlag = temp.IsImmediate ? "Y" : "N",
                        MailBcc = temp.MailBcc,
                        MailCc = ccAddress ?? temp.MailCc,
                        MailFrom = temp.MailFrom,
                        MailTo = address,
                        Organisation = temp.MailOrganisation,
                        ReplyTo = temp.MailReplyTo,
                        Subject = temp.MailSubject,
                        SubmitBy = temp.MailSubmitBy,
                        SendDate = sendDate ?? DateTime.Now,
                        SendFrom = temp.MailFrom
                    };

                    return mail;
                }
                else
                {
                    log.Error("Send email Address is null,data:" + data);
                    return null;
                }

            }
            catch (Exception rr)
            {
                log.Error("BuildMailModel Error", rr);
                return null;
            }
        }

        public static bool SingleEmailSend(string tempCode, string address, object data, string userId = null,
            string group = null, DateTime? sendDate = null, string ccAddress = null, Dictionary<string, string> dataMapping = null)
        {
            try
            {
                var model = BuildMailModel(tempCode, address, data, userId, group, sendDate, ccAddress, dataMapping);
                if (model == null)
                {
                    return false;
                }
                var c4Client = new C4CommonServiceClient();
                return c4Client.InsertToMailQueue(model) > 0;

            }
            catch (Exception rr)
            {
                log.Error("Get Template Error", rr);
                return false;
            }
        }

        public static bool SingleEmailSend(string tempCode, string address, object data,List<MailAttachment> attachments,  string userId = null,
      string group = null, DateTime? sendDate = null, string ccAddress = null, Dictionary<string, string> dataMapping = null)
        {
            try
            {
                var model = BuildMailModel(tempCode, address, data, userId, group, sendDate, ccAddress, dataMapping);
                if (model == null)
                {
                    return false;
                }
                var c4Client = new C4CommonServiceClient();
                if (attachments == null || !attachments.Any())
                    return c4Client.InsertToMailQueue(model) > 0;
                else
                {
                    return c4Client.InsertToMailQueueWithAttachment(model, attachments) > 0;
                }

            }
            catch (Exception rr)
            {
                log.Error("Get Template Error", rr);
                return false;
            }
        }

        public static bool SingleEmailSend(string tempCode, string address, object data, List<int> attachmentIds, string userId = null,
string group = null, DateTime? sendDate = null, string ccAddress = null, Dictionary<string, string> dataMapping = null)
        {
            try
            {
                var model = BuildMailModel(tempCode, address, data, userId, group, sendDate, ccAddress, dataMapping);
                if (model == null)
                {
                    return false;
                }
                var c4Client = new C4CommonServiceClient();
                if (attachmentIds == null || !attachmentIds.Any())
                    return c4Client.InsertToMailQueue(model) > 0;
                else
                {
                    return c4Client.InsertToMailQueueWithAttachmentId(model, attachmentIds) > 0;
                }

            }
            catch (Exception rr)
            {
                log.Error("Get Template Error", rr);
                return false;
            }
        }

        public static int InsertMailAttachment(MailAttachment mail)
        {
            if (mail != null)
            {
                var c4Client = new C4CommonServiceClient();
                return c4Client.InsertToMailAttachment(mail);
            }
            return -1;
        }

        public static int InsertMailAttachment(string dfsPath,string name,bool linkedResourceFlag)
        {
            //if (!string.IsNullOrEmpty(dfsPath))
            //{
            //    var c4Client = new C4CommonServiceClient();
            //    return c4Client.InsertToMailAttachmentByDfsPath(dfsPath, name, linkedResourceFlag);
            //}
            //return -1;
            throw new NotImplementedException("InsertMailAttachment not support!");
        }

        public static bool SingleEmailSend(string tempCode, string address, string ccAddress, object data,Dictionary<string,string> dataMapping=null)
        {
            return SingleEmailSend(tempCode, address, data, null, null, null, ccAddress, dataMapping);
        }


        public static bool SingleEmailSend(string tempCode, string address, string ccAddress, object data,List<MailAttachment> attachments, Dictionary<string, string> dataMapping = null)
        {
            return SingleEmailSend(tempCode, address, data, attachments, null, null, null, ccAddress, dataMapping);
        }

        public static bool SingleEmailSend(string tempCode, string address, string ccAddress, object data, List<int> attachmentIds, Dictionary<string, string> dataMapping = null)
        {
            return SingleEmailSend(tempCode, address, data, attachmentIds, null, null, null, ccAddress, dataMapping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempCode"></param>
        /// <param name="info">Key=Email,Value=UserId(it's can be empty)</param>
        /// <param name="group"></param>
        /// <param name="dataMapping"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> BatchEmailSend(string tempCode, Dictionary<string, string> info,
            string group = null, Dictionary<string, string> dataMapping = null)
        {
            var result = new Dictionary<string, bool>();
            foreach (var userInfo in info)
            {
                var re = SingleEmailSend(tempCode, userInfo.Key, userInfo.Value, group, dataMapping);
                result.Add(userInfo.Key, re);
            }
            return result;
        }
    }
}
