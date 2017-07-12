using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class ApplicationNotFoundException : ApplicationException
    {
        public ApplicationNotFoundException(string application)
            : base(string.Format("Application '{0}' was not found", application))
        {
        }
    }
}
