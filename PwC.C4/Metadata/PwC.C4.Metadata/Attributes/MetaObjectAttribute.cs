using System;

namespace PwC.C4.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MetaObjectAttribute : Attribute
    {
        public string Name { get; set; }

    }
}