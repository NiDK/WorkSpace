using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using PwC.C4.Configuration.Logging;

namespace PwC.C4.Configuration.PerformanceCounter
{
    class PerformanceCounterCollection
    {
        Dictionary<string, System.Diagnostics.PerformanceCounter> counters = new Dictionary<string, System.Diagnostics.PerformanceCounter>();
        public void AddCounter(System.Diagnostics.PerformanceCounter counter)
        {
            counters.Add(counter.CounterName, counter);
        }

        public System.Diagnostics.PerformanceCounter GetCounter(string name)
        {
            System.Diagnostics.PerformanceCounter counter;
            counters.TryGetValue(name, out  counter);
            return counter;
        }
    }

    public class PerfCounterCategoryConfig
    {
        static bool CounterExists(string counterName, string category)
        {
            try
            {
                return PerformanceCounterCategory.CounterExists(counterName, category);
            }
            catch
            {
                return false;
            }
        }


        static void RemoveCategory(string category)
        {
            try
            {
                if (PerformanceCounterCategory.Exists(category))
                    PerformanceCounterCategory.Delete(category);
            }
            catch //no permission
            {
            }
        }

        internal void EnsureCounters()
        {
            if (PerformanceCounterCategory.Exists(Category))
            {
                bool same = true;

                try
                {
                    PerformanceCounterCategory cat = new PerformanceCounterCategory(Category);
                    same = (cat.CategoryType == CategoryType);
                }
                catch //maybe delete by others as well
                {
                }

                if (same)
                {
                    if (counterConfigs != null)
                    {
                        foreach (PerfCounterConfig config in counterConfigs)
                        {
                            if (!CounterExists(config.Name, Category))
                            {
                                same = false;
                                break;
                            }
                        }
                    }
                }
                if (!same)
                {
                    RemoveCategory(Category);
                    InstallCounters();
                }
            }
            else
                InstallCounters();

            if (counterConfigs != null)
            {
                defaultInstanceCounters = CreatePerfCounters(null);
            }
        }

        private PerformanceCounterCollection CreatePerfCounters(string instance)
        {
            PerformanceCounterCollection counters = new PerformanceCounterCollection();
            foreach (PerfCounterConfig config in counterConfigs)
            {
                var perfCounter = CounterFromConfig(instance, config);
                if (perfCounter != null)
                    counters.AddCounter(perfCounter);
            }
            return counters;
        }

        void InstallCounters()
        {
            if (counterConfigs != null)
            {
                CounterCreationDataCollection col = new CounterCreationDataCollection();

                foreach (PerfCounterConfig config in counterConfigs)
                {
                    CounterCreationData data = new CounterCreationData(config.Name, config.Help, config.CounterType);
                    col.Add(data);
                }
                try
                {
                    PerformanceCounterCategory.Create(Category, Help, CategoryType, col);
                }
                catch (Exception ex)
                {
                    //we'll log this latter, but we can't use handle exception now
                    //  because we must be sure that it would be recursively
                    LoggingWrapper.Write(string.Format("Failed to create category '{0}': {1}\r\n{2}", Category,
                        ex.Message, ex.StackTrace));

                }

            }
        }



        [XmlAttribute("name")]
        public string Category;
        [XmlAttribute("help")]
        public string Help;
        [XmlAttribute("multipleInstance")]
        public bool MultipleInstance;

        private PerformanceCounterCategoryType CategoryType
        {
            get
            {
                return (MultipleInstance ? PerformanceCounterCategoryType.MultiInstance : PerformanceCounterCategoryType.SingleInstance);
            }
        }

        private PerfCounterConfig[] counterConfigs;
        [XmlArray("Counters")]
        [XmlArrayItem("Counter")]
        public PerfCounterConfig[] CounterConfigs
        {
            get
            {
                return counterConfigs;
            }
            set
            {
                counterConfigs = value;
            }
        }

        public System.Diagnostics.PerformanceCounter GetCounter(string name)
        {
            return defaultInstanceCounters.GetCounter(name);
        }

