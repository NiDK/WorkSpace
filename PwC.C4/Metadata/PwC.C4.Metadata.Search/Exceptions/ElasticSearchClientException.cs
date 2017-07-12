using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class ElasticSearchClientException : ApplicationException
    {
        public ElasticSearchClientException(string message)
            : base(message)
        {
        }

        public ElasticSearchClientException(string message, Exception innerException)
            : base(message,innerException)
        {
        }
    }
}
