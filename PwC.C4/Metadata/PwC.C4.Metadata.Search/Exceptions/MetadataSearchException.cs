using System;

namespace PwC.C4.Metadata.Search.Exceptions
{
    public class MetadataSearchException : ApplicationException
    {
        public MetadataSearchException(string message, Exception innerException)
            : base(message,innerException)
        {
        }
    }
}