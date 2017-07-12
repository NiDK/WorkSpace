using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumLableAttribute : Attribute
    {
        private string lable;
        public string Lable
        {
            get { return this.lable; }
        }

        public EnumLableAttribute(string _lable)
        {
            this.lable = _lable;
        }
    }
}
