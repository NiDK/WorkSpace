using System;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumeDisplayAttribute : Attribute
    {
        private bool display;
        public bool Display
        {
            get { return this.display; }
        }

        public EnumeDisplayAttribute(bool display)
        {
            this.display = display;
        }
    }
}