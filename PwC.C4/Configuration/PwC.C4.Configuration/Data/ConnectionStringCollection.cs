using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Serialization;

namespace PwC.C4.Configuration.Data
{
    public class ConnectionStringEntry
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("connectionString")]
        public string ConnectionString;
        
        [XmlAttribute("providerName")]
        public string ProviderName;
    }

    [XmlRoot(ConnectionStringCollection.SectionName)]
    public class ConnectionStringCollection : BaseConfig<ConnectionStringCollection>, IPostSerializer
    {
        private const string SectionName = "ConnectionStrings";
        private static ConnectionStringCollection _instances;
        internal static object SyncRoot = new object();
        static ConnectionStringCollection()
        {
            ConfigChanged += ConnectionStringCollectionConfigChanged;
            _instances = RemoteConfigurationManager.Instance.GetSection<ConnectionStringCollection>(SectionName);
            if (_instances == null)
            {
                _instances = LocalConfigurationManager.Instance.GetSection<ConnectionStringCollection>(SectionName);
            }

        }

        private static void ConnectionStringCollectionConfigChanged(object sender, EventArgs e)
        {
            LoadCache(Instance);
        }


        static void LoadCache(ConnectionStringCollection settings)
        {
            _instances = settings;
                //RemoteConfigurationManager.Instance.GetSection<ConnectionStringCollection>(SectionName);
        }

        static EventHandler _handler;

        public static void RegisterConfigChangedNotification(EventHandler handler)
        {
            _handler += handler;
        }

        public static ConnectionStringCollection Instances
        {
            get
            {
                return _instances;
            }
            set
            {
                _instances = value;
                if (_handler != null)
                    _handler(value, EventArgs.Empty);
            }
        }

        [XmlElement("add")]
        public ConnectionStringEntry[] Entries;

        private NameValueCollection collection = new NameValueCollection();

        [XmlIgnore]
        public string this[string name]
        {
            get
            {
                return collection[name];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return collection.GetEnumerator();
        }


        #region IPostSerializer 成员

        /// <summary>
        /// combine local connectionString and remote connectionString together, local connection string will be the final if the name is conflict
        /// </summary>
        public void PostSerializer()
        {
            if (Entries != null)
            {
                foreach (ConnectionStringEntry entry in Entries)
                {
                    collection[entry.Name]= entry.ConnectionString;
                }
            }

            foreach (ConnectionStringSettings entry in ConfigurationManager.ConnectionStrings)
            {
                collection[entry.Name] = entry.ConnectionString;
            }
        }

        #endregion
    }
}
