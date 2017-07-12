using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class HtmlSnippet
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public HtmlSnippetType ControlType { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Html { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        public DateTime ModifyTime { get; set; }

    }
}
