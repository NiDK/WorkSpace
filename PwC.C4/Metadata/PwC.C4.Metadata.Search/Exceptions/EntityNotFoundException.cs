using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string application)
            : base(string.Format("Entity '{0}' was not found", application))
        {
        }
    }
}
