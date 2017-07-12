using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Model.Enum;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Membership.Service
{
    internal static class AuthService
    {

        public static void SetPrincipalBasedOnUserData(HttpContext context, UserDataInCookie userdataincookie)
        {
            if (context.Items[AuthConst.CookieUserName] == null) context.Items.Add(AuthConst.CookieUserName, userdataincookie.UserName);
            if (context.Items[AuthConst.CurrentAuthMode] == null) context.Items.Add(AuthConst.CurrentAuthMode, userdataincookie.IsMobile);
            if (!string.IsNullOrEmpty(userdataincookie.StaffCode) &&
                !string.IsNullOrEmpty(userdataincookie.TentantId))
            {
                var code = userdataincookie.StaffCode + "|C4|" + userdataincookie.TentantId;
                var oasc = EncryptHelper.Encode(code);
                if (context.Items[AuthConst.StaffAndOrgCode] != null)
                {
                    context.Items.Remove(AuthConst.StaffAndOrgCode);
                }
                context.Items.Add(AuthConst.StaffAndOrgCode, oasc);
            }
            var identity = new GenericIdentity(userdataincookie.UserId);

            if (userdataincookie.Roles != null && userdataincookie.Roles.Length > 0)
            {
                SetPrincipal(context,
                    new GenericPrincipal(identity, userdataincookie.Roles.Select(p => p.Value).Distinct().ToArray()));
            }
            else
            {
                SetPrincipal(context, new GenericPrincipal(identity, null));
            }
        }

        public static void SetPrincipal(HttpContext context, IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (context != null)
            {
                context.User = principal;
            }
        }

        public static void SetCookieBasedOnUserData(string appCode,HttpContext context, UserDataInCookie userdataincookie,int ticketTimeout,int cookiesTimeout,AuthProvider auth)
        {
            var appCookiePath = AuthConst.AppCookiePath(appCode, auth);
            var authticket = new FormsAuthenticationTicket(
                1,
                (string)userdataincookie.UserId,
                DateTime.Now,
                DateTime.Now.AddMinutes(ticketTimeout),
                false,
                UserDataInCookie.WriteFromObject(userdataincookie));
            var encryptedTicket = FormsAuthentication.Encrypt(authticket);

            var authCustomCookie = new HttpCookie(appCookiePath, encryptedTicket)
            {
                Expires = DateTime.Now.AddMinutes(cookiesTimeout)
            };
            context.Response.Cookies.Add(authCustomCookie);
        }

        public static bool IsDebugUser()
        {
            var d = AppSettings.Instance.GetConfigSettings("IsDebugUser");
            if (d != null)
                return d.ToLower() == "true";
            return false;
        }

        public static void RegDebugUser(HttpContext context)
        {
            string appCode = AppSettings.Instance.GetAppCode();
            var userdataincookie = new UserDataInCookie
            {
                UserId = "Debug user Id",
                UserName = "Debug user",
                StaffCode = "Debug user code",
                AppCode = appCode,
                Roles = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Admin","Admin"), new KeyValuePair<string, string>("AppAdmin", "AppAdmin"), new KeyValuePair<string, string>("User", "User") },
                Groups = new KeyValuePair<string, string>[] {new KeyValuePair<string, string>("CN ALL","CN ALL"), new KeyValuePair<string, string>("HK ALL", "HK ALL") },
                TentantId = "Debug user TenantId",
                IsMobile = false
            };
            var tickTimeout = 10;
            var cookieTimeout = 10;

            AuthService.SetPrincipalBasedOnUserData(context, userdataincookie);
            AuthService.SetCookieBasedOnUserData(appCode, context, userdataincookie,
                tickTimeout, cookieTimeout, AuthProvider.DebugUser);
        }
    }
}
