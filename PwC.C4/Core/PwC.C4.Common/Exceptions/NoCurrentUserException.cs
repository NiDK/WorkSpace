using System;

namespace PwC.C4.Common.Exceptions
{
    public class NoCurrentUserException : Exception
    {
        public NoCurrentUserException(string message):base(message)
        {
            
        }

        public NoCurrentUserException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
