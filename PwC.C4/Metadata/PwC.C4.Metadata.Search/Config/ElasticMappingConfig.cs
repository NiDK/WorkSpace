using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using PwC.C4.Metadata.Search.Exceptions;
using PwC.C4.Configuration;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Metadata.Search.Config
{
    [XmlRoot("ElasticMapping")]
    public class ElasticMappingConfig : BaseConfig<ElasticMappingConfig>
    {
   
        [XmlElement("ElasticSearchVersion")]
        public string ElasticSearchVersion { get; set; }

        [XmlElement("Trace")]
        public Trace Trace { get; set; }

        [XmlElement("Application")]
        public ApplicationConfigCollection Applications { get; set; }

        public ElasticMappingConfig()
        {
            ElasticSearchVersion = "1.7.3";
            Trace = new Trace();
            ConfigChanged += ElasticMappingConfigChanged;
            Applications = new ApplicationConfigCollection();
        }

        private void ElasticMappingConfigChanged(object sender, EventArgs e)
        {
            Trace = new Trace();
            Applications = new ApplicationConfigCollection();
        }

        public string ElasticIndexName(string entityName,string application=null)
        {
            if (application == null)
            {
                application = AppSettings.Instance.GetAppCode();
            }
            if (Applications.Contains(application))
            {
                return Applications[application].ElasticName(entityName);
            }

            throw new ApplicationNotFoundException(application);
        }

        public ServiceDescription ToServiceDescription()
        {
            return new ServiceDescription
            {
                Name = "ElasticSearch",
                Clusters = Applications.Select(a => a.ToClusterDescription()).ToList()
            };
        }

        public List<string> ElasticNodes(string application = null)
        {
            if (application == null)
            {
                application = AppSettings.Instance.GetAppCode();
            }
            if (Applications.Contains(application))
            {
                var nodes = Applications[application].Nodes;
                if (nodes.Any())
                {
                    return nodes.Where(c=>c.Enabled).Select(nodeConfig => nodeConfig.ToString()).ToList();
                }
                throw new EsNodeNotFoundException(application);
            }
            throw new ApplicationNotFoundException(application);
        }
    }

    public class ApplicationConfigCollection : KeyedCollection<string, ApplicationConfig>
    {
        public ApplicationConfigCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        protected override string GetKeyForItem(ApplicationConfig item)
        {
            return item.Name;
        }
    }

    public class Trace
    {
        [XmlAttribute("SlowQueryThreshold")]
        public int SlowQueryThreshold { get; set; }
        public Trace()
        {
            SlowQueryThreshold = 10000;
        }
    }

    public class ApplicationConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string ElasticIndexName(string entityName)
        {
            var data = this.Entities.FirstOrDefault(c => c.Name == entityName);
            if (data!=null)
            {
                return data.IndexName;
            }
            return string.Empty;
        }

        [XmlElement("ConnectionPool")]
        public ConnectionPoolConfig ConnectionPool { get; set; }

        [XmlArray("Nodes")]
        [XmlArrayItem("Node")]
        public NodeConfig[] Nodes { get; set; }

        [XmlArray("Entities")]
        [XmlArrayItem("Entity")]
        public EntityConfig[] Entities { get; set; }

        #region ctor

        public ApplicationConfig()
        {
            ConnectionPool = new ConnectionPoolConfig();
            Nodes = new NodeConfig[0];

        }

        #endregion

        internal string ElasticName(string entityName)
        {
            return ElasticIndexName(entityName);
        }

        public ClusterDescription ToClusterDescription()
        {
            return new ClusterDescription
            {
                Name = this.Name,

                Nodes = this.Nodes.Where(n => n.Enabled).Select(n => n.ToNodeDescription()).ToList(),

                ConnectionPoolConfig = new ConnectionPool.Config.ConnectionPoolConfig
                {
                    Type = ConnectionPoolType.Concurrent,
                    Size = this.ConnectionPool.Size,
                    ConnectionLifeTimeMinutes = this.ConnectionPool.ConnectionLifeTimeMinutes,

                    SocketConfig = new ConnectionPool.Config.SocketConfig
                    {
                        ConnectTimeout = this.ConnectionPool.Socket.ConnectTimeout,
                        SendTimeout = this.ConnectionPool.Socket.SendTimeout,
                        ReceiveTimeout = this.ConnectionPool.Socket.ReceiveTimeout,
                        SendBufferSize = this.ConnectionPool.Socket.SendBufferSize,
                        ReceiveBufferSize = this.ConnectionPool.Socket.ReceiveBufferSize
                    }
                }
            };
        }
    }

    public class ConnectionPoolConfig
    {
        [XmlAttribute("Size")]
        public int Size { get; set; }

        [XmlAttribute("ConnectionLifeTimeMinutes")]
        public int ConnectionLifeTimeMinutes { get; set; }

        [XmlAttribute("FailureThreshold")]
        public int FailureThreshold { get; set; }

        [XmlAttribute("FailureWindowSeconds")]
        public int FailureWindowSeconds { get; set; }

        [XmlElement("Socket")]
        public SocketConfig Socket { get; set; }

        public ConnectionPoolConfig()
        {
            Size = 10;

            ConnectionLifeTimeMinutes = 60;
            FailureThreshold = 3;
            FailureWindowSeconds = 30;

            Socket = new SocketConfig();
        }
    }

    public class SocketConfig
    {
        [XmlAttribute("ConnectTimeout")]
        public int ConnectTimeout { get; set; }

        [XmlAttribute("SendTimeout")]
        public int SendTimeout { get; set; }

        [XmlAttribute("ReceiveTimeout")]
        public int ReceiveTimeout { get; set; }

        [XmlAttribute("SendBufferSize")]
        public int SendBufferSize { get; set; }

        [XmlAttribute("ReceiveBufferSize")]
        public int ReceiveBufferSize { get; set; }

        public SocketConfig()
        {
            ConnectTimeout = 3000;
            SendTimeout = 10000;
            ReceiveTimeout = 10000;
            SendBufferSize = 8192;
            ReceiveBufferSize = 8192;
        }
    }

    public class NodeConfig
    {
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("Host")]
        public string Host { get; set; }

        [XmlAttribute("Port")]
        public int Port { get; set; }

        public NodeConfig()
        {
            Enabled = true;
            Port = 9500;
        }

        public override string ToString()
        {
            return Host + ":" + Port;
        }

        public NodeDescription ToNodeDescription()
        {
            return new NodeDescription
            {
                Host = this.Host,
                Port = this.Port,

                FailureThreshold = 3,
                FailureWindowSeconds = 30
            };
        }
    }

    public class EntityConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IndexName")]
        public string IndexName { get; set; }

    }

}
