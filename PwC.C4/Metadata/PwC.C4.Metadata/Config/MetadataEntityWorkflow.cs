using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PwC.C4.Metadata.Config
{
    [Serializable, XmlRoot("Workflow")]
    public class MetadataEntityWorkflow
    {
        [XmlAttribute("enable")]
        public bool Enable { get; set; }
        [XmlAttribute("code")]
        public string Code { get; set; }
    }
}
