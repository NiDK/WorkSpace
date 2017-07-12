using System.Collections.Generic;

namespace PwC.C4.Scheduler.Service.Model
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
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
