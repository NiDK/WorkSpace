using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Model.Const
{
    public static class SearchItemMethod
    {
        public static readonly string And = "AND";
        public static readonly string Or = "OR";
        public static readonly string AndLeftBracket = "ALB";
        public static readonly string OrLeftBracket = "OLB";
        public static readonly string RightBracket = "RB";
        public static readonly string Empty = "";
    }
    public static class SearchItemOperator
    {
        public static readonly string Equal = "Equal";
        public static readonly string Intequal = "Intequal";
        public static readonly string Like = "Like";
        public static readonly string In = "In";
        public static readonly string Intin = "Intin";
        public static readonly string Contains = "Contains";
        public static readonly string NotEqual = "NotEqual";
        public static readonly string NotIntequal = "NotIntequal";
        public static readonly string NotLike = "NotLike";
        public static readonly string NotIn = "NotIn";
        public static readonly string NotIntin = "NotIntin";
        public static readonly string NotContains = "NotContains";
    }
}
