using System.Xml.Serialization;

namespace PwC.C4.Configuration
{
    [XmlRoot(RemoteConfigurationManagerConfiguration.TagName)]
    public class RemoteConfigurationManagerConfiguration : IXmlSerializable
    {
        public const string TagName = "RemoteConfigurationManager";

        private string applicationName;
        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }

        }

        private int readwriteTimeout;
        public int ReadWriteTimeout
        {
            get
            {
                return readwriteTimeout;
            }
            set
            {
                readwriteTimeout = value;
            }
        }


        private int timeInterval;
        public int TimerInterval
        {
            get
            {
                return timeInterval;
            }
            set
            {
                timeInterval = value;
            }
        }

        private string remoteConfigurationUrl;
        public string RemoteConfigurationUrl
        {
            get
            {
                return remoteConfigurationUrl;
            }
            set
            {
                remoteConfigurationUrl = value;
            }
        }


        private string localConfigurationFolder;
        public string LocalConfigurationFolder
        {
            get
            {
                return localConfigurationFolder;
            }
            set
            {
                localConfigurationFolder = value;
            }
        }

        void EnsureLocalApplicationFolder()
        {
            if (!string.IsNullOrEmpty(localConfigurationFolder) && !string.IsNullOrEmpty(applicationName))
            {
                if (!System.IO.Directory.Exists(LocalApplicationFolder))
                    System.IO.Directory.CreateDirectory(LocalApplicationFolder);
            }
        }



        public string LocalApplicationFolder
        {
            get
            {
                return ConfigUtility.Combine(localConfigurationFolder, applicationName);
            }
        }


        private bool backupConfig;
        public bool BackupConfig
        {
            get { return backupConfig; }
            set
            {
                backupConfig = value;
            }
        }

        private int maxBackupFiles;
        public int MaxBackupFiles
        {
            get { return maxBackupFiles; }
            set
            {
                maxBackupFiles = value;
            }
        }

        private bool checkRemoteConfig;
        public bool CheckRemoteConfig
        {
            get { return checkRemoteConfig; }
            set
            {
                checkRemoteConfig = value;
            }
        }


        #region IXmlSerializable 成员

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }



        public void ReadXml(System.Xml.XmlReader reader)
        {
            applicationName = ConfigUtility.GetStringValue(reader, "applicationName", DefaultConfig.ApplicationName);
            timeout = ConfigUtility.GetIntValue(reader, "timeout", DefaultConfig.Timeout);
            readwriteTimeout = ConfigUtility.GetIntValue(reader, "readwriteTimeout", DefaultConfig.ReadWriteTimeout);
            timeInterval = ConfigUtility.GetIntValue(reader, "timeInterval", DefaultConfig.TimerInterval);
            remoteConfigurationUrl = ConfigUtility.GetStringValue(reader, "remoteConfigurationUrl", DefaultConfig.RemoteConfigurationUrl);
            localConfigurationFolder = ConfigUtility.GetStringValue(reader, "localConfigurationFolder", DefaultConfig.LocalConfigurationFolder);
            backupConfig = ConfigUtility.GetBoolValue(reader, "backupConfig", DefaultConfig.BackupConfig);
            maxBackupFiles = ConfigUtility.GetIntValue(reader, "maxBackupFiles", DefaultConfig.MaxBackupFiles);
            checkRemoteConfig = ConfigUtility.GetBoolValue(reader, "checkRemoteConfig", DefaultConfig.CheckRemoteConfig);

            EnsureLocalApplicationFolder();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("applicationName", applicationName);
            writer.WriteAttributeString("timeout", timeout.ToString());
            writer.WriteAttributeString("readwriteTimeout", readwriteTimeout.ToString());
            writer.WriteAttributeString("timeInterval", timeInterval.ToString());
            writer.WriteAttributeString("remoteConfigurationUrl", remoteConfigurationUrl);
            writer.WriteAttributeString("localConfigurationFolder", localConfigurationFolder);
            writer.WriteAttributeString("backupConfig", backupConfig.ToString());
            writer.WriteAttributeString("maxBackupFiles", maxBackupFiles.ToString());
            writer.WriteAttributeString("checkRemoteConfig", checkRemoteConfig.ToString());
        }

        #endregion


        private static RemoteConfigurationManagerConfiguration defaultConfig;
        public static RemoteConfigurationManagerConfiguration DefaultConfig
        {
            get
            {
                if (defaultConfig == null)
                {
                    var config = new RemoteConfigurationManagerConfiguration
                    {
                        ApplicationName = ConfigUtility.ApplicationName,
                        Timeout = 5000,
                        ReadWriteTimeout = 5000,
                        TimerInterval = 10000
                    };

                    if (ConfigUtility.IsProd)
                    {
                        config.RemoteConfigurationUrl = "http://HKAPPPWV710/PwC.Configuration/ConfigVersionHandler.ashx";
                    }
                    else if (ConfigUtility.IsLabs)
                    {
                        config.RemoteConfigurationUrl = "http://Localhost/PwC.Configuration/ConfigVersionHandler.ashx";
                    }
                    else if (ConfigUtility.IsUAT)
                    {
                        config.RemoteConfigurationUrl = "http://10.150.64.11/PwC.Configuration/ConfigVersionHandler.ashx";
                    }
                    else
                    {
                        config.RemoteConfigurationUrl = "http://CNPEKAPPDWV031/PwC.Configuration/ConfigVersionHandler.ashx";
                    }
                    config.LocalConfigurationFolder = ConfigUtility.RootConfigFolder;
                    config.BackupConfig = false;
                    config.MaxBackupFiles = 10;
                    config.CheckRemoteConfig = true;

                    defaultConfig = config;
                }
                return defaultConfig;
            }
        }
    }
}
