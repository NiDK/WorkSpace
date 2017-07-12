using System;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumeExplainAttribute : Attribute
    {
        private string explain;
        public string Explain
        {
            get { return this.explain; }
        }

        public EnumeExplainAttribute(string explain)
        {
            this.explain = explain;
        }
    }
}