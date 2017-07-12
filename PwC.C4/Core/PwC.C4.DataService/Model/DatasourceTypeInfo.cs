using System;
using System.Runtime.Serialization;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class DataSourceTypeInfo
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Desc { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
    }
}