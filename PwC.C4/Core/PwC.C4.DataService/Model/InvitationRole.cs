using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PwC.C4.DataService.Model
{
    [DataContract]
    public partial class InvitationRole
    {
        [DataMember]
        [Key]
        public int RoleId { get; set; }
        [DataMember]
        public Guid Inid { get; set; }
        [DataMember]
        [Required]
        [StringLength(20)]
        public string RoleType { get; set; }
        [DataMember]
        [Required]
        [StringLength(60)]
        public string RoleEmail { get; set; }
        [DataMember]
        [Required]
        [StringLength(1)]
        public string IsRequired { get; set; } 
    }
}
