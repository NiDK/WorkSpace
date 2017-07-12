using System;

namespace PwC.C4.Common.Exceptions
{
    public class JavascriptException : Exception
    {
        public JavascriptException(string message):base(message)
        {
            
        }

        public JavascriptException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
