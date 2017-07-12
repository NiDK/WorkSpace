using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Infrastructure.Config
{
    [Serializable, XmlRoot("node")]
    public class SettingNode
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlText]
        public string Content { get; set; }
        [XmlArray("nodes"), XmlArrayItem("node")]
        public List<SettingNode> Nodes { get; set; }
    }
}
