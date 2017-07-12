using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PwC.C4.Configuration;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Metadata.Config
{
    [Serializable, XmlRoot("Metadata-Settings")]
    public class MetadataSettings : BaseConfig<MetadataSettings>
    {

        static MetadataSettings()
        {
            ConfigChanged += MetadataSettingsConfigChanged;
            
        }

        private static void MetadataSettingsConfigChanged(object sender, EventArgs e)
        {
            //LoadCache(Instance);
            MetadataSettingsExtension.ReloadConfig();
            CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-MetadataRender_metadataControlDic", true);
            CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-HtmlExtend_metadataControlDic", true);
            CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-HtmlExtend_metadataEntityControlDic", true);
        }

        internal const string DirectoryKeyFormat = "{0}\\{1}";
        internal const string DicrectoryKeyFormatSub = "{0}\\{1}\\{2}";

#if DEBUG
        public
#else
        internal
#endif
            static void LoadCache(MetadataSettings settings)
        {
            if (settings?.Entitys == null)
                return;
            lock (SyncRoot)
            {
                NodesCache.Clear();
                foreach (var settingEntity in settings.Entitys)
                {

                    if (settingEntity.Columns == null)
                        continue;
                    foreach (var settingColumn in settingEntity.Columns)
                    {
                        var dicKey1 = string.Format(DirectoryKeyFormat, settingEntity.EntityName, settingColumn.Name);
                        var dicKey2 = string.Format(DirectoryKeyFormat, "0", settingColumn.Name);
                        if (!NodesCache.ContainsKey(dicKey1))
                        {
                            NodesCache.Add(dicKey1, settingColumn);
                        }

                        if (!NodesCache.ContainsKey(dicKey2))
                        {
                            NodesCache.Add(dicKey2, settingColumn);
                        }
                    }

                }
            }
        }

        [XmlArray("Entitys"), XmlArrayItem("Entity")]
        public List<MetadataEntity> Entitys { get; set; }

        internal static object SyncRoot = new object();

#if DEBUG
        public
#else
        internal
#endif
            static Dictionary<string, MetadataEntityColumn> NodesCache =
                new Dictionary<string, MetadataEntityColumn>(StringComparer.CurrentCultureIgnoreCase);

    }
}
