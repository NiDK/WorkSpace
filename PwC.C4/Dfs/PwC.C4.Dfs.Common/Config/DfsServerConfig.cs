using System.Xml.Serialization;
using PwC.C4.Configuration;

namespace PwC.C4.Dfs.Common.Config
{
    [XmlRoot("DfsServerConfig")]
    public class DfsServerConfig : BaseConfig<DfsServerConfig>
    {
        [XmlElement("ServerIpAddress")]
        public string ServerIpAddress { get; set; }

        [XmlElement("ServerPort")]
        public int ServerPort { get; set; }

        [XmlElement("ServerRootFolder")]
        public string ServerRootFolder { get; set; }

    }
}
