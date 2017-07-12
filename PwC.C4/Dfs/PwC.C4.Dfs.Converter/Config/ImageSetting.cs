using System.Xml.Serialization;

namespace PwC.C4.Dfs.Converter.Config
{
    public class ImageSetting
    {
        [XmlAttribute("Size")]
        public string Size { get; set; }
        [XmlAttribute("Width")]
        public string Width { get; set; }

        [XmlAttribute("Height")]
        public string Height { get; set; }

        [XmlAttribute("Mode")]
        public string Mode { get; set; }

        [XmlAttribute("Quality")]
        public string Quality { get; set; }
    }
}