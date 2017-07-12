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
    public class HtmlCategory
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public Guid ParentId { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// 0.默认，仅作展示或无逻辑父级。1.不可展开，弹出Url。2，可展开，执行func
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Func { get; set; }
        [DataMember]
        public string Parameters { get; set; }
        [DataMember]
        public Dictionary<string,string> ParameterDic { get; set; }
        [DataMember]
        public string Icon { get; set; }
        /// <summary>
        /// 默认为True即闭合状态
        /// </summary>
        [DataMember]
        public bool IsCollapse { get; set; }
        /// <summary>
        /// 0,正常，1，其他，该项根据业务来确定
        /// </summary>
        [DataMember]
        public int Status { get; set; }
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
        /// <summary>
        /// 目录等级，从0开始
        /// </summary>
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public List<HtmlCategory> SubCategories { get; set; }

    }
}
