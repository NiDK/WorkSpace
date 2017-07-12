using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PwC.C4.Metadata.Model
{
    [DataContract]
    public class DataCommonApiModel<T>
    {
        [DataMember]
        public string DataId { get; set; }

        [DataMember]
        public T Model { get; set; }

        [DataMember]
        public string Json { get; set; }

        [DataMember]
        public Dictionary<string, string> Properties { get; set; }
    }

    [DataContract]
    public class EasyDataGridModel<T>
    {
        [DataMember]
        public long total { get; set; }
        [DataMember]
        public List<T> rows { get; set; }
    }
}
