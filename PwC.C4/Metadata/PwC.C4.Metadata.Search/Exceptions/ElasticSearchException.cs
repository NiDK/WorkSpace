using System;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class ElasticSearchException : ApplicationException
    {
        public ElasticSearchException(string message)
            : base(message)
        {
        }
    }
}
