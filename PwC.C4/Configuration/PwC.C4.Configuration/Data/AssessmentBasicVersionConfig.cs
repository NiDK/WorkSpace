using System;
using System.Xml.Serialization;
using PwC.C4.Infrastructure.Config.Logging;

namespace PwC.C4.Infrastructure.Config.Data
{
    public class AssessmentBasicVersionEntry
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("startTime")]
        public string StartTime;

        [XmlAttribute("endTime")]
        public string EndTime;

        [XmlAttribute("version")]
        public string Version;
    }

    [XmlRoot(AssessmentBasicVersionCollection.SectionName)]
    public class AssessmentBasicVersionCollection
    {
        private const string SectionName = "AssessmentBasicVersionConfig";
        private static AssessmentBasicVersionCollection instance;

        static AssessmentBasicVersionCollection()
        {
            instance = RemoteConfigurationManager.Instance.GetSection<AssessmentBasicVersionCollection>(SectionName);
        }

        static EventHandler _handler;

        public static void RegisterConfigChangedNotification(EventHandler handler)
        {
            _handler += handler;
        }

        public static AssessmentBasicVersionCollection Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                if (_handler != null)
                    _handler(value, EventArgs.Empty);
            }
        }     

        [XmlElement("add")]
        public AssessmentBasicVersionEntry[] Entries;    
      
        public string GetAssessmentDatabaseString(DateTime time)
        {
            string assessmentString = "AssessmentBasicV1";
            try
            {
                if(Entries != null && Entries.Length >0)
                {                
                    foreach(AssessmentBasicVersionEntry entry in Entries)
                    {
                        DateTime startDT = DateTime.Parse(entry.StartTime);
                        DateTime endDT = DateTime.Parse(entry.EndTime);

                        if (time > startDT && time <= endDT)
                        {
                            assessmentString = entry.Name;
                            break;
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                LoggingWrapper.HandleException(ex, "PwC.C4.Configuration.AssessmentBasicVersion");
            }

            return assessmentString;
        }
    }
}
