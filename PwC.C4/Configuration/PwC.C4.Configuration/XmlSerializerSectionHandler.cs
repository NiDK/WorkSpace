using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using PwC.C4.Configuration.Logging;
using Wintellect.Threading.ResourceLocks;

namespace PwC.C4.Configuration
{
    class ConfigInstances
    {
        private SafeReaderWriterLock configLock = new SafeReaderWriterLock();        
        private Dictionary<string, object> configInstances = new Dictionary<string, object>();

        public object this[string path]
        {
            get
            {                
                if (path == null)
                    return null;
                else
                    path = path.ToLower();
                object objRet;
                
                using (configLock.AcquireReaderLock())
                {
                    configInstances.TryGetValue(path, out objRet);                    
                }
                return objRet;
            }
        }

        public bool ContainsKey(string path)
        {            
            if (path == null)
                return false;
            else
                path = path.ToLower();

            bool contains;            
            using (configLock.AcquireReaderLock())
            {
                contains = configInstances.ContainsKey(path);                
            }
            return contains;
        }

        public bool Add(string path, object obj)
        {
            if (path == null)
                return false;
            else
                path = path.ToLower();

            bool added;           
            using (configLock.AcquireWriterLock())
            {
                if (configInstances.ContainsKey(path))
                    added = false;
                else
                {
                    configInstances.Add(path, obj);
                    added = true;
                }                
            }
            return added;
            
        }
    }

    /// <summary>
    /// ConfigutationSectionHandler that uses xml serialization to map config information to class defined in the type attribute of the config section
    /// </summary>
    public class XmlSerializerSectionHandler : IConfigurationSectionHandler
    {
        private const int CHANGE_CONFIG_DELAY = 5000;

        //private static readonly Dictionary<string, object> configInstances = new Dictionary<string, object>();
        private static readonly ConfigInstances configInstances = new ConfigInstances();
        private static readonly Dictionary<Type, List<EventHandler>> reloadTypeDelegates = new Dictionary<Type, List<EventHandler>>();
        private static readonly Dictionary<string, List<EventHandler>> reloadFileDelegates = new Dictionary<string, List<EventHandler>>();
        private static readonly OneResourceLock reloadDelegatesResourceLock = new OneResourceLock();

        private static readonly LogWrapper logger = new LogWrapper();

        public object Create(object parent, object configContext, XmlNode section)
        {
            object retVal = GetConfigInstance(section);

            System.Configuration.Configuration config = LocalConfigurationManager.LocalMainConfig;

            //SectionInformation info = config.GetSection(section.Name).SectionInformation;
            ConfigurationSection configSection = config.GetSection(section.Name);
            if (configSection.SectionInformation.RestartOnExternalChanges == false)
                SetupWatcher(config, configSection, retVal);

            return retVal;
        }

        #region CreateAndSetupWatcher

        public static T CreateAndSetupWatcher<T>(string path)
        {
            return CreateAndSetupWatcher<T>(path, null);
        }

        public static T CreateAndSetupWatcher<T>(string path, EventHandler OnConfigFileChangedByFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return CreateAndSetupWatcher<T>(doc.DocumentElement, path, OnConfigFileChangedByFile);
        }

        public static object CreateAndSetupWatcher(XmlNode section,
            string path, Type type,
            EventHandler OnConfigFileChangedByFile
            )
        {
            object obj = XmlSerializerSectionHandler.GetConfigInstance(section, type);
            SetupWatcher(path, obj);

            if (OnConfigFileChangedByFile != null)
                RegisterReloadNotification(path, OnConfigFileChangedByFile);

            return obj;
        }

        public static T CreateAndSetupWatcher<T>(XmlNode section, string path, EventHandler OnConfigFileChangedByFile)
        {
            return (T)CreateAndSetupWatcher(section, path, typeof(T), OnConfigFileChangedByFile);
        }
        #endregion

        private static T GetConfigInstance<T>(XmlNode section)
        {
            return (T)GetConfigInstance(section, typeof(T));
        }

        public static int GetConfigurationClassMajorVersion<T>()
        {
            return GetConfigurationClassMajorVersion(typeof(T));
        }

        internal const int DefaultMajorVersion = 1,
            DefaultMinorVersion = 1,
            DefaultUninitMinorVersion = 0;

        public static int GetConfigurationClassMajorVersion(Type type)
        {
            object[] objAttrs = type.GetCustomAttributes(typeof(ConfigurationVersionAttribute), false);
            if (objAttrs == null || objAttrs.Length == 0)
                return DefaultMajorVersion;
            else
                return ((ConfigurationVersionAttribute)objAttrs[0]).MajorVersion;
        }

