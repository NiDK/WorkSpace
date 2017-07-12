using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.Models
{
    [DataContract]
    public class FormItem
    {
        [DataMember]
        public Dictionary<string, object> Data { get; set; }

        [DataMember]
        public FormControl Control { get; set; }
    }
}
