using System.Collections.Generic;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.DataService.Model;

namespace PwC.C4.Metadata.Model
{
    public class EntitySearchColumn
    {
        public string Name { get; set; }
        public string Label { get; set; }
        /// <summary>
        /// TextBox = 1,CheckBox = 3,Select = 4,Textarea = 5,PeoplePick =11 same as TemplateEngines
        /// </summary>
        public BaseControlType Type { get; set; }
        public List<DataSourceObject> DataSource { get; set; }
        public string DataSourceName { get; set; }
        public string[] Method { get; set; }
        public DataSourceType DataSourceType { get; set; }
        public int Order { get; set; }
        public bool IsSelected { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as EntitySearchColumn;
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
