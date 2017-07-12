using System;
using System.Security.Cryptography;
using System.Text;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Client.Helper
{
    public static class DfsUrlHelper
    {
        public static bool IsValidSignedUrl(string signedUrl)
        {
            int sigIndex = signedUrl.IndexOf("sig=", StringComparison.Ordinal);
            if (sigIndex != -1)
            {
                string sig = signedUrl.Substring(sigIndex + 4);
                string expectedSig = GenerateSignature(signedUrl.Substring(0, sigIndex - 1));
                return sig == expectedSig;
            }
            return false;
        }

        public static string SignUrl(SecurityLevel securityLevel, string url, string appCode)
        {
            var context = new SignatureContext
            {
                Timestamp = DateTimeUtility.CurrentTimestamp.ToString(),
                Url = url,
                AppCode = appCode
            };

            return SignUrl(context);
        }

        public static string SignUrl(SecurityLevel securityLevel, string url, UrlSignDomain domain, string staffId)
        {
            if (securityLevel != SecurityLevel.Public)
            {
                ArgumentHelper.AssertNotNullOrEmpty(staffId, "staffId");

                var context = new SignatureContext
                {
                    Timestamp = DateTimeUtility.CurrentTimestamp.ToString(),
                    Url = url,
                    Domain = domain,
                    StaffId = staffId,
                };

                return SignUrl(context);
            }

            return url;
        }

        public static string SignUrl(SecurityLevel securityLevel, string url, UrlSignDomain domain, string email, string staffId)
        {
            if (securityLevel == SecurityLevel.Private)
                ArgumentHelper.AssertNotNullOrEmpty(staffId, "staffId");

            var context = new SignatureContext
            {
                Timestamp = DateTimeUtility.CurrentTimestamp.ToString(),
                Url = url,
                Domain = domain,
                Email = email,
                StaffId = staffId,
            };

            return SignUrl(context);
        }

        public static string SignUrl(SignatureContext context)
        {
            return SignUrl(context.GenerateSignatureBase());
        }

        public static string SignUrl(string url)
        {
            url = url.ToLower();
            return string.Format("{0}&sig={1}", url, GenerateSignature(url));
        }

        private static readonly byte[] signatureSecret = Encoding.ASCII.GetBytes("1c321549-afb8-4663-aa2e-a889695e564b");
        private static string GenerateSignature(string data)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureSecret);
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBytes);

            var result = new StringBuilder(hashBytes.Length * 2);
            for (int i = 0; i < hashBytes.Length; ++i)
                result.Append(hashBytes[i].ToString("x2"));
            return result.ToString();
        }
    }
}
