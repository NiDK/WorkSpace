using System.Collections.Generic;

namespace PwC.C4.Ants.Service.Models
{
    public class QueryItem
    {
        public int Size { get; set; }
        public int From { get; set; }
        public List<QuerySortItem> Sorts { get; set; }
        public List<QueryItemField> Fields { get; set; }
    }
}