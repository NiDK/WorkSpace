using System.Collections.Generic;
using System.IO;
using System.Linq;
using PwC.C4.Testing.Scheduler.ServiceConsole.Model;

namespace PwC.C4.Testing.Scheduler.ServiceConsole
{
    public static class PluginConfigsExtension
    {
        public static PluginConfigs LoadConfig()
        {
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, "PluginConfigs.config");
            return PwC.C4.Configuration.XmlSerializer<PluginConfigs>.DeserializeFromFile(filePath);
        }

        public static List<PluginSetting> GetEnablePlugin()
        {
            var e = LoadConfig();
            return e.PluginSettings.Where(c => c.Enable).ToList();
        }

    }
}
