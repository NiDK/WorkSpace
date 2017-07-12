using System;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description
        {
            get { return this.description; }
        }

        public EnumDescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
}