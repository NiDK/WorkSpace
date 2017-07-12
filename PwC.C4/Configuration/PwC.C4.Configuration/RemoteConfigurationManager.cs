using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using PwC.C4.Configuration.Logging;

namespace PwC.C4.Configuration
{
    [XmlRoot(Namespace = "PwC.C4.Configuration")]
    public class RemoteConfigurationManager : BaseConfigurationManager
    {
        private RemoteConfigurationManagerConfiguration config;

        static RemoteConfigurationManager instance = new RemoteConfigurationManager();

        public static RemoteConfigurationManager Instance
        {
            get
            {
                return instance;
            }
        }

        const string RemoteConfigFileAppSettingKey = "RemoteConfigFile";
        const string RemoteConfigurationManagerConfigFileName = "RemoteConfigurationManager.config";

        private static readonly LogWrapper logger = new LogWrapper();

        private static string GetRemoteConfigFile()
        {
            //TODO: we MUST create default configuration here in case that "RemoteConfigurationManagerConfiguration.config" does not exist
            string remoteFile = System.Configuration.ConfigurationManager.AppSettings[RemoteConfigFileAppSettingKey];
            if (remoteFile == null)
                remoteFile = LocalConfigurationManager.MapConfigPath(RemoteConfigurationManagerConfigFileName);
            else
                remoteFile = LocalConfigurationManager.MapConfigPath(remoteFile);
            if (!File.Exists(remoteFile))
            {
                Log("Config file '" + remoteFile + "' doesn't exists, use/create new configuration files in '" + ConfigUtility.DefaultApplicationConfigFolder + "'");
                remoteFile = ConfigUtility.Combine(ConfigUtility.DefaultApplicationConfigFolder, RemoteConfigurationManagerConfigFileName);
                if (!File.Exists(remoteFile))
                {
                    Directory.CreateDirectory(ConfigUtility.DefaultApplicationConfigFolder);
                    using (XmlTextWriter writer = new XmlTextWriter(remoteFile, Encoding.UTF8))
                    {
                        writer.WriteStartElement(RemoteConfigurationManagerConfiguration.TagName);
                        RemoteConfigurationManagerConfiguration.DefaultConfig.WriteXml(writer);
                        writer.WriteEndElement();
                        writer.Close();
                    }
                }
            }
            return remoteFile;

        }

        protected RemoteConfigurationManager()
            : base()
        {
            var configFile = GetRemoteConfigFile();
            try
            {
                config =
                    XmlSerializerSectionHandler.CreateAndSetupWatcher<RemoteConfigurationManagerConfiguration>(configFile);

                if (config.CheckRemoteConfig)
                {
                    System.Timers.Timer timer = new System.Timers.Timer(config.TimerInterval);
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerCallback);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex,
                    "Unabled to load RemoteConfigurationManager configuration file, Please set '" + RemoteConfigFileAppSettingKey + "' in appSettings",
                    "RemoteConfigurationManager");
                throw ex;
            }
        }


        static string GetSectionName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        string GetFileName(string sectionName)
        {
            return sectionName + ".config";
        }


        string GetPath(string sectionName)
        {
            return ConfigUtility.Combine(config.LocalApplicationFolder, GetFileName(sectionName));
        }

        static void RemoveOldBackupFiles(string sectionName)
        {
            string[] files = Directory.GetFiles(instance.config.LocalApplicationFolder, sectionName + ".*.config");
            if (files.Length > instance.config.MaxBackupFiles)
            {
                List<string> lst = new List<string>(files);
                lst.Sort();

                while (lst.Count > instance.config.MaxBackupFiles)
                {
                    File.Delete(lst[0]);
                    lst.RemoveAt(0);
                }
            }
        }

        static string GetTempFileName(string filePath)
        {
            return filePath + "." + Guid.NewGuid().ToString("N");
        }

