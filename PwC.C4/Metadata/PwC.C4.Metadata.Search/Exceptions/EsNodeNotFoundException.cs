using System;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class EsNodeNotFoundException : ApplicationException
    {
        public EsNodeNotFoundException(string application)
            : base(string.Format("EsNode '{0}' was not found", application))
        {
        }
    }
}