        /*
        private static void GetVersion(string version,out int majorVersion,out int minorVersion)
        {
            string[] strs = version.Trim().Split('.');
            if (strs.Length < 1)
            {
                majorVersion = 1;
                minorVersion = 1;
            }
            else if (!int.TryParse(strs[0], out majorVersion))
            {
                majorVersion = 1;
                minorVersion = 1;
            }
            else
            {
                if (strs.Length <2 || !int.TryParse(strs[1], out minorVersion))
                    minorVersion = 1;
            }
        }

        internal static string GetConfigVersion(XmlElement section)
        {
            string version = section.GetAttribute("version");
            int majorVersion,minorVersion;
            GetVersion(version, out majorVersion, out minorVersion);
            return majorVersion + "." + minorVersion;
        }
         * */

        internal static void GetConfigVersion(XmlElement section, out int major, out int minor)
        {
            if (!int.TryParse(section.GetAttribute("majorVersion"), out major))
                major = DefaultMajorVersion;
            if (!int.TryParse(section.GetAttribute("minorVersion"), out minor))
                minor = DefaultMinorVersion;
        }


        public static object GetConfigInstance(XmlNode section, Type t)
        {
            //string version = ((XmlElement)section).GetAttribute("version");
            int fileMajorVersion;
            int fileMinorVersion;
            GetConfigVersion((XmlElement)section, out fileMajorVersion, out fileMinorVersion);
            int clsMajorVersion = GetConfigurationClassMajorVersion(t);
            if (fileMajorVersion != clsMajorVersion)
                throw new VersionIncompatibleException("Class Major Version is not equal to the file version", clsMajorVersion, fileMajorVersion);

           
            try
            {
                XmlSerializer ser = new XmlSerializer(t);
                object obj = ser.Deserialize(new XmlNodeReader(section));
                if (obj is IPostSerializer)
                {
                    ((IPostSerializer)obj).PostSerializer();
                }
                return obj;
            }
            catch (Exception ex)
            {
                Exception innerEx = ex;
                if (ex.InnerException != null)
                    innerEx = ex.InnerException;

                throw new ConfigurationErrorsException(string.Format(
                    "XmlSerializerSectionHandler failed to GetConfigInstance from '{0}'.  Error: \r\n{1}", t.FullName, innerEx.ToString()), innerEx);
            }
        }

        public static T GetConfigInstance<T>(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return GetConfigInstance<T>(doc.DocumentElement);
        }


        private static object GetConfigInstance(XmlNode section)
        {
            XPathNavigator nav = section.CreateNavigator();
            string typeName = (string)nav.Evaluate("string(@type)");
            Type t = Type.GetType(typeName);

            if (t == null)
                throw new ConfigurationErrorsException("XmlSerializerSectionHandler failed to create type '" + typeName +
                    "'.  Please ensure this is a valid type string.", section);

            return GetConfigInstance(section, t);
        }


        private static string GetConfigFilePath(System.Configuration.Configuration confFile, ConfigurationSection section)
        {
            string configSource = section.SectionInformation.ConfigSource;
            if (configSource == String.Empty)
            {
                return Path.GetFullPath(confFile.FilePath);
            }
            else
            {

                return ConfigUtility.Combine(Path.GetDirectoryName(confFile.FilePath), configSource);
            }
        }


        private static void SetupWatcher(System.Configuration.Configuration config, ConfigurationSection configSection, object configInstance)
        {
            string filePath = GetConfigFilePath(config, configSection);
            SetupWatcher(filePath, configInstance);
        }

        public static void SetupWatcher(string filePath, object configInstance)
        {

            string fileName = Path.GetFileName(filePath);

            if (configInstances.Add(fileName, configInstance))
            {
                FileWatcher.Instance.AddFile(filePath, DelayedProcessConfigChange);
            }
        }

        static void CloneObject(object srcObject, object targetObject)
        {
            Type type = targetObject.GetType();

            PropertyInfo propInstance = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy);
            if (propInstance != null && propInstance.CanRead && propInstance.CanWrite)
            {
                propInstance.SetValue(null, srcObject, null);
                return;//we don't set original object here, if instance is set
            }

