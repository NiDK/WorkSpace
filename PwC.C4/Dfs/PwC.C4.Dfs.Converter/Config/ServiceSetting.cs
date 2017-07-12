using System.Xml.Serialization;

namespace PwC.C4.Dfs.Converter.Config
{
    public class ServiceSetting
    {
        [XmlElement("Interval")]
        public int Interval { get; set; }

        [XmlElement("Buffer")]
        public int Buffer { get; set; }
    }
}