using System;
using System.Runtime.Serialization;

namespace PwC.C4.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception thrown if a command is executed without the \'dbo\' specified.
    /// </summary>
    [Serializable]
    public class NoDboException : DataException
    {
        internal NoDboException(string instanceName, string procedureName) : base("Did not specify \"dbo\" before the procedure call " + procedureName + " against database " + instanceName)
        {

        }

        #region Exception serialization support
        protected NoDboException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    };

    /// <summary>
    /// Thrown when a database does not have a configured connection string.
    /// </summary>
    [Serializable]
    public class DatabaseNotConfiguredException : DataException
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="instanceName"></param>
        internal DatabaseNotConfiguredException(string instanceName)
            : base("Unable to retrieve database " + instanceName + " from connection string configuration file.")
        {
        }

        #region Exception serialization support
        protected DatabaseNotConfiguredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    };

    /// <summary>
    /// Thrown when a database has a connectivity issue.
    /// </summary>
    [Serializable]
    public class DatabaseDownException : DataException
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal DatabaseDownException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        internal DatabaseDownException(string message)
            : base(message)
        {

        }

        #region Exception serialization support
        protected DatabaseDownException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }

    /// <summary>
    /// An exception that encapsulates logging logic.
    /// </summary>
    /// <remarks>
    /// No longer logs errors as logging now happens on higher level (The config setting 'LogErrorsToDatabase' is off by default)
    /// </remarks>
    public class C4DataException : Exception
    {
        internal C4DataException(string instanceName, string connectionString, string errorMessage, string operation, Exception innerException)
            : base("Error against: " + instanceName + ":" + errorMessage, innerException)
        {
        }
    }
}
