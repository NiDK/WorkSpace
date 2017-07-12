using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PwC.C4.Configuration
{
    public class RemoteConfigSectionParam
    {
        [XmlAttribute("name")]
        public string SectionName;

        [XmlAttribute("majorVerion")]
        public int MajorVersion;

        [XmlAttribute("minorVerion")]
        public int MinorVersion;

        [XmlAttribute("downloadUrl")]
        public string DownloadUrl;
    }

    public class RemoteConfigSectionCollection
    {
        [XmlAttribute("machine")]
        public string Machine;

        [XmlAttribute("application")]
        public string Application;

        [XmlElement("section")]
        public List<RemoteConfigSectionParam> Sections;

        public int Count
        {
            get
            {
                return Sections.Count;
            }
        }

        public RemoteConfigSectionParam this[int index]
        {
            get
            {
                return Sections[index];
            }
        }


        public void AddSection(string sectionName, int major, int minor)
        {
            AddSection(sectionName, major, minor, null);
        }

        public void AddSection(string sectionName, int major, int minor, string url)
        {
            var param = new RemoteConfigSectionParam
            {
                SectionName = sectionName,
                MajorVersion = major,
                MinorVersion = minor,
                DownloadUrl = url
            };
            Sections.Add(param);
        }

        public RemoteConfigSectionCollection()
        {
            Machine = Environment.MachineName;
            Sections = new List<RemoteConfigSectionParam>();
        }

        public RemoteConfigSectionCollection(string appName)
            : this()
        {
            this.Application = appName;
        }

        public IEnumerator<RemoteConfigSectionParam> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

    }
}
