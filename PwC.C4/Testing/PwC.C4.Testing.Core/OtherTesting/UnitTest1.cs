using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.Dfs.Common;

namespace PwC.C4.Testing.Core.Other
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            DateTime datev;
            System.IFormatProvider format = new System.Globalization.CultureInfo("zh-HK", true);
            DateTime.TryParse("20/12/2016 10:12", format, DateTimeStyles.AdjustToUniversal, out datev);
            var str = datev.ToString(CultureInfo.InvariantCulture);
        }
        [TestMethod]
        public void CreateMembershipSettings()
        {
            MembershipSettings m = new MembershipSettings();
            m.Auths = new List<AuthProvider>();
            var au1 = new AuthProvider
            {
                Name = "ApplicationCenter",
                Settings = new Settings
                {
                    CacheExpiration = 3000,
                    CookiePrefix = "CASCookie|" + Guid.NewGuid() + "|",
                    SessionPrefix = "Session|" + Guid.NewGuid() + "|",
                    AdTwoFactorLoginPageUrl =
                        "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/mobile/pgTwoFactorADVerify.aspx",
                    FormAutenLoginUrl = "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/pglogin.aspx",
                    UnAuthorizedPageUrl = "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/pgUnAuthorized.aspx",
                    SmsTwoFactorLoginPageUrl =
                        "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/mobile/pgTwoFactorADVerify.aspx",
                    MobileAuthTicketTimeout = 525600,
                    MobileCookieTimeout = 525600,
                    WebAuthTicketTimeout = 600,
                    WebCookieTimeout = 600
                }
            };
            m.Auths.Add(au1);
            var au2 = new AuthProvider
            {
                Name = "vProfile",
                Settings = new Settings
                {
                    CacheExpiration = 3000,
                    CookiePrefix = "CASCookie|" + Guid.NewGuid() + "|",
                    SessionPrefix = "Session|" + Guid.NewGuid() + "|",
                    AdTwoFactorLoginPageUrl =
            "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/mobile/pgTwoFactorADVerify.aspx",
                    FormAutenLoginUrl = "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/pglogin.aspx",
                    UnAuthorizedPageUrl = "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/pgUnAuthorized.aspx",
                    SmsTwoFactorLoginPageUrl =
            "http://cn-pekappdev025:9008/ApplicationCenter.Weblogin/mobile/pgTwoFactorADVerify.aspx",
                    MobileAuthTicketTimeout = 525600,
                    MobileCookieTimeout = 525600,
                    WebAuthTicketTimeout = 600,
                    WebCookieTimeout = 600
                }
            };
            m.Auths.Add(au2);

            var xml = PwC.C4.Configuration.XmlSerializer<MembershipSettings>.Serializer(m);
        }
    }

    public class MembershipSettings
    {
        public List<AuthProvider> Auths { get; set; }
    }

    public class Settings
    {
        public int CacheExpiration { get; set; }
        public int MobileAuthTicketTimeout { get; set; }
        public int WebAuthTicketTimeout { get; set; }
        public int MobileCookieTimeout { get; set; }
        public int WebCookieTimeout { get; set; }
        public string FormAutenLoginUrl { get; set; }
        public string UnAuthorizedPageUrl { get; set; }
        public string CookiePrefix { get; set; }
        public string SessionPrefix { get; set; }
        public string AdTwoFactorLoginPageUrl { get; set; }
        public string SmsTwoFactorLoginPageUrl { get; set; }
    }



    public class AuthProvider
    {
        public string Name { get; set; }
        public Settings Settings { get; set; }
    }
}
