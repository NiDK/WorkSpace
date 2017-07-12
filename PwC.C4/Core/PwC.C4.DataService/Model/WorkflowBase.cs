using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public class WorkflowBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public string WorkflowCode { get; set; }
        [DataMember]
        public string ActionCode { get; set; }
        [DataMember]
        public int FormId { get; set; }
        [DataMember]
        public string RecordId { get; set; }
        [DataMember]
        public int InstanceId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserRole { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
    }
}
