using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Model
{
    public class EntityTable
    {
        public string SortIndex { get; set; }
        public string SortDir { get; set; }
        public string SortId { get; set; }
        public string Start { get; set; }
        public string PageSize { get; set; }
    }
}
