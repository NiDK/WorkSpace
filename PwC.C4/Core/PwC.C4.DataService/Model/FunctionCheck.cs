using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class FunctionCheck
    {
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Controller { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public List<string> RoleName { get; set; }
    }
}
