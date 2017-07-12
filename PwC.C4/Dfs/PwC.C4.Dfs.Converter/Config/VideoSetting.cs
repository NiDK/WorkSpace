using System.Xml.Serialization;

namespace PwC.C4.Dfs.Converter.Config
{
    public class VideoSetting
    {
        [XmlAttribute("Size")]
        public string Size { get; set; }
        [XmlAttribute("AppendSilentAudioStream")]
        public bool AppendSilentAudioStream { get; set; }
        [XmlAttribute("VideoCodec")]
        public string VideoCodec { get; set; }

        //[XmlAttribute("AudioCodec")]
        //public string AudioCodec { get; set; }

        //[XmlAttribute("AudioSampleRate")]
        //public int AudioSampleRate { get; set; }

        //[XmlAttribute("VideoFrameCount")]
        //public int VideoFrameCount { get; set; }

        [XmlAttribute("VideoFrameRate")]
        public int VideoFrameRate { get; set; }

        [XmlAttribute("VideoFrameSize")]
        public string VideoFrameSize { get; set; }

        [XmlAttribute("CustomOutputArgs")]
        public string CustomOutputArgs { get; set; }

        [XmlAttribute("CreateCapture")]
        public bool CreateCapture { get; set; }
    }
}