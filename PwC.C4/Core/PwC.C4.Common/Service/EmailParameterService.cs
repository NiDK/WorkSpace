using System.Collections.Generic;
using System.Linq;
using PwC.C4.Common.Interface;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Common.Service
{
    public class EmailParameterService:IEmailParameterService
    {

        
        #region Singleton

        private static EmailParameterService _instance = null;
        private static readonly object LockHelper = new object();
        private static C4CommonServiceClient _client = null;
        private static string _appCode = null;

        public EmailParameterService()
        {
        }

        public static IEmailParameterService Instance()
        {
            if (_instance == null || _client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _client != null && _appCode != null) return _instance;
                    _instance = new EmailParameterService();
                    _client = new C4CommonServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }

        #endregion

        public List<EmailParameter> GetEmailParameters(string groupName = null)
        {
            return _client.EmailParameters_GetByGroup(_appCode,groupName);
        }

        public List<EmailParameter> GetEmailParameters(int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return GetEmailParameters("", pageIndex, pageSize, out totalCount, groupName);
        }

        public List<EmailParameter> GetEmailParameters(string name, int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return _client.EmailParameters_GetByPaging(_appCode, name, pageIndex, pageSize, groupName, out totalCount);
        }

        public EmailParameter GetEmailParameter(int paraId)
        {
            return _client.EmailParameter_Get(_appCode, paraId);
        }

        public EmailParameter GetEmailParameter(string code, string groupName = null)
        {
            var list = GetEmailParameter(new List<string>() {code}, groupName);
            return list.Any() ? list.First() : new EmailParameter();
        }

        public List<EmailParameter> GetEmailParameter(List<string> codes, string groupName = null)
        {
            return _client.EmailParameters_GetByCodes(_appCode, codes, groupName);
        }

        public int UpdateEmailParameter(EmailParameter emailParameter)
        {
            return _client.EmailParameter_Update(emailParameter);
        }

        public bool DeleteEmailParameter(int paraId,int modifyBy)
        {
            return _client.EmailParameter_Delete(_appCode, paraId, modifyBy);
        }
    }
}
