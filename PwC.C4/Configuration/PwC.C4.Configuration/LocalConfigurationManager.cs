using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Xml;

namespace PwC.C4.Configuration
{
    public class LocalConfigurationManager : BaseConfigurationManager
    {
        private static LocalConfigurationManager instance;

        public static LocalConfigurationManager Instance
        {
            get { return instance; }
        }

        static string localBaseConfigFolder;
        public static string LocalBaseConfigFolder
        {
            get
            {
                return localBaseConfigFolder;
            }
        }

        static LocalConfigurationManager()
        {
            _systemConfig = GetExeConfig();
            localBaseConfigFolder = Path.GetDirectoryName(_systemConfig.FilePath);
            instance = new LocalConfigurationManager();
            XmlDocument doc = new XmlDocument();
            if (File.Exists(_systemConfig.FilePath))
            {
                doc.Load(_systemConfig.FilePath);
            }
            else
            {
                doc.LoadXml("<configuration/>");

            }
            _systemConfigXml = doc;
        }


        private LocalConfigurationManager()
        {

        }

        public static string Combine(string folder, string file)
        {
            return ConfigUtility.Combine(folder, file);
        }

        public static string MapConfigPath(string fileName)
        {
            return Combine(localBaseConfigFolder, fileName);
        }

        #region local main config
        private static System.Configuration.Configuration _systemConfig;

        private static System.Configuration.Configuration GetExeConfig()
        {
            string AppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if ((AppVirtualPath != null) && (0 < AppVirtualPath.Length))
            {
                /* 
                 * Use the root web.config. This did check where the request was and 
                 * load that web.config. In longhorn we cannot call HttpContext.Request in 
                 * the Application Start. This throws an exception. Changed to always pull
                 * the root web.config
                 * */
                return WebConfigurationManager.OpenWebConfiguration(AppVirtualPath);
            }
            else
            {
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }

        internal static System.Configuration.Configuration LocalMainConfig
        {
            get
            {
                return _systemConfig;
            }
        }

        static XmlDocument _systemConfigXml;

        internal static XmlDocument SystemConfigXml
        {
            get
            {
                return _systemConfigXml;
            }
        }
        #endregion



        static string GetSectionConfigSource(string name)
        {
            //it sucks for the xml namespace issue!
            XmlNodeList nodeList = _systemConfigXml.DocumentElement.GetElementsByTagName(name);
            if (nodeList.Count == 0)
                return string.Empty;
            //XmlElement elm =(XmlElement)_systemConfigXml.DocumentElement.SelectSingleNode(name);
            //if (elm == null) return null;
            XmlElement elm = (XmlElement)nodeList[0];
            return elm.GetAttribute("configSource");
        }

        private static string GetConfigSectionFileName(string name)
        {
            string configSource = GetSectionConfigSource(name);
            string folder = Path.GetDirectoryName(_systemConfig.FilePath);
            if (configSource.Length == 0)
                return "";
            else
                return Combine(folder, configSource);
        }

        protected override object OnCreate(string sectionName, Type type, out int major, out int minor)
        {
            major = XmlSerializerSectionHandler.GetConfigurationClassMajorVersion(type);
            minor = XmlSerializerSectionHandler.DefaultUninitMinorVersion;
            string configPath = GetConfigSectionFileName(sectionName);

            if (configPath.Length == 0) return null;

            object retVal;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                retVal = XmlSerializerSectionHandler.GetConfigInstance(doc.DocumentElement, type);
                XmlSerializerSectionHandler.GetConfigVersion(doc.DocumentElement, out major, out minor);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Error when create local configuration: sectionName=" + sectionName + ",type=" + type.Name + ", create entry config instead", sectionName);
                //if exception here, return default configuration class and then setup watch
                retVal = Activator.CreateInstance(type);
            }

            XmlSerializerSectionHandler.SetupWatcher(configPath, retVal);
            return retVal;
        }

    }
}
