using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace StaffHandbook.Models.Enum
{
    public enum Status
    {
        [Description("Draft")]
        Draft=0,
        [Description("Submitted for review")]
        Submittedforreview=1,
        [Description("Under CO's review (1)")]
        UnderCOReview1=2,
        [Description("Under CO's review (2)")]
        UnderCOReview2 = 3,
        [Description("CO Approved")]
        COApproved = 4,
        [Description("Pending further info")]
        PendingFurtherInfo=5,        
        [Description("Completed")]
        Completed=6
    }
}