using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Exceptions
{
    public class NoMetadataEntityException : Exception
    {
        public NoMetadataEntityException(string message):base(message)
        {
            
        }

        public NoMetadataEntityException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
