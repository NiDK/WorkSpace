using System;
using System.Web;
using System.Web.Security;
using PwC.C4.Configuration.Maintenance;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Web.Auth
{
    public class AuthorizationHelper
    {
        public static readonly string CaptchaCookieName = "vcode_dfs";
        public static readonly string EncryptedCaptchaCookieName = "VK_DFS";

        public static void VerifyCaptcha(HttpContext context, string returnUrl)
        {
            if (MaintenanceConfig.Instance.IsFunctionEnabled("DfsCaptcha"))
            {
                string code1 = GetCookie(context, CaptchaCookieName);
                string code2 = GetEncryptedCookie(context, EncryptedCaptchaCookieName);

                if (!string.IsNullOrWhiteSpace(code1) &&
                    !string.IsNullOrWhiteSpace(code2) &&
                    string.Equals(code1, code2, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                context.Response.Redirect("/Home/Validate?return=" + HttpUtility.UrlEncode(returnUrl));
            }
        }

        public static void RedirectToLogin(HttpContext context, string returnUrl, UrlSignDomain domain)
        {
            string redirectUrl;

            switch (domain)
            {
                case UrlSignDomain.Portal:

                    redirectUrl = string.Format("\\CA\\Get_VTicket?fromHost={0}&returnUrl={1}&loginUrl={2}",
                                                HttpUtility.UrlEncode(DfsConfig.Instance.SecurityConfig.PortalHost),
                                                HttpUtility.UrlEncode(returnUrl),
                                                HttpUtility.UrlEncode(DfsConfig.Instance.SecurityConfig.PortalLoginUrl));
                    break;

                default: // default to tms

                    redirectUrl = String.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, HttpUtility.UrlEncode(returnUrl));
                    break;
            }

            context.Response.Redirect(redirectUrl);
        }

        private static string GetEncryptedCookie(HttpContext context, string name)
        {
            var cookie = GetCookie(context, name);
            return string.IsNullOrEmpty(cookie) ? null : EncryptHelper.Decode(cookie);
        }

        private static string GetCookie(HttpContext context, string name)
        {
            var cookie = context.Request.Cookies[name];
            return cookie != null ? cookie.Value : null;
        }
    }
}
