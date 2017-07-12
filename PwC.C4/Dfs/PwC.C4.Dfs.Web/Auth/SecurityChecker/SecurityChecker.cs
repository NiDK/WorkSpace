using System.Web;
using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common.Model.Enums;

namespace PwC.C4.Dfs.Web.Auth.SecurityChecker
{
    public abstract class SecurityChecker
    {
        public SecurityVerifyResult Check(HttpContext       context, 
                                          string            url,
                                          string appCode,
                                          SignatureContext  signatureContext,
                                          out SecurityLevel securityLevel)
        {
            if (!CheckSignature(url))
            {
                securityLevel = SecurityLevel.Public;
                return SecurityVerifyResult.InvalidUrl;
            }

            return CheckSecurity(context, url, appCode, signatureContext, out securityLevel);
        }

        private bool CheckSignature(string url)
        {
            return DfsUrlHelper.IsValidSignedUrl(url);
        }

        public abstract SecurityVerifyResult CheckSecurity(HttpContext       context, 
                                                           string            url,
                                                           string appCode,
                                                           SignatureContext  signatureContext,
                                                           out SecurityLevel securityLevel);
    }
}
