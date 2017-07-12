using System;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumeDataFieldNameAttribute : Attribute
    {
        private string explain;
        public string DataFieldName
        {
            get { return this.explain; }
        }

        public EnumeDataFieldNameAttribute(string explain)
        {
            this.explain = explain;
        }
    }
}