using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class MailAttachment
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public byte[] Content { get; set; }
        [DataMember]
        public string MimeType { get; set; }
        [DataMember]
        public bool LinkedResourceFlag { get; set; }
    }
}
