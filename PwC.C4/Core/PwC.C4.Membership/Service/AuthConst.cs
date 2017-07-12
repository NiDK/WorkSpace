using System;
using System.Collections.Generic;
using PwC.C4.Membership.Config;
using PwC.C4.Membership.Model.Enum;

namespace PwC.C4.Membership.Service
{
    internal static class AuthConst
    {
        public const string ApplicationCenter = "ApplicationCenter";
        public const string vProfile = "vProfile";
        public const string TokenName = "HTTP_AUTHORIZATION";
        public const string ServiceTicket = "ServiceTicket";
        public const string CookieUserName = "UserName";
        public const string StaffAndOrgCode = "PwC.OASC";
        public const string CurrentAuthMode = "PwC.CAM";
        public const string vProfileSuccessCode = "Success";
        public static string AppCookiePath(string appCode, AuthProvider provider)
        {
            if (provider == AuthProvider.DebugUser)
            {
                return $"DebugUserLoginStatus-{appCode}";
            }
            return $"{CookiePrefix(provider)}{appCode}";
        }

        public static string CookiePrefix(AuthProvider provider)
        {
            var me = MembershipSettings.Instance.GetAuthProviderSettings(provider);
            return me.CookiePrefix;
        }

        public static string SessionPrefix(AuthProvider provider)
        {
            var me = MembershipSettings.Instance.GetAuthProviderSettings(provider);
            return me.SessionPrefix;
        }

        public static string TokenInCookie(AuthProvider provider)
        {
            var me = CookiePrefix(provider);
            return me + "TokenInCookie";
        }

        public static Dictionary<string, AuthProvider> AuthN2E =
            new Dictionary<string, AuthProvider>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"ApplicationCenter", AuthProvider.ApplicationCenter},
                {"vProfile", AuthProvider.vProfile}
            };

        public static Dictionary<AuthProvider, string> AuthE2N = new Dictionary<AuthProvider, string>()
        {
            {AuthProvider.ApplicationCenter, "ApplicationCenter"},
            {AuthProvider.vProfile, "vProfile"}
        };

        public static Dictionary<string,string> vProfileVerifyStatus = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase); 
    }
}
