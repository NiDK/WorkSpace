using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Rush.WcfService.Models
{
    [DataContract]
    public class FormMain
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string FormName { get; set; }
        [DataMember]
        public string ConnName { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public string AliasName { get; set; }
        /// <summary>
        /// 在数据库中使用的是@ConnectionString，别问我为什么，我懒 by ych
        /// </summary>
        [DataMember]
        public string LinkTrackingConnName { get; set; }
        [DataMember]
        public Guid Layout { get; set; }
        [DataMember]
        public string LayoutName { get; set; }
        [DataMember]
        public string Props { get; set; }
        public List<string> Properties => this.Props.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
        [DataMember]
        public string Structure { get; set; }
        public List<FormControl> FormStructure => JsonHelper.Deserialize<List<FormControl>>(this.Structure);
        [DataMember]
        public string JavaScript { get; set; }
        [DataMember]
        public string Styles { get; set; }
        [DataMember]
        public string LayoutCode { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        public DateTime ModifyTime { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
