using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Model
{
    public class SearchItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Method { get; set; }
        public List<SearchItem> SubSearchItems { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as SearchItem;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Name == p.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
