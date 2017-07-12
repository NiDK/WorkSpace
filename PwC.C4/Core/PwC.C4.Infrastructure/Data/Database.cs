using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Exceptions;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Infrastructure.Data
{
    internal class DatabaseCollection : System.Collections.ObjectModel.KeyedCollection<string, Database>
    {
        public DatabaseCollection():base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Returns the database instance name.
        /// </summary>
        protected override string GetKeyForItem(Database item)
        {
            return item.InstanceName;
        }

    }

    /// <summary>
    /// Encapsulates information about a particular database and its connectivity.
    /// </summary>
    [Serializable]
    public class Database
    {
        private static readonly LogWrapper log = new LogWrapper();
        private string connectionString;
        
        private string connectionStringAsync;

        internal void SetConnectionString(string connString)
        {
            this.connectionString = connString;
            this.connectionStringAsync = connString + ";async=true;";
        }

        internal Database(string instanceName,string conntectString =null)
        {
            this.instanceName = instanceName;

            try
            {
                var str = conntectString ?? AppSettings.Instance.GetConntectStringV2(instanceName);
                SetConnectionString(str);          
            }
            catch(Exception ee)
            {
                log.Error("Database instanceName error", ee);
                throw new DatabaseNotConfiguredException(instanceName);
            }

        }

        private Database()
        {

        }

        private long lastTimeout;
        private int timeoutCount;
               
        private int connectionsDenied;
        
        private long lastConnectionDenied;

        private int totalTimeoutCount;
        private int totalConnectionsServed;
        private int totalConnectionsDenied;

        private string exceptionLog = string.Empty;

		private object padLock = new object();

		/// <summary>
		/// Register an exception thrown by a sql connection.
		/// </summary>
		public void RegisterSqlTimeout(Exception e)
		{
            exceptionLog = e.ToString() + " : " + e.Message + " : " + e.StackTrace;
            if ((e is InvalidOperationException && e.Message.StartsWith("Timeout expired.")) ||
                (e.InnerException != null && e.InnerException is SqlException && (e.Message.StartsWith("SQL Server does not exist or access denied.") || e.InnerException.Message.StartsWith("An error has occurred while establishing a connection to the server."))) ||
                    (e is SqlException && (e.Message.StartsWith("SQL Server does not exist or access denied.") || e.Message.StartsWith("An error has occurred while establishing a connection to the server."))))
    		{
				lock (padLock)
				{
					lastTimeout = DateTime.Now.Ticks;
					timeoutCount++;
                    totalTimeoutCount++;
                    SqlConnection.ClearPool(new SqlConnection(GetConnectionString()));                    
				}
                if (log.IsErrorEnabled) log.Error("Execption while RegisterSqlTimeout", e);
                throw new DatabaseDownException("Database " + instanceName + " is down.");
			}          
		}
	
		/// <summary>
        /// The name of the database.
        /// </summary>
        private string instanceName;       

		private void CheckConnectivity()
		{

			if (System.Configuration.ConfigurationManager.AppSettings["DatabaseConnectivityState"] == "enabled")
            {
                if (ConnectivityState == ConnectivityState.Down)
                {
                    connectionsDenied++;
                    totalConnectionsDenied++;
                    lastConnectionDenied = DateTime.Now.Ticks;
                    throw new DatabaseDownException("Database " + instanceName + " is down.");
                }
            }
            totalConnectionsServed++;
		}

        /// <summary>
        /// Whether or not the database will throw a downtime exception.
        /// </summary>
        public ConnectivityState ConnectivityState
        {
            get
            {
                if (timeoutCount > 0)
                {
                    //Timeout 2 minutes
                    long downTicks = 1200000000;
                    if (lastTimeout + downTicks > DateTime.Now.Ticks)
                        return ConnectivityState.Down;
                    ResetState();
                }
                return ConnectivityState.Up;
            }
        }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string InstanceName
        {
            get
            {
                return instanceName;
            }
           
        }

        /// <summary>
        /// Resets the state tracking of the database.
        /// </summary>
        public void ResetState()
        {
			timeoutCount = 0;
            connectionsDenied = 0;
        }

        /// <summary>
        /// Retrieves the connection string.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return connectionString;
        }

        public string GetAsyncConnectionString()
        {
            return connectionStringAsync;
        }

        public SqlConnection GetAsyncConnection()
        {
            CheckConnectivity();
            return new SqlConnection(GetAsyncConnectionString());
        }

        /// <summary>
        /// Returns a closed SQL connection.
        /// </summary>
        public SqlConnection GetConnection()
        {
            CheckConnectivity();

            return new SqlConnection(GetConnectionString());
        }

		public SqlConnection GetOpenConnection()
		{
			CheckConnectivity();
			SqlConnection connection = new SqlConnection(GetConnectionString());
			connection.Open();
			return connection;
		}

        /// <summary>
        /// Returns a Database instance.
        /// </summary>
        /// <param name="instanceName">The name of the database (reflected in the ConnectionStrings section of the app config)</param>
        /// <example>  
        /// Database db = Database.GetDatabase("Films");
		///	</example>
        public static Database GetDatabase(string instanceName)
        {
            return DatabaseManager.Instance.GetDatabase(instanceName);
        }

        public static Database GetDatabase(string instanceName, string connectString)
        {
            return DatabaseManager.Instance.GetDatabase(instanceName, connectString);
        }


        /// <summary>
        /// Get all registered Database instances (one per database).
        /// </summary>
        /// <returns></returns>
        public static List<Database> GetDatabases()
        {
            return new List<Database>(DatabaseManager.Instance.Databases);
        }

        /// <summary>
        /// Retrieve a CLOSED SqlConnection object via an instance name.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string instanceName)
        {
            return GetDatabase(instanceName).GetConnection();
        }


        #region Instrumentation Presentation Methods

        /// <summary>
        /// Retrieves basic state information and statistics on the database.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public string GetHtmlStatus(StringBuilder sb)
        {
            if (ConnectivityState == ConnectivityState.Down)
            {
                sb.Append("<dl class=\"dangerousServer\">");
            }
            else 
            {
                sb.Append("<dl class=\"happyServer\">");
            }
            
            AddHeaderLine(sb, this.InstanceName);
           
            AddPropertyLine(sb, "Total Exceptions", totalTimeoutCount);
            AddPropertyLine(sb, "Total Connections Served", totalConnectionsServed);
            AddPropertyLine(sb, "Total Connections Denied", totalConnectionsDenied);

            if (timeoutCount > 0 && connectionsDenied > 0)
            {
                sb.Append("<hr/>");
                AddPropertyLine(sb, "Connections Denied Since Downed", connectionsDenied);
                if (connectionsDenied > 0)
                    AddPropertyLine(sb, "Last Connection Denied", new DateTime(lastConnectionDenied).ToString());
            }
            if (lastTimeout > 0)
            {
                sb.Append("<hr/>");
                AddPropertyLine(sb, "Last Exception Caught", new DateTime(lastTimeout).ToString());
                sb.AppendFormat("<dd class=\"exception\">{0}</dd>", exceptionLog);
            }
            sb.Append("</dl>");
            
            return sb.ToString();
        }

        private void AddPropertyLine(StringBuilder sb, string propName, object propValue)
        {
            if (propValue.ToString() == "0") {}
            else
                sb.AppendFormat("<dt>{0}:</dt><dd>{1}</dd>", propName, propValue);
        }

        private void AddHeaderLine(StringBuilder sb, object propValue)
        {
            sb.AppendFormat("<h3>{0}:</h3>", propValue);
        }

        #endregion
    }

    /// <summary>
    /// The state of the database as understood by the client.
    /// </summary>
    public enum ConnectivityState
    {
        /// <summary>
        /// The database can be connected to
        /// </summary>
        Up = 0,
        /// <summary>
        /// The database cannot be connected to.
        /// </summary>
        Down = 1,
    }

}
