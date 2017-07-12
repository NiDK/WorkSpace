using System;

namespace PwC.C4.Common.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException(string message):base(message)
        {
            
        }

        public EmailException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
