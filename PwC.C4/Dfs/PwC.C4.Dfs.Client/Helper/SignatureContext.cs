using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Client.Helper
{
    public class SignatureContext
    {
        #region Parameter Key Definition

        public const string DomainKey = "sig_d";
        public const string TimestampKey = "sig_t";
        public const string SignatureKey = "sig";
        public const string UserIdKey = "sig_u";
        public const string UserNameKey = "sig_n";
        public const string EmailKey = "sig_e";
        public const string ApplicationKey = "sig_a";
        public const string PolicyMaskKey = "sig_pm";
        public const string NegativePolicyMaskKey = "sig_npm";

        #endregion

        public string Url { get; set; }

        public UrlSignDomain? Domain { get; set; }
        public string Timestamp { get; set; }
        public string Signature { get; set; }

        public string StaffId { get; set; }
        public string Email { get; set; }
        public string AppCode { get; set; }

        public string StaffName { get; set; }

        public SecurityPolicy SecurityPolicy { get; set; }

        #region Helper

        public bool HasSignature
        {
            get { return !string.IsNullOrEmpty(Signature); }
        }

        public bool HasStaffId
        {
            get { return !string.IsNullOrEmpty(StaffId); }
        }

        public bool HasEmail
        {
            get { return !string.IsNullOrEmpty(Email); }
        }

        public bool HasAppCode
        {
            get { return !string.IsNullOrEmpty(AppCode); }
        }

        #endregion

        #region GenerateSignatureBase

        public string GenerateSignatureBase()
        {
            var builder = new StringBuilder(Url);
            builder.Append("?");
            builder.Append(BuildParameters());
            return builder.ToString();
        }

        private string BuildParameters()
        {
            var builder = new StringBuilder();

            foreach (var parameter in AllParameters)
            {
                if (parameter.HasValue)
                {
                    builder.Append(parameter.Name);
                    builder.Append("=");
                    builder.Append(parameter.Value);
                    builder.Append("&");
                }
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private List<BoundParameter> allParameters;
        private List<BoundParameter> AllParameters
        {
            get
            {
                if (allParameters == null)
                {
                    var parameters = new List<BoundParameter>();

                    parameters.Add(new BoundParameter(ApplicationKey, AppCode));
                    parameters.Add(new BoundParameter(DomainKey, ValueToString(Domain)));
                    parameters.Add(new BoundParameter(EmailKey, HttpUtility.UrlEncode(Email)));
                    parameters.Add(new BoundParameter(TimestampKey, Timestamp));
                    parameters.Add(new BoundParameter(UserIdKey, StaffId));
                    parameters.Add(new BoundParameter(UserNameKey, StaffName));
                    parameters.Add(new BoundParameter(PolicyMaskKey, MaskToString(SecurityPolicy)));
                    parameters.Add(new BoundParameter(NegativePolicyMaskKey, NMaskToString(SecurityPolicy)));

                    allParameters = parameters;
                }

                return allParameters;
            }
        }

        private string MaskToString(SecurityPolicy policy)
        {
            if (policy != null)
            {
                return policy.Mask == SecurityPolicy.DefaultMask ? 
                    null : policy.Mask.ToString();
            }

            return null;
        }

        private string NMaskToString(SecurityPolicy policy)
        {
            if (policy != null)
            {
                return policy.NMask == SecurityPolicy.DefaultNMask ? 
                    null : (~policy.NMask).ToString();
            }

            return null;
        }

        private string ValueToString(int value)
        {
            return value > 0 ? value.ToString() : null;
        }

        private string ValueToString(UrlSignDomain? value)
        {
            return value != null ? ((int)value).ToString() : null;
        }

        private class BoundParameter
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public bool HasValue
            {
                get { return !string.IsNullOrEmpty(Value); }
            }

            public BoundParameter(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        #endregion

        #region Parse

        public static SignatureContext Parse(string url)
        {
            ArgumentHelper.AssertNotEmpty(url);

            var parameters = HttpUtility.ParseQueryString(new Uri(url).Query);

            return new SignatureContext
            {
                Url = url,
                Domain = ParseDomain(parameters[DomainKey]),
                Timestamp = parameters[TimestampKey],

                Signature = parameters[SignatureKey],

                StaffId = parameters[UserIdKey],
                StaffName = parameters[UserNameKey],
                Email = parameters[EmailKey],
                AppCode = parameters[ApplicationKey],

                SecurityPolicy = ParseSecurityPolicy(parameters[PolicyMaskKey], parameters[NegativePolicyMaskKey])
            };
        }

        private static UrlSignDomain? ParseDomain(string value)
        {
            int enumValue;
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out enumValue))
            {
                if (Enum.IsDefined(typeof(UrlSignDomain), enumValue))
                    return (UrlSignDomain)Enum.Parse(typeof(UrlSignDomain), value);
            }

            return null;
        }

        private static int ParseInt(string value)
        {
            int result;

            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out result))
                return result;

            return 0;
        }

        private static SecurityPolicy ParseSecurityPolicy(string mask, string nmask)
        {
            var maskValue = string.IsNullOrEmpty(mask) ? SecurityPolicy.DefaultMask : ulong.Parse(mask);
            var nmaskValue = string.IsNullOrEmpty(nmask) ? SecurityPolicy.DefaultNMask : ~ulong.Parse(nmask);
            return new SecurityPolicy(maskValue, nmaskValue);
        }

        #endregion
    }
}
