using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.UnitTest
{
    [MetaObject(Name = "App_Demo")]
    public class TestModel : DynamicMetadata
    {
        [MetaColumn(IsPk = true, Name = "RecordId")]
        public Guid RecordId
        {
            get { return SafeGet<Guid>("RecordId"); }
            set { SafeSet("RecordId", value); }
        }

        public string _id
        {
            get { return SafeGet<string>("_id"); }
            set { SafeSet("_id", value); }
        }
    }
}
