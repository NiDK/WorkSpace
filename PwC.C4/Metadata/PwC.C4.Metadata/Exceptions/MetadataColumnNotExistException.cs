using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Exceptions
{
    public class MetadataColumnNotExistException : Exception
    {
        public MetadataColumnNotExistException(string message):base(message)
        {
            
        }

        public MetadataColumnNotExistException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
