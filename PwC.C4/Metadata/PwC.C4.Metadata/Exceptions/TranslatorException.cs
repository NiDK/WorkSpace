using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Exceptions
{
    public class TranslatorException : Exception
    {
        public TranslatorException(string message):base(message)
        {
            
        }

        public TranslatorException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
