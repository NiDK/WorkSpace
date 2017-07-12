using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Serialization;

namespace PwC.C4.Configuration
{
    public class AppSettingEntry
    {
        [XmlAttribute("key")]
        public string Key;
        [XmlAttribute("value")]
        public string Value;
    }

    [XmlRoot(AppSettingCollection.SectionName)]
    public class AppSettingCollection : IPostSerializer
    {
        private const string SectionName = "appSettings";
        static AppSettingCollection()
        {
            instance = RemoteConfigurationManager.Instance.GetSection<AppSettingCollection>(SectionName);
        }
        private static AppSettingCollection instance;

        public static AppSettingCollection Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                if (_handler != null)
                    _handler(value, EventArgs.Empty);
            }
        }


        static EventHandler _handler;
        public static void RegisterConfigChangedNotification(EventHandler handler)
        {
            _handler += handler;
        }

        private NameValueCollection collection = new NameValueCollection();

        [XmlElement("add")]
        public AppSettingEntry[] Entries;

        [XmlIgnore]
        public string this[string key]
        {
            get
            {
                return collection[key];
            }
        }

        #region IPostSerializer 成员

        public void PostSerializer()
        {
            if (Entries != null)
            {
                foreach (AppSettingEntry entry in Entries)
                {
                    collection[entry.Key] = entry.Value;
                }

            }

            //merge together
            foreach (string key in ConfigurationManager.AppSettings)
            {
                collection[key] = ConfigurationManager.AppSettings[key];
            }
        }

        #endregion
    }
}
