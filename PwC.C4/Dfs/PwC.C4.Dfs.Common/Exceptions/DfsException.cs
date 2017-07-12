using System;

namespace PwC.C4.Dfs.Common.Exceptions
{
    public class DfsException : ApplicationException
    {
        public DfsException(string message)
            : base(message)
        {
        }

        public DfsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
