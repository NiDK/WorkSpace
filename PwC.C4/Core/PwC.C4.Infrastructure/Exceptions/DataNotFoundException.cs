namespace PwC.C4.Infrastructure.Exceptions
{
    /// <summary>
    /// This exception is thrown if no data is returned when some data was expected.
    /// </summary>
    public class DataNotFoundException : DataException
    {
        /// <summary>
        /// Constructs a DataNotFoundException exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        public DataNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Constructs a DataNotFoundException exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="dbInstanceName">The database instance name.</param>
        public DataNotFoundException(string message, string dbInstanceName)
            : base(message)
        {
            this.DBInstanceName = dbInstanceName;
        }

        /// <summary>
        /// Constructs a DataNotFoundException exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="dbInstanceName">The database instance name.</param>
        /// <param name="procedureName">The procedure name.</param>
        public DataNotFoundException(string message, string dbInstanceName, string procedureName)
            : base(message)
        {
            this.DBInstanceName = dbInstanceName;
            this.procedureName = procedureName;
        }
    }
}