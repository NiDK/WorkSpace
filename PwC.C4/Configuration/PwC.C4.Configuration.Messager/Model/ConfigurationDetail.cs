using System;
using System.Xml;

namespace PwC.C4.Configuration.Messager.Model
{
    public class ConfigurationDetail
    {
        public string ConfigName { get; set; }
        public Guid Id { get; set; }

        public Guid ConfigId { get; set; }

        public string AppCode { get; set; }

        public Int16 Major { get; set; }

        public int? Minor { get; set; }

        public string Xml { get; set; }

        public XmlDocument Content { get; set; } 

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateTimeStr => this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");

        public bool IsDeleted { get; set; }

        public int Status { get; set; }
    }
}