using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PwC.C4.Configuration;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Membership.Config
{
    [Serializable, XmlRoot("MembershipSettings")]
    public class MembershipSettings : BaseConfig<MembershipSettings>
    {

        static MembershipSettings()
        {
            ConfigChanged += MembershipSettingsConfigChanged;
        }

        private static void MembershipSettingsConfigChanged(object sender, EventArgs e)
        {
            NodesCache.Clear();
        }


#if DEBUG
        public
#else
        internal
#endif
            static void LoadCache(MembershipSettings settings)
        {
            if (settings?.Auths == null)
                return;
            lock (SyncRoot)
            {
                NodesCache.Clear();
                foreach (var settingEntity in settings.Auths)
                {

                    if (settingEntity == null)
                        continue;
                    if (!NodesCache.ContainsKey(settingEntity.Name))
                    {
                        NodesCache.Add(settingEntity.Name, settingEntity);
                    }
                }
            }
        }

#if DEBUG
        public
#else
        internal
#endif
            static Dictionary<string, AuthProviderSettings> NodesCache =
                new Dictionary<string, AuthProviderSettings>(StringComparer.CurrentCultureIgnoreCase);

        internal static object SyncRoot = new object();

        [XmlArray("Auths"), XmlArrayItem("AuthProviderSettings")]
        public List<AuthProviderSettings> Auths { get; set; }



    }
}