        /// <summary>
        /// default counters
        /// </summary>
        private PerformanceCounterCollection defaultInstanceCounters = new PerformanceCounterCollection();

        /// <summary>
        /// instance counters
        /// </summary>
        private Dictionary<string, PerformanceCounterCollection> dtInstanceCounters = new Dictionary<string, PerformanceCounterCollection>();


        public System.Diagnostics.PerformanceCounter GetCounter(string counterName, string instance)
        {
            PerformanceCounterCollection perfCat = defaultInstanceCounters;
            if (instance != null)
            {
                lock (dtInstanceCounters)
                {
                    if (!dtInstanceCounters.TryGetValue(instance, out perfCat))
                    {
                        perfCat = CreatePerfCounters(instance);
                        dtInstanceCounters.Add(instance, perfCat);
                        //Dictionary<string, PerformanceCounterCollection> cats = new Dictionary<string, PerformanceCounterCollection>(dtInstanceCounters);
                        //cats[instance] = perfCat;
                        //dtInstanceCounters = cats;
                    }
                }
            }
            return perfCat.GetCounter(counterName);
        }


        System.Diagnostics.PerformanceCounter CounterFromConfig(string instance, PerfCounterConfig config)
        {
            try
            {
                System.Diagnostics.PerformanceCounter counter;
                if (MultipleInstance)
                {
                    if (instance == null)
                        counter = new System.Diagnostics.PerformanceCounter(Category, config.Name, ConfigUtility.ApplicationName, false);
                    else
                        counter = new System.Diagnostics.PerformanceCounter(Category, config.Name, instance, false);
                }
                else
                    counter = new System.Diagnostics.PerformanceCounter(Category, config.Name, false);
                counter.RawValue = config.RawValue;
                return counter;
            }
            catch (Exception ex)
            {
                //we'll log this latter, but we can't use handle exception now
                //  because we must be sure that it would be recursively
                LoggingWrapper.Write(string.Format("Failed to create counter '{0}': {1}\r\n{2}", config.Name,
                    ex.Message, ex.StackTrace));
                return null;
            }
        }
    }

    [XmlRoot("PerfCounterCategories")]
    public class PerfCounterCategoryConfigCollection
    {

        private PerfCounterCategoryConfig[] categories;
        [XmlElement("Category")]
        public PerfCounterCategoryConfig[] Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                if (value != null)
                {
                    foreach (PerfCounterCategoryConfig cat in value)
                    {
                        dt.Add(cat.Category, cat);
                    }
                }
            }
        }

        Dictionary<string, PerfCounterCategoryConfig> dt = new Dictionary<string, PerfCounterCategoryConfig>();

        static PerfCounterCategoryConfigCollection()
        {
            instance = ConfigManager.GetSection<PerfCounterCategoryConfigCollection>();
            instance.EnsureCategories();
        }

        private static PerfCounterCategoryConfigCollection instance;

        public static PerfCounterCategoryConfigCollection Instance
        {
            get
            {
                return instance;
            }
            set
            {
                value.EnsureCategories();
                instance = value;
            }
        }


        private void EnsureCategories()
        {
            if (categories != null)
            {
                foreach (PerfCounterCategoryConfig cat in categories)
                {
                    cat.EnsureCounters();
                }
            }
        }

        public System.Diagnostics.PerformanceCounter GetCounter(string name)
        {
            if (categories != null && categories.Length > 0)
                return categories[0].GetCounter(name);
            else
                return null;
        }
        public System.Diagnostics.PerformanceCounter GetCounter(string category, string name)
        {
            PerfCounterCategoryConfig cat;
            if (dt.TryGetValue(category, out cat))
                return cat.GetCounter(name);
            else
                return null;
        }

        public System.Diagnostics.PerformanceCounter GetCounter(string category, string name, string instance)
        {
            PerfCounterCategoryConfig cat;
            if (dt.TryGetValue(category, out cat))
                return cat.GetCounter(name, instance);
            else
                return null;
        }

    }
}
