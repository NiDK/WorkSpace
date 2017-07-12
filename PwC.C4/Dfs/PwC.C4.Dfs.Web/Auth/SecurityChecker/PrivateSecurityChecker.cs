using System.Web;
using System.Web.Security;
using PwC.C4.Common.Exceptions;
using PwC.C4.Common.Provider;
using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common.Model.Enums;

namespace PwC.C4.Dfs.Web.Auth.SecurityChecker
{
    public class PrivateSecurityChecker : SecurityChecker
    {
        public override SecurityVerifyResult CheckSecurity(HttpContext       context,
                                                           string            url,
                                                           string appCode,
                                                           SignatureContext  signatureContext,
                                                           out SecurityLevel securityLevel)
        {
            securityLevel = SecurityLevel.Public;

            if (!signatureContext.HasStaffId)
            {
                return SecurityVerifyResult.NoUserIdFound;
            }

            string ticket;
            if (!CheckCookie(context, out ticket))
            {
                return SecurityVerifyResult.UserNotLogin;
            }

            return CheckUser(ticket, signatureContext.StaffId, out securityLevel);
        }

        private SecurityVerifyResult CheckUser(string ticket, string user, out SecurityLevel securityLevel)
        {
            securityLevel = SecurityLevel.Public;

            var onlineUser = CurrentUserProvider.StaffName;
            if (onlineUser == null)
            {
                throw new NoCurrentUserException("CheckUser error for download file");
            }

            if (CurrentUserProvider.StaffId != user)
            {
                return SecurityVerifyResult.UserMismatch;
            }

            securityLevel = SecurityLevel.Private;
            return SecurityVerifyResult.Success;
        }

        private bool CheckCookie(HttpContext context, out string ticket)
        {
            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                ticket = cookie.Value;
                return true;
            }

            ticket = string.Empty;
            return false;
        }
    }
}