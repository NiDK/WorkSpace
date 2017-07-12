using System;

namespace PwC.C4.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MetaColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsPk { get; set; }
    }
}
