using System;
using System.Collections.Generic;
using System.Linq;
using PwC.C4.Common.Interface;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.Common.Service
{
    internal class EmailTemplateService:IEmailTemplateService
    {

        #region Singleton

        private static EmailTemplateService _instance = null;
        private static readonly object LockHelper = new object();
        private static C4CommonServiceClient _client = null;
        private static string _appCode = null;
        public EmailTemplateService()
        {
        }

        public static IEmailTemplateService Instance()
        {
            if (_instance == null || _client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _client != null && _appCode != null) return _instance;
                    _instance = new EmailTemplateService();
                    _client = new C4CommonServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }

        #endregion

        public List<EmailTemplate> GetEmailTemplates(string groupName = null)
        {
            return _client.EmailTemplates_GetByGroup(_appCode, groupName);
        }

        public List<EmailTemplate> GetEmailTemplates(int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return GetEmailTemplates("", pageIndex, pageSize, out totalCount, groupName);
        }

        public List<EmailTemplate> GetEmailTemplates(string name, int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return _client.EmailTemplates_GetByPaging(_appCode, name, pageIndex, pageSize, groupName, out totalCount);
        }

        public EmailTemplate GetEmailTemplate(int paraId)
        {
            return _client.EmailTemplate_Get(_appCode, paraId);
        }

        public EmailTemplate GetEmailTemplate(string code, string groupName = null)
        {
            var list = GetEmailTemplate(new List<string>() {code}, groupName);
            return list.Any() ? list.First() : new EmailTemplate();
        }

        public List<EmailTemplate> GetEmailTemplate(List<string> codes, string groupName = null)
        {
            return _client.EmailTemplates_GetByCodes(_appCode, codes, groupName);
        }

        public int UpdateEmailTemplate(EmailTemplate emailParameter)
        {
            return _client.EmailTemplate_Update(emailParameter);
        }

        public bool DeleteEmailTemplate(int paraId, int modifyBy)
        {
            return _client.EmailTemplate_Delete(_appCode,paraId, modifyBy);
        }
    }
}
