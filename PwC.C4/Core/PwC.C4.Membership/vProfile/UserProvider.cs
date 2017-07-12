using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Config;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Model.Enum;
using PwC.C4.Membership.Service;

namespace PwC.C4.Membership.vProfile
{
    public class UserProvider : IUserProvider
    {
        private static readonly LogWrapper Log = new LogWrapper();
        private IWCFService _wcfClient;
        private readonly AuthProviderSettings _authProviderSettings = null;
        private readonly string _appCode = null;
        private IWCFService VProfileService => _wcfClient ?? (_wcfClient = new WCFServiceClient());
        private bool _isMobile = false;
        #region Singleton

        private static vProfile.UserProvider _instance = null;
        private static readonly object LockHelper = new object();

        public UserProvider(string appCode)
        {
            _authProviderSettings = MembershipSettings.Instance.GetAuthProviderSettings(AuthProvider.vProfile);
            _appCode = appCode;
        }

        public static IUserProvider Instance(string appCode = null)
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                    {
                        appCode = appCode ?? AppSettings.Instance.GetAppCode();
                        _instance = new vProfile.UserProvider(appCode);
                    }
                }
            }
            return _instance;
        }

        #endregion

        public string StaffName()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.StaffName;
            }
            catch (Exception ee)
            {
                Log.Error("StaffName is Null", ee);
                return "No current user";
            }
        }

        public string StaffId()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.StaffId;
            }
            catch (Exception rr)
            {
                Log.Error("StaffId is Null", rr);
                return "No current id";
            }
        }

        public string StaffCode()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.StaffCode;
            }
            catch (Exception rr)
            {
                Log.Error("StaffCode is Null", rr);
                return "No current id";
            }
        }

        public string OrganizationCode()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.TenantId;
            }
            catch (Exception rr)
            {
                Log.Error("OrganizationCode is Null", rr);
                return "No current id";
            }
        }

        public bool IsMobileMode()
        {
            var s = false;
            var m = HttpContext.Current.Items[AuthConst.CurrentAuthMode];
            if (m != null)
            {
               bool.TryParse(m.ToString(), out s);
            }
            return s;
        }

        public CurrentUserInfo GetCurrentUser()
        {
            var co = HttpContext.Current.User.Identity.Name;
            if (co != null)
            {
                var oascArray = co.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries);
                var cure = CacheHelper.GetCacheItem<CurrentUserInfo>(AuthConst.StaffAndOrgCode + "-" + co);
                if (cure != null)
                {
                    return cure;
                }
                var staffCode = oascArray[0];
                var orgCode = oascArray[1];
                var query = new OrgCodeStaffCodeModel()
                {
                    ApiAccess = AppSettings.Instance.GetApiAccess(),
                    OrganizationCode = orgCode,
                    StaffCode = staffCode
                };

                var baseQuery = JsonHelper.Serialize(query);

                var queryRole = new GetRolesByAppCodeOrgCodeStaffCode()
                {
                    ApiAccess = AppSettings.Instance.GetApiAccess(),
                    OrganizationCode = orgCode,
                    StaffCode = staffCode,
                    ApplicationCode = _appCode
                };

                var result = VProfileService.GetUserByOrgCodeStaffCode(baseQuery);
                var data = JsonHelper.Deserialize<VProfileReturn<GetUserReturnData>>(result);
                if (data.StatusMsg == AuthConst.vProfileSuccessCode)
                {
                    cure = new CurrentUserInfo()
                    {
                        StaffId = data.Data.UserId,
                        StaffName = data.Data.UserName,
                        StaffCode = data.Data.StaffCode,
                        TenantId = data.Data.OrganizationCode,
                        Email = data.Data.Email,
                        Phone = data.Data.Phone
                    };
                    var role = VProfileService.GetRolesByAppCodeOrgCodeStaffCode(JsonHelper.Serialize(queryRole));
                    var roleData = JsonHelper.Deserialize<VProfileReturn<List<Dictionary<string, object>>>>(role);
                    if (roleData.StatusMsg == AuthConst.vProfileSuccessCode)
                    {
                        cure.Roles = roleData.Data.ToRolesList();
                        cure.RolesKv = roleData.Data.ToRolesKv();
                    }
                    else
                    {
                        cure.Roles = new List<string>();
                        cure.RolesKv = new KeyValuePair<string, string>[0];
                    }

                    var group = VProfileService.GetGroupsByOrgCodeStaffCode(baseQuery);
                    var groupData = JsonHelper.Deserialize<VProfileReturn<List<Dictionary<string, object>>>>(group);
                    if (groupData.StatusMsg == AuthConst.vProfileSuccessCode)
                    {
                        cure.Groups = groupData.Data.ToGroupsList();
                        cure.GroupsKv = groupData.Data.ToGroupsKv();
                    }
                    else
                    {
                        cure.Groups = new List<string>();
                        cure.GroupsKv = new KeyValuePair<string, string>[0];
                    }
                    CacheHelper.SetCacheItem(AuthConst.StaffAndOrgCode + "-" + co, cure, null,
                        DateTime.Now.AddSeconds(CurrentCookieTimeout()));
                    return cure;
                }

            }
            Log.Error("HttpContext.Current.User.Identity.Name is null");
            return new CurrentUserInfo() {Roles = new List<string>()};
        }

        public List<CurrentMenu> GetCurrentMenu()
        {
            throw new NotImplementedException("This funcation only support by Application center!");
        }

        public List<string> GetCurrentRoles()
        {
            var c = GetCurrentUser();
            return c.Roles;
        }

        public bool CheckToken(HttpContext context, string token, string ticket)
        {
            var t = "";
            if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(ticket))
            {
                _isMobile = true;
                t = token;
            }
            else if (string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(ticket))
            {
                _isMobile = false;
                t = ticket;
            }
            else
            {
                t = "";
            }
            if (!string.IsNullOrEmpty(t))
            {
                return Verify(context, t);
            }
            return false;

        }

        private bool Verify(HttpContext context, string token)
        {
            var verify = new VerifyModel()
            {
                ApiAccess = AppSettings.Instance.GetApiAccess(),
                Token = HttpUtility.UrlDecode(token),
                WithGroup = true,
                WithRole = true
            };
            if (string.IsNullOrEmpty(token)) return false;
            var currentUser = VProfileService.Verify(JsonHelper.Serialize(verify));
            var cur = JsonHelper.Deserialize<VProfileReturn<VerifyReturnData>>(currentUser);
            if (cur.StatusMsg != AuthConst.vProfileSuccessCode)
            {
                Log.Error("vProfile verify failed!status code:" + cur.StatusMsg);
                return false;
            }
            var userdataincookie = new UserDataInCookie
            {
                UserId = cur.Data.StaffCode + "|C4|" + cur.Data.OrganizationCode,
                UserName = cur.Data.UserName,
                StaffCode = cur.Data.StaffCode,
                Email = cur.Data.Email,
                Phone = cur.Data.Phone,
                AppCode = _appCode,
                Roles = cur.Data.RolesKv,
                Groups = cur.Data.GroupsKv,
                TentantId = cur.Data.OrganizationCode,
                IsMobile = _isMobile
            };
            var tickTimeout = _authProviderSettings.WebAuthTicketTimeout;
            var cookieTimeout = _authProviderSettings.WebCookieTimeout;
            if (_isMobile)
            {
                tickTimeout = _authProviderSettings.MobileAuthTicketTimeout;
                cookieTimeout = _authProviderSettings.MobileCookieTimeout;
            }
            if (Log.IsDebugEnabled)
                Log.Error("vprofile userdataincookie:" + JsonHelper.Serialize(userdataincookie));
            AuthService.SetPrincipalBasedOnUserData(context, userdataincookie);
            AuthService.SetCookieBasedOnUserData(_appCode, context, userdataincookie,
                tickTimeout, cookieTimeout, AuthProvider.vProfile);
            return true;
        }

        public AuthProvider CurrentProvider()
        {
            if (AuthService.IsDebugUser())
            {
                return AuthProvider.DebugUser;
            }
            return AuthProvider.vProfile;
        }

        private int CurrentCookieTimeout()
        {
            return IsMobileMode() ? _authProviderSettings.MobileCookieTimeout : _authProviderSettings.WebCookieTimeout;
        }

        public void Redirect2LoginPage(HttpContext context)
        {
            var loginUrl = ReturnUrlFix(context.Request.Url);
            context.Response.Redirect(loginUrl, true);
        }

        public void Redirect2LoginPage(HttpContextBase contextBase)
        {
            if (contextBase.Request.Url != null)
            {
                var loginUrl = ReturnUrlFix(contextBase.Request.Url);
                contextBase.Response.Redirect(loginUrl, true);
            }
        }

        private string ReturnUrlFix(Uri url)
        {
            var queryString = new NameValueCollection {HttpUtility.ParseQueryString(url.Query)};
            if (!string.IsNullOrEmpty(queryString[AuthConst.ServiceTicket]))
                queryString.Remove(AuthConst.ServiceTicket);
            var loginUrl = _authProviderSettings.FormAutenLoginUrl;
            var q = string.Join("&",
                queryString.AllKeys.Select(a => $"{HttpUtility.UrlEncode(a)}={HttpUtility.UrlEncode(queryString[a])}"));
            var currentUrl = url.GetLeftPart(UriPartial.Path);
            if (!string.IsNullOrEmpty(q))
            {
                currentUrl = currentUrl + "?" + q;
            }
            currentUrl = HttpUtility.UrlEncode(currentUrl);
            loginUrl += "?returnUrl=" + currentUrl + "&appCode=" + _appCode;
            return loginUrl;
        }
    }
}
