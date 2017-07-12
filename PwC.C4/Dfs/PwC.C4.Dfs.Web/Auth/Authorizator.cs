using System.Web;
using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Dfs.Web.Auth.SecurityChecker;
using PwC.C4.Dfs.Web.Services;

namespace PwC.C4.Dfs.Web.Auth
{
    public static class Authorizator
    {
        public static bool CheckAccess(SecurityLevel securityLevel, HttpContext context, string url, string appCode)
        {
            return CheckAccessImpl(securityLevel, context, url, appCode);
        }



        private static bool CheckAccessImpl(SecurityLevel securityLevel, HttpContext context, string url,  string appCode)
        {
            switch (securityLevel)
            {
                case SecurityLevel.Public:
                    return true;

                case SecurityLevel.Protected:
                    return CheckProtectedAccess(context, url, appCode);

                default:
                    return CheckPrivateAccess(context, url, appCode);
            }
        }

        private static readonly SecurityChecker.SecurityChecker privateChecker = new PrivateSecurityChecker();

        private static bool CheckPrivateAccess(HttpContext context, string url, string appCode)
        {
            var signatureContext = SignatureContext.Parse(url);

            SecurityLevel securityLevel;
            var result = privateChecker.Check(context, url, appCode, signatureContext, out securityLevel);

            switch (result)
            {
                case SecurityVerifyResult.Success:

                    return true;

                case SecurityVerifyResult.UserNotLogin:

                    AuthorizationHelper.RedirectToLogin(context, url, signatureContext.Domain.Value);
                    return false;

                default:

                    PerfCounters.Instance.CountInvalidRequest();
                    DfsHelper.SendErrorResponse(context, 400);
                    return false;
            }
        }

        private static readonly SecurityChecker.SecurityChecker protectedChecker = new ProtectedSecurityChecker();

        private static bool CheckProtectedAccess(HttpContext context, string url, string appCode)
        {
            var signatureContext = SignatureContext.Parse(url);

            SecurityLevel securityLevel;
            var result = protectedChecker.Check(context, url, appCode, signatureContext, out securityLevel);

            switch (result)
            {
                case SecurityVerifyResult.Success:

                    bool captcha = (securityLevel == SecurityLevel.Protected)
                        && (!SecurityPolicy.CaptchaDisabled(signatureContext.SecurityPolicy));

                    if (captcha)
                    {
                        AuthorizationHelper.VerifyCaptcha(context, url);
                    }

                    return true;

                default:

                    PerfCounters.Instance.CountInvalidRequest();
                    DfsHelper.SendErrorResponse(context, 400);
                    return false;
            }
        }
    }
}
