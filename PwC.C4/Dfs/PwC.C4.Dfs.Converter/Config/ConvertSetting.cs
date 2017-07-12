using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PwC.C4.Dfs.Converter.Config
{
    [Serializable, XmlRoot("ConvertInfo")]
    public class ConvertInfo
    {
        [XmlAttribute("AppCode")]
        public string AppCode { get; set; }

        [XmlAttribute("Enable")]
        public bool Enable { get; set; }

        [XmlAttribute("EnableVideoType")]
        public string EnableVideoType { get; set; }

        [XmlAttribute("EnableImageType")]
        public string EnableImageType { get; set; }

        [XmlArray("ImageSettings"), XmlArrayItem("ImageSetting")]
        public List<ImageSetting> ImageSettings { get; set; }

        [XmlArray("VideoSettings"), XmlArrayItem("VideoSetting")]
        public List<VideoSetting> VideoSettings { get; set; }
    }
}