using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Sketch.Models
{
    /// <summary>
    /// 
    /// </summary>
    [MetaObject(Name = "Form_Sketch")]
    public class SketchModel : DynamicMetadata
    {
        [MetaColumn(IsPk = true, Name = "RecordId")]
        public string RecordId
        {
            get { return SafeGet<string>("RecordId"); }
            set { SafeSet("RecordId", value); }
        }

        public string Creator
        {
            get { return SafeGet<string>("CreateBy"); }
            set { SafeSet("CreateBy", value); }
        }

        public bool IsDeleted
        {
            get { return SafeGet<bool>("IsDeleted"); }
            set { SafeSet("IsDeleted", value); }
        }

}

    
}
