using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Metadata.Config
{
    [Serializable, XmlRoot("Entity")]
    public class MetadataEntity
    {
        [XmlAttribute("name")]
        public string EntityName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Workflow")]
        public MetadataEntityWorkflow Workflow { get; set; }

        [XmlElement("DefaultColumn")]
        public string DefaultColumn { get; set; }

        [XmlElement("SearchColumn")]
        public string SearchColumn { get; set; }

        [XmlElement("EntityTranslatedColumn")]
        public string EntityTranslatedColumn { get; set; }

        [XmlElement("ListTranslatedColumn")]
        public string ListTranslatedColumn { get; set; }

        [XmlElement("StringFormatColumn")]
        public string StringFormatColumn { get; set; }

        [XmlElement("JoinFormatColumn")]
        public string JoinFormatColumn { get; set; }

        [XmlElement("ArrayColumn")]
        public string ArrayColumn { get; set; }

        [XmlArray("Columns"), XmlArrayItem("Column")]
        public List<MetadataEntityColumn> Columns { get; set; }
    }

}
