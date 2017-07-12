using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Metadata.Config
{
    [Serializable, XmlRoot("Property")]
    public class MetadataEntityColumnProperty
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
