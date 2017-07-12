using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ApplicationCenter.Model;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Config;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Model.Enum;
using PwC.C4.Membership.Service;

namespace PwC.C4.Membership.ApplicationCenter
{
    public class UserProvider : IUserProvider
    {
        private static readonly LogWrapper Log = new LogWrapper();
        private IApplicationService _wcfClient;
        private readonly AuthProviderSettings _authProviderSettings = null;
        private readonly string _appCode = null;
        private IApplicationService ApplicationService => _wcfClient ?? (_wcfClient = new ApplicationServiceClient());
        private bool _isMobile = false;
        #region Singleton

        private static UserProvider _instance = null;
        private static readonly object LockHelper = new object();

        public UserProvider(string appCode)
        {
            _authProviderSettings = MembershipSettings.Instance.GetAuthProviderSettings(AuthProvider.ApplicationCenter);
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
                        _instance = new UserProvider(appCode);
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
                return HttpContext.Current.Items[AuthConst.CookieUserName].ToString();
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
                return HttpContext.Current.User.Identity.Name;
            }
            catch (Exception rr)
            {
                Log.Error("StaffId is Null", rr);
                return "No current id";
            }
        }

        public string StaffCode()
        {
            throw new NotImplementedException("This funcation only support by vProfile!");
        }

        public string OrganizationCode()
        {
            throw new NotImplementedException("This funcation only support by vProfile!");
        }

        public CurrentUserInfo GetCurrentUser()
        {

            var user = ApplicationService.GetCurrentUser(HttpContext.Current.User.Identity.Name, _appCode);
            var info = new CurrentUserInfo()
            {
                StaffId = user.UserID,
                StaffName = user.UserName,
                Email = user.EmailAddress,
                Phone = user.MobileNo,
                Roles = user.Roles.Select(c=>c.RoleName).ToList(),
                Menus = GetCurrentMenu()
            };
            return info;
        }

        public List<CurrentMenu> GetCurrentMenu()
        {
            try
            {
                var menuList = GetUserMenus().OrderBy(m => m.MenuOrder).ToList();
                var cm = MappingTo(menuList);
                var menus = cm.Where(c => c.ParentId == Guid.Empty).OrderBy(c => c.MenuOrder).ToList();
                foreach (var currentMenu in menus)
                {
                    GroupMenu(cm, currentMenu);
                }
                return menus;
            }
            catch (Exception ee)
            {
                Log.Error("MenuRoles is Null", ee);
                return new List<CurrentMenu>();
            }
        }

        internal IEnumerable<vwMenuRole> GetUserMenus()
        {
            var menu = ApplicationService.GetMenuRoleMapping(_appCode);
            if (menu != null && menu.Length > 0)
            {
                foreach (var item in menu)
                {
                    if (HttpContext.Current.User.IsInRole(item.RoleName))
                    {
                        yield return item;
                    }
                }
            }
        }

        private List<CurrentMenu> MappingTo(List<vwMenuRole> menus)
        {
            var mn = new List<Guid>();
            var cm = new List<CurrentMenu>();
            menus.ForEach(menu =>
            {
                if (!mn.Contains(menu.MenuID))
                {
                    mn.Add(menu.MenuID);
                    cm.Add(new CurrentMenu()
                    {
                        MenuId = menu.MenuID,
                        MenuUrl = menu.MenuURL,
                        MenuName = menu.MenuName,
                        MenuOrder = menu.MenuOrder,
                        ParentId = menu.ParentMenuID ?? Guid.Empty
                    });
                }
            });
            return cm;
        }

        private void GroupMenu(List<CurrentMenu> menus, CurrentMenu m)
        {
            m.SubMenus = menus.Where(c => c.ParentId == m.MenuId).OrderBy(c => c.MenuOrder).ToList();
            if (m.SubMenus.Any())
            {
                foreach (var currentMenu in m.SubMenus)
                {
                    GroupMenu(menus, currentMenu);
                }
            }
        }

