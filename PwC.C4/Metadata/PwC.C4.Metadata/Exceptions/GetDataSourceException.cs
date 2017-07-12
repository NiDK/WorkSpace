using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Exceptions
{
    public class GetDataSourceException : Exception
    {
        public GetDataSourceException(string message):base(message)
        {
            
        }

        public GetDataSourceException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
