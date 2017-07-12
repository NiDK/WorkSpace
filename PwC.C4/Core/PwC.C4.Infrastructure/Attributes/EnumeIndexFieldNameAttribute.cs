using System;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumeIndexFieldNameAttribute : Attribute
    {
        private string explain;
        public string IndexFieldName
        {
            get { return this.explain; }
        }

        public EnumeIndexFieldNameAttribute(string explain)
        {
            this.explain = explain;
        }
    }
}