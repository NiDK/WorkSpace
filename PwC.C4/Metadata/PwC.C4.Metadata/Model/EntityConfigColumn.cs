namespace PwC.C4.Metadata.Model
{
    public class EntityConfigColumn
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }


        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as EntityConfigColumn;
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
