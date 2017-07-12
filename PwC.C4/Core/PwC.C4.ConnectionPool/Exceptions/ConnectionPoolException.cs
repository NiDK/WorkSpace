using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Exceptions
{
    [Serializable]
    public class ConnectionPoolException : ApplicationException
    {
        public ConnectionPoolException(string message)
            : base(message)
        {
        }

        public ConnectionPoolException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
