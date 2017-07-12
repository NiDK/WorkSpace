using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PwC.C4.Configuration.Maintenance
{
    [XmlRoot("Maintenance")]
    public class MaintenanceConfig : BaseConfig<MaintenanceConfig>
    {
        [XmlElement("Function")]
        public FunctionConfig[] FunctionConfigs { get; set; }

        private Dictionary<string, FunctionConfig> functionConfigMap = null;

        [XmlIgnore]
        public Dictionary<string, FunctionConfig> FunctionConfigMap
        {
            get
            {
                if (functionConfigMap == null)
                {
                    var map = new Dictionary<string, FunctionConfig>();
                    if (FunctionConfigs != null && FunctionConfigs.Length > 0)
                    {
                        foreach (var config in FunctionConfigs)
                            map.Add(config.Name, config);
                    }
                    functionConfigMap = map;
                }
                return functionConfigMap;
            }
        }

        public bool IsFunctionEnabled(string func)
        {
            if (string.IsNullOrEmpty(func))
                throw new ArgumentOutOfRangeException("Function must be non-empty");

            FunctionConfig functionConfig;
            if (FunctionConfigMap.TryGetValue(func, out functionConfig))
                return functionConfig.Enabled;
            return true;
        }
    }

    public class FunctionConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        public FunctionConfig()
        {
            Enabled = true;
        }
    }
}
