using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Metadata.Config
{
    [Serializable, XmlRoot("Column")]
    public class MetadataEntityColumn
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("size")]
        public int Size { get; set; }
        [XmlAttribute("pk")]
        public bool IsPk { get; set; }
        [XmlAttribute("indexable")]
        public bool Indexable { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlArray("Properties"), XmlArrayItem("Property")]
        public List<MetadataEntityColumnProperty> BaseProperties { get; set; }
        [XmlIgnore]
        public Dictionary<string, string> Properties
        {
            get { return BaseProperties.ToDictionary(c => c.Key, c => c.Value); }
        }

    }
}
