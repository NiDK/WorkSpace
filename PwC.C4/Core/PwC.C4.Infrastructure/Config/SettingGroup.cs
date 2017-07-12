using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Infrastructure.Config
{
    [Serializable, XmlRoot("group")]
    public class SettingGroup
    {
        [XmlAttribute("name")]
        public string GroupName { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlArray("nodes"), XmlArrayItem("node")]
        public List<SettingNode> Nodes { get; set; }
    }
}
