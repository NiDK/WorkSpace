using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
     [DataContract]
    public class EmailTemplate
    {
        [DataMember]
        public int TemplateId { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string TemplateCode { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string MailFrom { get; set; }
        [DataMember]
        public string MailReplyTo { get; set; }
        [DataMember]
        public string MailCc { get; set; }
        [DataMember]
        public string MailBcc { get; set; }
        [DataMember]
        public string MailOrganisation { get; set; }
        [DataMember]
        public string MailSubject { get; set; }
        [DataMember]
        public string MailContent { get; set; }
        [DataMember]
        public string MailSubmitBy { get; set; }
        [DataMember]
        public bool IsImmediate { get; set; }
        [DataMember]
        public System.DateTime CreateDate { get; set; }
        [DataMember]
        public System.DateTime ModifyDate { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
