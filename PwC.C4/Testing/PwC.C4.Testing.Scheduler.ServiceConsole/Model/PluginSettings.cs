using System.Collections.Generic;
using System.Xml.Serialization;

namespace PwC.C4.Testing.Scheduler.ServiceConsole.Model
{

    public class PluginConfigs
    {
        public List<PluginSetting> PluginSettings { get; set; }
    }

    public class PluginSetting
    {
        public string Code { get; set; }

        public bool Enable { get; set; }

        public string AssemblyInfo { get; set; }

        public string CornExpression { get; set; }

        public List<Parameter> Parameters { get; set; }

        [XmlIgnore]
        public Dictionary<string, string> ParameterDic
        {
            get
            {
                var newDic = new Dictionary<string,string>();
                this.Parameters.ForEach(c =>
                {
                    newDic.Add(c.Name,c.Value);
                });
                return newDic;
            } 
        }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
