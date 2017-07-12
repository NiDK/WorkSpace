using System;
using System.Configuration;
using System.Xml.Serialization;

namespace PwC.C4.Configuration
{
    [XmlRoot("ErrorTrackerConfig")]
    public class ErrorTrackerConfig
    {
        private static string configPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static ErrorTrackerConfig instance = ConfigManager.GetSection <ErrorTrackerConfig>("ErrorTrackerConfig");


        #region FlushTimerSeconds
        /// <summary>
        /// Time between writes to destination (service or DB)
        /// </summary>
        private short flushTimerSeconds  = 60;

        [XmlElement("FlushTimerSeconds")]
        public short FlushTimerSeconds
        {
            get { return flushTimerSeconds; }
            set { flushTimerSeconds = value; }
        }
        #endregion

        #region FlushTimeSpan
        [XmlIgnore]
        public TimeSpan FlushTimeSpan
        {
            get { return TimeSpan.FromSeconds(flushTimerSeconds); }
        }
        #endregion

        /// <summary>
        /// Max number of occurrences to store in database (can be overridden if user on qualityTeam or priority exception)
        /// </summary>
        #region OccurrenceThreshhold
        private int occurrenceThreshhold = 5;

        [XmlElement("OccurrenceThreshhold")]
        public int OccurrenceThreshhold
        {
            get { return occurrenceThreshhold; }
            set { occurrenceThreshhold = value; }
        }
        #endregion
	
        #region MaxObjectsPerQueue
        /// <summary>
        /// max number of ExceptionOccurrences and VipCounters to keep in memory before forwarding
        /// </summary>
        private short maxObjectsPerQueue = 1000;

        [XmlElement("MaxObjectsPerQueue")]
        public short MaxObjectsPerQueue
        {
            get { return maxObjectsPerQueue; }
            set { maxObjectsPerQueue = value; }
        }
        #endregion

        #region MaxInMemoryKeys
        /// <summary>
        /// Maximum number of primary key lookups to store in memory
        /// </summary>
        private short maxInMemoryKeys = 10000;

        [XmlElement("MaxInMemoryKeys")]
        public short MaxInMemoryKeys
        {
            get { return maxInMemoryKeys; }
            set { maxInMemoryKeys = value; }
        }
        #endregion


        #region RedirectType
        /// <summary>
        /// whether to redirect user (production) or allow refresh to cause more exceptions
        /// options are none, transfer, redirect
        /// </summary>
        private string redirectType = "Redirect";

        [XmlElement("RedirectType")]
        public string RedirectType
        {
            get {
                return redirectType;
            }
            set { 
                redirectType = value; 
            }
        }

        private string errorPage = "~/Error.aspx?ETH={0}&EOH={1}";

        [XmlElement("ErrorPage")]
        public string ErrorPage
        {
            get { return errorPage; }
            set { errorPage = value; }
        }

        private string profileMaintenancePage = "~/Modules/ProfileDisplay/Pages/ErrorPages/ProfileMaintenance.aspx";

        [XmlElement("ProfileMaintenancePage")]
        public string ProfileMaintenancePage
        {
            get { return profileMaintenancePage; }
            set { profileMaintenancePage = value; }
        }


        #endregion

		[Obsolete()]
        public static ErrorTrackerConfig GetInstance(short ftSecs, int occThresh, short maxObj, short maxKeys, string redirectType)
        {
            ErrorTrackerConfig etc = new ErrorTrackerConfig();
            etc.FlushTimerSeconds = ftSecs;
            etc.OccurrenceThreshhold = occThresh;
            etc.MaxObjectsPerQueue = maxObj;
            etc.MaxInMemoryKeys = maxKeys;
            etc.RedirectType = redirectType;
            return etc;
        }

        public static ErrorTrackerConfig GetInstance()
        {
            if (instance == null)
                throw new ConfigurationErrorsException(
                    "Unable to locate or deserialize the 'ErrorTrackerConfig' section.");
            return instance;
        }
    }
}

