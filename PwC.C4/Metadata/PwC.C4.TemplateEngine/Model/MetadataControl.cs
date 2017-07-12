using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;
using PwC.C4.DataService.Model;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.TemplateEngine.Model
{
    [DataContract]
    public class MetadataControl
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string DataFormat { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public ControlType BaseControlType { get; set; }
        [DataMember]
        public DataSourceType DataSourceType { get; set; }
        [DataMember]
        public string DataSource { get; set; }
        public MvcHtmlString ControlHtml { get; set; }
        public string Html { get; set; }
        [DataMember]
        public bool IsRequire { get; set; }
        [DataMember]
        public string InputRegular { get; set; }
        [DataMember]
        public string RequireInfo { get; set; }
        [DataMember]
        public string InvalidMessage { get; set; }
        [DataMember]
        public List<DataSourceObject> DataSourceObjects { get; set; }
        [DataMember]
        public int Size { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as MetadataControl;
            if ((System.Object)p == null)
            {
                return false;
            }
            return p.Name+p.Group == this.Name + this.Group;
        }

        public override int GetHashCode()
        {
            var info = this.Name + this.Group;
            return info.GetHashCode();
        }
    }
}
