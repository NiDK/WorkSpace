using System;
using System.Runtime.Serialization;

namespace PwC.C4.Rush.WcfService.Models
{
    [DataContract]
    public class FormLayout
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Html { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
