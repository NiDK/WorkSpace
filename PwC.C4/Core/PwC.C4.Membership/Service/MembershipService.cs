using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Membership.Service
{
    public class MembershipService
    {
        


        #region Singleton

        private static MembershipService _instance = null;
        private static readonly object LockHelper = new object();
        private static C4MembershipServiceClient _client = null;
        private static string _appCode = null;
        public MembershipService()
        {
        }

        public static MembershipService Instance()
        {
            if (_instance == null || _client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _client == null && _appCode != null) return _instance;
                    _instance = new MembershipService();
                    _client = new C4MembershipServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }

        #endregion


        public FunctionCheckResult FunctionCheck(string area,string controller,string action,string url,List<string> roles )
        {
            if (_client == null)
            {
                _client = new C4MembershipServiceClient();
            }
            var model = new FunctionCheck()
            {
                Action = action,
                AppCode = _appCode,
                Area = area,
                Controller = controller,
                RoleName = roles,
                Url = url
            };
            return _client.CheckFunctionRight(model);
        }
    }
}
