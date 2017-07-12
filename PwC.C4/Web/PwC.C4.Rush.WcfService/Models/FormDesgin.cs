using System.Collections.Generic;
using System.Runtime.Serialization;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Rush.WcfService.Models.Enum;

namespace PwC.C4.Rush.WcfService.Models
{
    [DataContract]
    public class FormControl
    {
        [DataMember]
        public ControlType ControlType { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// <div>{data}</div>
        /// </summary>
        [DataMember]
        public string LabelTemplate { get; set; }
        /// <summary>
        /// <div>{data}</div>
        /// </summary>
        [DataMember]
        public string ValueTemplate { get; set; }
        [DataMember]
        public string GridSystem { get; set; }
        [DataMember]
        public List<FormControl> SubFormDesgins { get; set; }
        [DataMember]
        public string Parameters { get; set; }
    }
    [DataContract]
    public class FromRender
    {
        [DataMember]
        public Dictionary<string,object> Data { get; set; }
        [DataMember]
        public List<FormControl> Controls { get; set; }
        [DataMember]
        public string FormName { get; set; }
        [DataMember]
        public string Layout { get; set; }
        [DataMember]
        public string Styles { get; set; }
        [DataMember]
        public string Javascripts { get; set; }
    }

}
