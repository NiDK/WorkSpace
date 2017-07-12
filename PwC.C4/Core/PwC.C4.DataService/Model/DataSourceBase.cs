using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class DataSourceBase
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public int Order { get; set; }
    }
}
