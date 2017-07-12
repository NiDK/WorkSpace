using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class DataSourceObject
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Desc { get; set; }
    }
}
