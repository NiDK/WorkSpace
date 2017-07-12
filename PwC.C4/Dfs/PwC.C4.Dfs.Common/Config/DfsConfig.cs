using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Ccr.Core;
using PwC.C4.Configuration;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Common.Config
{
    [XmlRoot("DfsConfig")]
    public class DfsConfig : BaseConfig<DfsConfig>
    {
        [XmlElement("UploadSecurityToken")]
        public string UploadSecurityToken { get; set; }

        [XmlElement("MaxFileSizeBytes")]
        public long MaxFileSizeBytes { get; set; }

        [XmlElement("FileBlockSizeBytes")]
        public int FileBlockSizeBytes { get; set; }

        [XmlElement("SignatureLifetimeMinutes")]
        public int SignatureLifetimeMinutes { get; set; }

        [XmlElement("MaxBatchStoreSize")]
        public int MaxBatchStoreSize { get; set; }

        [XmlElement("MaxBatchGetSize")]
        public int MaxBatchGetSize { get; set; }

        [XmlElement("DfsUploadServer")]
        public string DfsUploadServer { get; set; }

        [XmlElement("DfsUploadPort")]
        public int DfsUploadPort { get; set; }

        [XmlElement("PacketSize")]
        public int PacketSize { get; set; }

        [XmlElement("ClientUrlDomain")]
        public string ClientUrlDomain { get; set; }

        private Uri clientUrlDomainUri;
        internal Uri ClientUrlDomainUri
        {
            get
            {
                if (clientUrlDomainUri == null)
                {
                    clientUrlDomainUri = new Uri(ClientUrlDomain);
                }
                return clientUrlDomainUri;
            }
        }

        [XmlArray("FileTypeConfigs")]
        [XmlArrayItem("FileType")]
        public FileTypeConfigCollection FileTypeConfigs { get; set; }

        [XmlArray("FileMimeMappings")]
        [XmlArrayItem("File")]
        public FileMimeMappingCollection FileMimeMappings { get; set; }

        [XmlArray("KeyspaceConfigs")]
        [XmlArrayItem("Keyspace")]
        public KeyspaceConfigCollection KeyspaceConfigs { get; set; }

        [XmlElement("UploadQueueConfig")]
        public QueueConfig UploadQueueConfig { get; set; }

        [XmlElement("GetQueueConfig")]
        public QueueConfig GetQueueConfig { get; set; }

        public SecurityConfig SecurityConfig { get; set; }

        #region ctor

        public DfsConfig()
        {
            MaxFileSizeBytes = 5242880; // 5M
            FileBlockSizeBytes = 5242880; // 5M

            SignatureLifetimeMinutes = 30;

            MaxBatchStoreSize = 100;
            MaxBatchGetSize = 100;

            UploadQueueConfig = new QueueConfig();
            GetQueueConfig = new QueueConfig();

            SecurityConfig = new SecurityConfig();
        }

        #endregion

        public string GetMimeType(string extension)
        {
            ArgumentHelper.AssertNotEmpty(extension);

            FileMimeMapping config;
            if (TryGetValue(FileMimeMappings, extension.ToLower(), out config))
            {
                return config.MimeType;
            }

            return string.Empty;
        }

        public string GetKeyspace(string fileType)
        {
            ArgumentHelper.AssertNotEmpty(fileType);

            FileTypeConfig config;
            if (TryGetValue(FileTypeConfigs, fileType, out config))
            {
                return config.Keyspace;
            }

            return null;
        }

        public Uri GetClientUrlDomain(string keyspace)
        {
            ArgumentHelper.AssertNotEmpty(keyspace);

            KeyspaceConfig config;
            if (TryGetValue(KeyspaceConfigs, keyspace, out config))
            {
                if (config.ClientUrlDomainUri != null)
                    return config.ClientUrlDomainUri;
            }

            return ClientUrlDomainUri;
        }

        public SecurityLevel GetSecurityLevel(string keyspace,string appCode)
        {
            ArgumentHelper.AssertNotEmpty(keyspace);

            KeyspaceConfig config;
            if (TryGetValue(KeyspaceConfigs, keyspace, out config))
            {
                return config.GetSecurityLevel(appCode);
            }

            return SecurityLevel.Public;
        }

        public long GetMaxFileSize(string keyspace)
        {
            ArgumentHelper.AssertNotEmpty(keyspace);

            KeyspaceConfig config;
            if (TryGetValue(KeyspaceConfigs, keyspace, out config))
            {
                if (config.MaxFileSizeBytes > 0)
                    return config.MaxFileSizeBytes;
            }

            return MaxFileSizeBytes;
        }

        private bool TryGetValue<T>(KeyedCollection<string, T> collection, string key, out T value)
        {
            if (collection != null && collection.Contains(key))
            {
                value = collection[key];
                return true;
            }

            value = default(T);
            return false;
        }
    }

    #region FileTypeConfig

    public class FileTypeConfigCollection : KeyedCollection<string, FileTypeConfig>
    {
        protected override string GetKeyForItem(FileTypeConfig item)
        {
            return item.Name;
        }
    }

    public class FileTypeConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Keyspace")]
        public string Keyspace { get; set; }
    }

    #endregion

    #region MimeMappingConfig

    public class FileMimeMappingCollection : KeyedCollection<string, FileMimeMapping>
    {
        protected override string GetKeyForItem(FileMimeMapping item)
        {
            return item.Extension.ToLower();
        }
    }

    public class FileMimeMapping
    {
        [XmlAttribute("Extension")]
        public string Extension { get; set; }

        [XmlAttribute("MimeType")]
        public string MimeType { get; set; }
    }

    #endregion

    #region KeyspaceConfig

    public class KeyspaceConfigCollection : KeyedCollection<string, KeyspaceConfig>
    {
        public KeyspaceConfigCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        protected override string GetKeyForItem(KeyspaceConfig item)
        {
            return item.Name;
        }
    }

    public class KeyspaceConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ClientUrlDomain")]
        public string ClientUrlDomain { get; set; }

        private Uri _uri;
        internal Uri ClientUrlDomainUri
        {
            get
            {
                if (_uri == null && !string.IsNullOrEmpty(ClientUrlDomain))
                {
                    _uri = new Uri(ClientUrlDomain);
                }
                return _uri;
            }
        }

        [XmlAttribute("SecurityLevel")]
        public SecurityLevel SecurityLevel { get; set; }

        [XmlAttribute("MaxFileSizeBytes")]
        public long MaxFileSizeBytes { get; set; }

        [XmlElement("Application")]
        public ApplicationConfig[] ApplicationConfigs { get; set; }

        private Dictionary<string, ApplicationConfig> _map;
        private Dictionary<string, ApplicationConfig> Map
        {
            get
            {
                if (_map == null)
                {
                    var map = new Dictionary<string, ApplicationConfig>();
                    if (ApplicationConfigs != null)
                    {
                        foreach (var config in ApplicationConfigs)
                            map[config.AppCode] = config;
                    }
                    _map = map;
                }
                return _map;
            }
        }

        #region ctor

        public KeyspaceConfig()
        {
            this.SecurityLevel = SecurityLevel.Public;
        }

        #endregion

        public SecurityLevel GetSecurityLevel(string appCode)
        {
            ApplicationConfig config;
            if (Map.TryGetValue(appCode, out config))
                return config.SecurityLevel;
            return SecurityLevel;
        }
    }

    public class ApplicationConfig
    {
        [XmlAttribute("AppCode")]
        public string AppCode { get; set; }

        [XmlAttribute("SecurityLevel")]
        public SecurityLevel SecurityLevel { get; set; }

        public ApplicationConfig()
        {
            this.SecurityLevel = SecurityLevel.Public;
        }
    }



    #endregion

    #region QueueConfig

    public class QueueConfig
    {
        [XmlElement("ThreadCount")]
        public int ThreadCount { get; set; }

        [XmlElement("ThreadPriority")]
        public ThreadPriority ThreadPriority { get; set; }

        [XmlElement("UseBackgroundThreads")]
        public bool UseBackgroundThreads { get; set; }

        [XmlElement("ThrottlePolicy")]
        public TaskExecutionPolicy ThrottlePolicy { get; set; }

        [XmlElement("MaxQueueDepth")]
        public int MaxQueueDepth { get; set; }

        public QueueConfig()
        {
            ThreadCount = 10;
            ThreadPriority = ThreadPriority.Normal;
            UseBackgroundThreads = true;
            ThrottlePolicy = TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks;
            MaxQueueDepth = 10000;
        }
    }

    #endregion

    #region SecurityConfig

    public class SecurityConfig
    {
        [XmlArray("OAuth2DfsReadRoles")]
        [XmlArrayItem("Role")]
        public List<Guid> OAuth2DfsReadRoles { get; set; }

        public string PortalHost { get; set; }
        public string PortalLoginUrl { get; set; }

        public SecurityConfig()
        {
            this.PortalHost = "http://Localhost/Dfs";
            this.PortalLoginUrl = "http://Localhost/Dfs";
        }
    }

    #endregion


    #region File Converter


    public class FileConverter
    {
        [XmlAttribute("appcode")]
        public string AppCode { get; set; }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlArray("ConvertSettings")]
        [XmlArrayItem("ConvertSetting")]
        public List<ConvertSetting> ConvertSettings { get; set; } 
    }

    public class ConvertSettingCollection : KeyedCollection<string, ConvertSetting>
    {
        public ConvertSettingCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        protected override string GetKeyForItem(ConvertSetting item)
        {
            return item.Type;
        }
    }

    public class ConvertSetting
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("ClientUrlDomain")]
        public string ClientUrlDomain { get; set; }
    }

    #endregion
}
