
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Runtime.Serialization;


namespace PwC.C4.DataService.Model
{
    [DataContract]
    [Table("Invitation")]
    public partial class Invitation
    {
        [Key]
        [DataMember]
        public Guid Inid { get; set; } 

        [Required]
        [StringLength(1)]
        [DataMember]
        public string SendFlag { get; set; }

        [StringLength(20)]
        [DataMember]
        public string EmailClientType { get; set; }
        [DataMember]
        public string InvitationType { get; set; }
        [DataMember]
        public DateTime? SendDate { get; set; }
        [DataMember]
        public DateTime? InvitationStartTime { get; set; }
        [DataMember]
        public DateTime? InvitationEndTime { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        [StringLength(100)]
        public string Location { get; set; }
        [DataMember]
        public int? InvitationtimeZone { get; set; }
        [DataMember]
        [Required]
        [StringLength(60)]
        public string CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        [Required]
        [StringLength(50)]
        public string AppCode { get; set; }
    }



}