        public List<string> GetCurrentRoles()
        {
            try
            {
                var user = ApplicationService.GetCurrentUser(HttpContext.Current.User.Identity.Name, _appCode);
                if (user?.Roles == null || !user.Roles.Any())
                {
                    Log.Error("CurrentRole is empty,Name:" + HttpContext.Current.User.Identity.Name);
                    return new List<string>();
                }
                return user.Roles.Select(c => c.RoleName).ToList();
            }
            catch (Exception ee)
            {
                Log.Error("CurrentRole is Null", ee);
                return new List<string>();
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

        public bool CheckToken(HttpContext context, string token, string ticket)
        {

            if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(ticket))
            {
                _isMobile = true;
                return VerifySsoKey(context, token);
            }
            else if (string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(ticket))
            {
                _isMobile = false;
                return VerifyServiceTicket(context, ticket);
            }
            else
            {
                return false;
            }

        }

        private bool VerifySsoKey(HttpContext context, string ssokey)
        {
            if (string.IsNullOrEmpty(ssokey)) return false;
            var currentUser = ApplicationService.VerifyHashCode(HttpUtility.UrlDecode(ssokey), _appCode);
            if (string.IsNullOrEmpty(currentUser?.UserID)) return false;
            var userdataincookie = Verfiy(currentUser);
            AuthService.SetPrincipalBasedOnUserData(context, userdataincookie);
            AuthService.SetCookieBasedOnUserData(_appCode, context, userdataincookie,
                _authProviderSettings.MobileAuthTicketTimeout, _authProviderSettings.MobileCookieTimeout,AuthProvider.ApplicationCenter);
            return true;
        }

        private bool VerifyServiceTicket( HttpContext context, string serviceTicket)
        {
            if (string.IsNullOrEmpty(serviceTicket)) return false;
            var currentUser = ApplicationService.VerifyServiceTicket(serviceTicket);
            if (string.IsNullOrEmpty(currentUser?.UserID)) return false;
            var userdataincookie = Verfiy(currentUser);
            AuthService.SetPrincipalBasedOnUserData(context, userdataincookie);
            AuthService.SetCookieBasedOnUserData(_appCode, context, userdataincookie,
                _authProviderSettings.WebAuthTicketTimeout, _authProviderSettings.WebCookieTimeout, AuthProvider.ApplicationCenter);
            return true;
        }

        private UserDataInCookie Verfiy(global::ApplicationCenter.Model.CurrentUser currentUser)
        {

            var userdataincookie = new UserDataInCookie
            {
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                IsMobile = _isMobile,
                Email = currentUser.EmailAddress,
                Phone = currentUser.MobileNo,
                AppCode = _appCode
            };
            if (currentUser.Roles != null && currentUser.Roles.Length > 0)
            {
                userdataincookie.Roles = new KeyValuePair<string, string>[currentUser.Roles.Length];
                for (int i = 0; i < currentUser.Roles.Length; i++)
                {
                    userdataincookie.Roles[i] =
                        new KeyValuePair<string, string>(currentUser.Roles[i].RoleID.ToString(),
                            currentUser.Roles[i].RoleName);
                }
            }
            if(Log.IsDebugEnabled)
                Log.Error("application center userdataincookie:" + JsonHelper.Serialize(userdataincookie));
            return userdataincookie;
        }

        public AuthProvider CurrentProvider()
        {
            if (AuthService.IsDebugUser())
            {
                return  AuthProvider.DebugUser;
            }
            return AuthProvider.ApplicationCenter;
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
            var queryString = new NameValueCollection { HttpUtility.ParseQueryString(url.Query) };
            if (!string.IsNullOrEmpty(queryString[AuthConst.ServiceTicket]))
                queryString.Remove(AuthConst.ServiceTicket);
            var loginUrl = _authProviderSettings.FormAutenLoginUrl;
            var q = string.Join("&",
                queryString.AllKeys.Select(a => $"{HttpUtility.UrlEncode(a)}={HttpUtility.UrlEncode(queryString[a])}"));
            var currentUrl = url.GetLeftPart(UriPartial.Path); ;
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
