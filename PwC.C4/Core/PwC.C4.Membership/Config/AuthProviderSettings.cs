using System;
using System.Xml.Serialization;

namespace PwC.C4.Membership.Config
{
    [Serializable, XmlRoot("AuthProviderSettings")]
    public class AuthProviderSettings
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("CacheExpiration")]
        public int CacheExpiration { get; set; }
        [XmlElement("MobileAuthTicketTimeout")]
        public int MobileAuthTicketTimeout { get; set; }
        [XmlElement("WebAuthTicketTimeout")]
        public int WebAuthTicketTimeout { get; set; }
        [XmlElement("MobileCookieTimeout")]
        public int MobileCookieTimeout { get; set; }
        [XmlElement("WebCookieTimeout")]
        public int WebCookieTimeout { get; set; }
        [XmlElement("FormAutenLoginUrl")]
        public string FormAutenLoginUrl { get; set; }
        [XmlElement("UnAuthorizedPageUrl")]
        public string UnAuthorizedPageUrl { get; set; }
        [XmlElement("CookiePrefix")]
        public string CookiePrefix { get; set; }
        [XmlElement("SessionPrefix")]
        public string SessionPrefix { get; set; }
    }
}