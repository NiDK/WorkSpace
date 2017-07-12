using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PwC.C4.Infrastructure.Data
{
    class CommandCache : Dictionary<string, SqlCommand>
    {
		internal SqlCommand GetCommandCopy(SqlConnection connection, string databaseInstanceName, string procedureName)
        {
            SqlCommand copiedCommand;
			string commandCacheKey = databaseInstanceName + procedureName;

			if (!this.ContainsKey(commandCacheKey))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;

				if(connection.State != ConnectionState.Open)
					connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                connection.Close();

                lock (this)
                {
					this[commandCacheKey] = command;
                }
            }

			copiedCommand = this[commandCacheKey].Clone();
            copiedCommand.Connection = connection;
            return copiedCommand;
        }
    }
}
