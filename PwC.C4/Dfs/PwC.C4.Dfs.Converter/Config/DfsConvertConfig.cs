using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using PwC.C4.Configuration;
using static PwC.C4.Dfs.Common.Model.Const;

namespace PwC.C4.Dfs.Converter.Config
{

    [XmlRoot("DfsConvertConfig")]
    public class DfsConvertConfig : BaseConfig<DfsConvertConfig>
    {
        [XmlElement("EnableConvertApps")]
        public string EnableConvertApps { get; set; }

        [XmlElement("ServiceSetting")]
        public ServiceSetting ServiceSetting { get; set; }

        [XmlArray("ConvertInfos"), XmlArrayItem("ConvertInfo")]
        public List<ConvertInfo> ConvertInfos { get; set; }

    }
}
