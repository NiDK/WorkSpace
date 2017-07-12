using System;
using System.Runtime.Serialization;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class DataSourceDetail
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public Guid DataSourceTypeId { get; set; }
        [DataMember]
        public string DataSourceTypeName { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public DateTime ModifyTime { get; set; }

    }
}