            ICopyable srcCopyable = srcObject as ICopyable;
            if (srcCopyable != null)
            {
                srcCopyable.CopyTo(targetObject);
                return;
            }

            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.CanRead && prop.CanWrite)
                    prop.SetValue(targetObject, prop.GetValue(srcObject, null), null);
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                field.SetValue(targetObject, field.GetValue(srcObject));
            }
        }

        class EventObject
        {
            EventHandler handler;
            object sender;
            EventArgs args;

            public EventObject(EventHandler handler, object sender, EventArgs args)
            {
                this.handler = handler;
                this.sender = sender;
                this.args = args;
            }

            public void Execute()
            {
                handler(sender, args);
            }
        }

        static void CallEventHandler(object obj)
        {
            try
            {
                EventObject evtObj = (EventObject)obj;
                evtObj.Execute();
            }
            catch (Exception ex)
            {
                logger.HandleException(ex, "XmlSerializerSectionHandler");
            }
        }

        private static void DelayedProcessConfigChange(object sender, EventArgs args)
        {
            try
            {
                string filePath = ((string)sender).ToLower();
                string fileName = Path.GetFileName(filePath);

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                //refresh the section in case anyone else uses it
                ConfigurationManager.RefreshSection(doc.DocumentElement.Name);

                object configInstance = configInstances[fileName];
                Type newSettingsType = configInstance.GetType();
                object newSettings = GetConfigInstance(doc.DocumentElement, newSettingsType);
                CloneObject(newSettings, configInstance);

                /*
                Type newSettingsType = configInstance.GetType();
                object newSettings = GetConfigInstance(doc.DocumentElement, newSettingsType);
			
                PropertyInfo[] props =newSettingsType.GetProperties();
                foreach ( PropertyInfo prop in props )
                {
                    if ( prop.CanRead && prop.CanWrite )
                        prop.SetValue(configInstance, prop.GetValue(newSettings, null), null);
                }

                FieldInfo[] fields = newSettingsType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                {
                    field.SetValue(configInstance, field.GetValue(newSettings));
                }
                */

                List<EventHandler> typeHandlers = new List<EventHandler>();

                using (reloadDelegatesResourceLock.WaitToRead())
                {
                    List<EventHandler> delegateMethods;
                    if (reloadTypeDelegates.TryGetValue(newSettingsType, out delegateMethods))
                    {
                        typeHandlers.AddRange(delegateMethods);
                    }

                    if (reloadFileDelegates.TryGetValue(filePath, out delegateMethods))
                    {
                        typeHandlers.AddRange(delegateMethods);
                    }
                }

                FileChangedEventArgs eventArgs = new FileChangedEventArgs(filePath);

                foreach (EventHandler delegateMethod in typeHandlers)
                {
                    //use thread pool to call it pararrelly instead of serially call
                    //delegateMethod(newSettings, eventArgs);
                    ThreadPool.QueueUserWorkItem(CallEventHandler,
                        new EventObject(delegateMethod, newSettings, eventArgs));
                }

                //foreach (EventHandler delegateMethod in fileHandlers)
                //{
                //    delegateMethod(filePath, eventArgs);
                //}
            }
            catch (Exception ex)
            {
                logger.HandleException(ex, "XmlSerializerSectionHandler");
            }
        }

        /// <summary>
        /// Method is used to register for notifications when a particular type has
        /// been reloaded. 
        /// </summary>
        /// <param name="type">Type to monitor for.</param>
        /// <param name="delegateMethod">Delegate method to call.</param>
        public static void RegisterReloadNotification(Type type, EventHandler delegateMethod)
        {
            RegisterReloadNotification(type, delegateMethod, true);
        }


        public static void RegisterReloadNotification(Type type, EventHandler delegateMethod, bool allowMultiple)
        {
            List<EventHandler> delegateMethods;

            using (reloadDelegatesResourceLock.WaitToWrite())
            {
                if (!allowMultiple || !reloadTypeDelegates.TryGetValue(type, out delegateMethods))
                {
                    delegateMethods = new List<EventHandler>();
                    delegateMethods.Add(delegateMethod);
                    reloadTypeDelegates[type] = delegateMethods;
                }
                else
                {
                    delegateMethods.Add(delegateMethod);
                }
            }
        }

        public static void RegisterReloadNotification(string filePath, EventHandler delegateMethod)
        {
            RegisterReloadNotification(filePath, delegateMethod, true);
        }

        public static void RegisterReloadNotification(string filePath, EventHandler delegateMethod, bool allowMultiple)
        {
            List<EventHandler> delegateMethods;
            filePath = filePath.ToLower();

            using (reloadDelegatesResourceLock.WaitToWrite())
            {
                if (!allowMultiple || !reloadFileDelegates.TryGetValue(filePath, out delegateMethods))
                {
                    delegateMethods = new List<EventHandler>();
                    delegateMethods.Add(delegateMethod);
                    reloadFileDelegates[filePath] = delegateMethods;
                }
                else
                {
                    delegateMethods.Add(delegateMethod);
                }
            }
        }
    }

    public class FileChangedEventArgs : EventArgs
    {
        public string FileName;

        public FileChangedEventArgs(string fileName)
        {
            this.FileName = fileName;
        }
    }

}
