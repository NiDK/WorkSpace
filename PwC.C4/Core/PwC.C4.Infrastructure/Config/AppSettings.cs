using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PwC.C4.Configuration;

namespace PwC.C4.Infrastructure.Config
{
    [Serializable, XmlRoot("AppSettings")]
    public class AppSettings :BaseConfig<AppSettings>
    {

        static AppSettings()
        {
            ConfigChanged += AppSettingsConfigChanged;
        }

        private static void AppSettingsConfigChanged(object sender, EventArgs e)
        {
           AppSettingsExtension.ClearCache();
        }

        internal const string DirectoryKeyFormat = "{0}\\{1}";
        internal const string DicrectoryKeyFormatSub = "{0}\\{1}\\{2}";

#if DEBUG
        public
#else
        internal
#endif
            static void LoadCache(AppSettings settings)
        {

            if (settings == null)
                return;
            if (settings.Groups == null)
                return;
            lock (SyncRoot)
            {
                NodesCache.Clear();
                foreach (var settingGroup in settings.Groups)
                {

                    if (settingGroup.Nodes == null)
                        continue;
                    foreach (var settingNode in settingGroup.Nodes)
                    {
                        var dicKey1 = string.Format(DirectoryKeyFormat, settingGroup.GroupName, settingNode.Key);
                        var dicKey2 = string.Format(DirectoryKeyFormat, "0", settingNode.Key);
                        if (!NodesCache.ContainsKey(dicKey1))
                        {
                            NodesCache.Add(dicKey1, settingNode);
                        }

                        if (!NodesCache.ContainsKey(dicKey2))
                        {
                            NodesCache.Add(dicKey2, settingNode);
                        }
                    }

                }
            }
        }

        [XmlArray("groups"), XmlArrayItem("group")]
        public List<SettingGroup> Groups { get; set; }

        internal static object SyncRoot = new object();

#if DEBUG
        public
#else
        internal
#endif
            static Dictionary<string, SettingNode> NodesCache =
                new Dictionary<string, SettingNode>(StringComparer.CurrentCultureIgnoreCase);

    }
}
