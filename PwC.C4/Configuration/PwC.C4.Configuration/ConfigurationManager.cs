using System;
using System.Xml.Serialization;

namespace PwC.C4.Configuration
{
    public class ConfigManager
    {
        static string GetConfigSectionName<T>()
        {
            Type t = typeof(T);
            object[] attrs = t.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (attrs.Length > 0)
            {
                return ((XmlRootAttribute)attrs[0]).ElementName;
            }
            return t.Name;
        }

        public static T GetSection<T>()
        {
            string name = GetConfigSectionName<T>();
            return GetSection<T>(name);
        }

        public static T GetSection<T>(bool getTemp)
        {
            string name=GetConfigSectionName<T>();;
            if (getTemp)
            {
                RemoteConfigurationManager.Instance.ClearCahce(name);

            }
         
            return GetSection<T>(name);
            
        }

        public static T GetSection<T>(string name)
        {
            bool fromRemote;
            return GetSection<T>(name, out fromRemote);
        }

        public static T GetSection<T>(string name, out bool fromRemote)
        {
            fromRemote = false;
            T obj = LocalConfigurationManager.Instance.GetSection<T>(name);
            if (obj != null)
                return obj;
            else
            {
                BaseConfigurationManager.Log("Unabled to get section '" + name + "' from local configuration file, loading remotely...");
                fromRemote = true;
                return RemoteConfigurationManager.Instance.GetSection<T>(name);
            }
        }

        public static T GetSection<T>(string name, string path)
        {
            bool fromRemote;
            return GetSection<T>(name, path, out fromRemote);
        }

        public static T GetSection<T>(string name, string path, out bool fromRemote)
        {
            if (System.IO.File.Exists(path))
            {
                fromRemote = false;
                return XmlSerializerSectionHandler.CreateAndSetupWatcher<T>(LocalConfigurationManager.MapConfigPath(path));
            }
            else
            {
                fromRemote = true;
                return RemoteConfigurationManager.Instance.GetSection<T>(name);
            }
        }

        public static AppSettingCollection AppSettings
        {
            get
            {
                return AppSettingCollection.Instance;
            }
        }

        public static void RegisterAppSettingsConfigChangedNotification(EventHandler handler)
        {
            AppSettingCollection.RegisterConfigChangedNotification(handler);
        }

        //public static string GetConnectionString(string name)
        //{
        //    return ConnectionStringCollection.Instance[name];
        //}

    }
}
