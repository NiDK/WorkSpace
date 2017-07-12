using System;
using System.Runtime.Serialization;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class DataSourceDelete
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public DataSource Type { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }

    }
}