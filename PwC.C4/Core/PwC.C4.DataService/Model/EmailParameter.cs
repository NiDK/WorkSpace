using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class EmailParameter
    {
        [DataMember]
        public int ParameterId { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string ParameterCode { get; set; }
        [DataMember]
        public string ParameterName { get; set; }
        [DataMember]
        public string Assembly { get; set; }
        [DataMember]
        public ParameterType ParameterType { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public System.DateTime CreateDate { get; set; }
        [DataMember]
        public System.DateTime ModifyDate { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
    }
}
