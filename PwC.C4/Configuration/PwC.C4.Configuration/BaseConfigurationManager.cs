using System;
using System.Collections.Generic;
using System.Configuration;
using PwC.C4.Configuration.Logging;

namespace PwC.C4.Configuration
{
    public abstract class BaseConfigurationManager
    {
        protected BaseConfigurationManager()
        {
            configEntries = new Dictionary<string, ConfigEntry>();            
            configLocker = new object();
        }

        private static LogWrapper logger = new LogWrapper();


        internal Dictionary<string, ConfigEntry> configEntries;       
        protected object configLocker;
        protected abstract object OnCreate(string sectionName, Type type, out int major, out int minor);


        internal ConfigEntry GetEntry(string sectionName)
        {
            sectionName = sectionName.ToLower();
            ConfigEntry entry;            
            lock (configLocker)
            {
                configEntries.TryGetValue(sectionName, out entry);
            }            
            return entry;
        }

        public T GetSection<T>(string section)
        {
            string sectionName = section.ToLower();
            ConfigEntry entry = GetEntry(sectionName);            
            if (entry == null)
            {                
                lock (configLocker)
                {                    
                    if (!configEntries.TryGetValue(sectionName, out entry))
                    {
                        entry = new ConfigEntry(section, typeof(T), OnCreate);
                        configEntries.Add(sectionName, entry);
                    }                    
                }
            }
            return (T)entry.Value;
        }

        public void ClearCahce(string sectionName)
        {
            sectionName = sectionName.ToLower();
            lock (configLocker)
            {
                configEntries.Remove(sectionName);
            }
        }
        public static void HandleException(Exception ex, string msg, string sectionName)
        {
            //this sucks, but we need to prevent recursive calling for creating ErrorTrackerConfig
            if (sectionName != "ErrorTrackerConfig")
                logger.HandleException(new ConfigurationErrorsException(msg, ex), "ConfigurationManager");
            else
                logger.Info("ConfigurationManager", new ConfigurationErrorsException(msg, ex));
        }

        public static void Log(string msg)
        {
            logger.Info(msg);
        }
    }
}
