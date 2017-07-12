using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class MailQueueModelStream
    {
        [DataMember]
        public string AppCode { get; set; }

        [DataMember]
        public string MailTo { get; set; }

        [DataMember]
        public string MailCc { get; set; }

        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// 已经解析过Parameter的Email内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string SendFrom { get; set; }

        [DataMember]
        public string ReplyTo { get; set; }

        [DataMember]
        public string MailBcc { get; set; }

        [DataMember]
        public string Organisation { get; set; }

        [DataMember]
        public string SubmitBy { get; set; }

        [DataMember]
        public DateTime SendDate { get; set; }

        [DataMember]
        public string ImmediateFlag { get; set; }

        [DataMember]
        public string MailFrom { get; set; }
    }
}