        static string GetBackupFileName(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf('.') + 1) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".config";
        }



        void OnConfigFileChanged(object sender, EventArgs args)
        {
            try
            {
                string filePath = ((FileChangedEventArgs)args).FileName;
                string sectionName = GetSectionName(filePath);

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                int major, minor;
                XmlSerializerSectionHandler.GetConfigVersion(doc.DocumentElement, out major, out minor);

                ConfigEntry entry = GetEntry(sectionName);
                if (entry != null)
                    entry.MinorVersion = minor;
            }
            catch (Exception ex)
            {
                logger.HandleException(ex, "RemoteConfigurationManager");
            }
        }



        object CreateLocalObject(Type type, string path, out int major, out int minor)
        {
            try
            {
                if (File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    object obj = XmlSerializerSectionHandler.CreateAndSetupWatcher(doc.DocumentElement,
                        path, type, OnConfigFileChanged);

                    XmlSerializerSectionHandler.GetConfigVersion(doc.DocumentElement, out major, out minor);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "RemoteConfigurationManager.CreateLocalObject,type=" + type.Name + ",path=" + path, type.Name);
            }
            major = XmlSerializerSectionHandler.GetConfigurationClassMajorVersion(type);
            minor = XmlSerializerSectionHandler.DefaultUninitMinorVersion;
            return null;
        }

        #region Override entry

        protected override object OnCreate(string sectionName, Type type, out int major, out int minor)
        {
            string fileName = GetFileName(sectionName);
            string path = GetPath(sectionName);
            object obj = CreateLocalObject(type, path, out major, out minor);
            if (obj != null)
                return obj;

            //Get Remote Config version
            major = XmlSerializerSectionHandler.GetConfigurationClassMajorVersion(type);
            minor = XmlSerializerSectionHandler.DefaultUninitMinorVersion;
            try
            {
                RemoteConfigSectionParam param = GetServerVersion(sectionName, major);
                if (param != null)
                {
                    //download from remote!
                    if (Download(param.SectionName, param.DownloadUrl, path, CheckDownloadStream))
                        obj = CreateLocalObject(type, path, out major, out minor);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Error when download configuration '" + sectionName + "' from remote server for the firet time", sectionName);
            }

            //if object is null use default object instead
            if (obj == null)
            {
                Log("Cannot get section '" + sectionName + "' with type '" + type.Name + "' from RemoteConfiguration, create empty instance instead");
                obj = Activator.CreateInstance(type);
                XmlSerializerSectionHandler.SetupWatcher(path, obj);
                XmlSerializerSectionHandler.RegisterReloadNotification(path, OnConfigFileChanged);
            }
            return obj;

        }

        #endregion

        RemoteConfigSectionParam GetServerVersion(string name, int majorVersion)
        {
            RemoteConfigSectionCollection lstParams = new RemoteConfigSectionCollection(config.ApplicationName);
            lstParams.AddSection(name, majorVersion, XmlSerializerSectionHandler.DefaultUninitMinorVersion);
            lstParams = GetServerVersions(lstParams);
            if (lstParams.Count == 0)
                return null;
            else
                return lstParams[0];
        }

        RemoteConfigSectionCollection GetServerVersions(RemoteConfigSectionCollection lstInputParams)
        {
            try
            {

                MemoryStream ms = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(RemoteConfigSectionCollection));
                ser.Serialize(ms, lstInputParams);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.RemoteConfigurationUrl);
                req.ContentType = "text/xml";
                req.Method = "POST";
                req.Timeout = config.Timeout;
                req.ReadWriteTimeout = config.ReadWriteTimeout;
                req.ContentLength = ms.Length;
                req.ServicePoint.Expect100Continue = false;
                req.ServicePoint.UseNagleAlgorithm = false;
                req.KeepAlive = false;

                using (Stream stream = req.GetRequestStream())
                {
                    
                    byte[] buf = ms.ToArray();
                    stream.Write(buf, 0, buf.Length);
                    //string str = System.Text.Encoding.Default.GetString(buf);
                    

                    stream.Close();
                }

                RemoteConfigSectionCollection lstOutput;
                using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
                {
                    using (Stream stream = rsp.GetResponseStream())
                    {
                        lstOutput = (RemoteConfigSectionCollection)ser.Deserialize(stream);
                        stream.Close();
                    }
                    rsp.Close();

                }
                return lstOutput;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Unabled to GetServerVersions from '" + config.RemoteConfigurationUrl + "'", "RemoteConfigurationManager");
                return new RemoteConfigSectionCollection();
            }
        }

        static void WriteStreamToFile(Stream stream, string file)
        {
            using (FileStream fout = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                byte[] buf = new byte[4096];
                int length;
                while ((length = stream.Read(buf, 0, buf.Length)) > 0)
                    fout.Write(buf, 0, length);
                fout.Close();
            }
        }


        static bool Download(string resourceName, string url, string targetPath, DownloadChecker checker)
        {
            try
            {

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                req.KeepAlive = false;

                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                string tmpFile = GetTempFileName(targetPath);
                using (Stream rspStream = rsp.GetResponseStream())
                {
                    //using (FileStream fout = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
                    {
                        try
                        {
                            if (checker != null)
                            {
                                //check before download
                                //Response.ContentLength is not reliable if Response use buffered mode
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    byte[] buf = new byte[4096];
                                    int length;
                                    while ((length = rspStream.Read(buf, 0, buf.Length)) > 0)
                                        ms.Write(buf, 0, length);
                                    ms.Position = 0;
                                    checker(resourceName, ms);
                                    ms.Position = 0;
                                    WriteStreamToFile(ms, tmpFile);
                                    ms.Close();
                                }
                            }
                            else
                                WriteStreamToFile(rspStream, tmpFile);

                            //need to check version here at the first place!

                            //this sucks, but this is to reduce the confliction of writing and reading 
                            // because of sucks of Windows, the copyfile is non-transaction. 
                            // we must remove the file before change its name!!!
                            if (File.Exists(targetPath))
                            {
                                if (!Instance.config.BackupConfig)
                                    File.Delete(targetPath);
                                else
                                {
                                    RemoveOldBackupFiles(GetSectionName(targetPath));
                                    File.Move(targetPath, GetBackupFileName(targetPath));
                                }
                            }
                            File.Move(tmpFile, targetPath);


                            //File.Replace(tmpFile, targetPath, null);
                        }
                        finally
                        {
                            File.Delete(tmpFile);
                        }
                    }
                }
                rsp.Close();
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Unabled to download '" + url + "' to '" + targetPath + "'", resourceName);
                return false;
            }
        }

        class DownloadParam
        {
            public string Name;
            public string LocalPath;
            public string Url;
            public DownloadChecker Checker;

            public DownloadParam(string name, string url, string path, DownloadChecker checker)
            {
                this.Name = name;
                this.Url = url;
                this.LocalPath = path;
                this.Checker = checker;
            }
        }

        public void InvalidAllConfigs()
        {
            RemoteConfigSectionCollection lstParams = new RemoteConfigSectionCollection(config.ApplicationName);
            lock (configLocker)
            {
                foreach (ConfigEntry entry in configEntries.Values)
                {
                    if (entry.IsSet)
                        lstParams.AddSection(entry.Name, entry.MajorVersion, entry.MinorVersion);
                }
            }
            InvalidConfigs(lstParams);
        }

        public void InvalidConfig(string name, int majorVersion)
        {
            RemoteConfigSectionCollection lstParams = new RemoteConfigSectionCollection(config.ApplicationName);
            int orgMinorVersion = 0;
            lock (configLocker)
            {
                foreach (ConfigEntry entry in configEntries.Values)
                {
                    if (entry.Name == name)
                    {
                        if (entry.IsSet && entry.MajorVersion == majorVersion)
                        {
                            orgMinorVersion = entry.MinorVersion;
                            lstParams.AddSection(entry.Name, entry.MajorVersion, entry.MinorVersion);
                        }
                        break;
                    }
                }
            }
            InvalidConfigs(lstParams);
        }

        private void InvalidConfigs(RemoteConfigSectionCollection lstParams)
        {
            if (lstParams.Count == 0) return;
            RemoteConfigSectionCollection newParams = GetServerVersions(lstParams);
            if (newParams.Count == 0) return;

            Dictionary<string, RemoteConfigSectionParam> tblOldParam = new Dictionary<string, RemoteConfigSectionParam>(lstParams.Count);
            foreach (RemoteConfigSectionParam item in lstParams)
                tblOldParam.Add(item.SectionName, item);

            foreach (RemoteConfigSectionParam param in newParams.Sections)
            {
                string localPath = GetPath(param.SectionName);
                if (!Download(param.SectionName, param.DownloadUrl, localPath, CheckDownloadStream))
                {
                    throw new System.Configuration.ConfigurationErrorsException("Unabled to download '" + param.DownloadUrl + "' to '" + localPath + "'");
                }
                FileWatcher.Instance.ProcessFileChanged(localPath);

                Log(string.Format("Minor version of config '{0}({1})' has been updated manually from {2} to {3}",
                            param.SectionName, param.MajorVersion,
                            tblOldParam[param.SectionName].MinorVersion, param.MinorVersion));
            }
        }


        void TimerCallback(object sender, System.Timers.ElapsedEventArgs args)
        {
            RemoteConfigSectionCollection lstParams = new RemoteConfigSectionCollection(config.ApplicationName);
            //configLocker.AcquireReaderLock(-1);
            //using (configLocker.AcquireReaderLock())
            lock (configLocker)
            {
                foreach (ConfigEntry entry in configEntries.Values)
                {
                    if (entry.IsSet)
                        lstParams.AddSection(entry.Name, entry.MajorVersion, entry.MinorVersion);
                }
                //configLocker.ReleaseReaderLock();
            }
            if (lstParams.Count == 0) return;

            lstParams = GetServerVersions(lstParams);
            if (lstParams.Count == 0) return;
            foreach (RemoteConfigSectionParam param in lstParams.Sections)
            {
                ThreadPool.QueueUserWorkItem(
                    delegate(object obj)
                    {
                        DownloadParam dp = (DownloadParam)obj;
                        Download(dp.Name, dp.Url, dp.LocalPath, dp.Checker);
                    },
                    new DownloadParam(param.SectionName,
                        param.DownloadUrl,
                        GetPath(param.SectionName),
                        CheckDownloadStream)
                        );
            }
        }

        delegate void DownloadChecker(string sectionName, Stream stream);

        void CheckDownloadStream(string sectionName, Stream stream)
        {
            ConfigEntry entry = this.GetEntry(sectionName);
            if (entry == null)
                throw new System.Configuration.ConfigurationErrorsException("No entry '" + sectionName + "' in RemoteConfigurationManager");

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlSerializerSectionHandler.GetConfigInstance(doc.DocumentElement, entry.EntryType);
        }

    }

}
