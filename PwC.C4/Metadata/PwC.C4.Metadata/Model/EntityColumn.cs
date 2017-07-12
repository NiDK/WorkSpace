using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Metadata.Model
{
    public class EntityColumn:DynamicMetadata
    {
        public string Name
        {
            get { return SafeGet<string>("Name"); }
            set { SafeSet("Name", value); }
        }
        public string Label
        {
            get { return SafeGet<string>("Label"); }
            set { SafeSet("Label", value); }
        }
        public string SortName
        {
            get { return SafeGet<string>("SortName"); }
            set { SafeSet("SortName", value); }
        }
        public string ShortName
        {
            get { return SafeGet<string>("ShortName"); }
            set { SafeSet("ShortName", value); }
        }
        public string Width
        {
            get { return SafeGet<string>("Width"); }
            set { SafeSet("Width", value); }
        }
        public int Order
        {
            get { return SafeGet<int>("Order"); }
            set { SafeSet("Order", value); }
        }
        public bool Sortable
        {
            get { return SafeGet<bool>("Sortable"); }
            set { SafeSet("Sortable", value); }
        }
        public bool Searchable
        {
            get { return SafeGet<bool>("Searchable"); }
            set { SafeSet("Searchable", value); }
        }
        public bool Visable
        {
            get { return SafeGet<bool>("Visable"); }
            set { SafeSet("Visable", value); }
        }

        public string Type
        {
            get { return SafeGet<string>("Type"); }
            set { SafeSet("Type", value); }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as EntityColumn;
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
