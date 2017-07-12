using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Security.Permissions;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.Infrastructure.Exceptions
{
	/// <summary>
	/// Thrown by Low level database access classes (Procedure class)
	/// </summary>
	[Serializable]
	public class DatabaseExecutionException : DataException
	{
		#region Database
		protected Database database;

		/// <summary>
		/// Affected database 
		/// </summary>
		public Database Database
		{
			get { return database; }
			set { database = value; }
		}
		#endregion

		#region Command
		private SqlCommand command;

		public SqlCommand Command
		{
			get { return command; }
			set { command = value; }
		}
		#endregion

		private static string GetParameterString(SqlCommand c)
		{
			string ret = string.Empty;

			try
			{
				if (c == null) return string.Empty;

				foreach (SqlParameter p in c.Parameters)
				{
					ret += "{@" + p.ParameterName + "}={" + p.Value + "}";
				}
			}
			catch (Exception) { } // Ignore these

			return ret;
		}

		public DatabaseExecutionException(Database database, string procedureName, Exception innerException)
			:
			base(
			"Database Exception Against Database: " + database.InstanceName + " \n "
			+ "\n With procedure: " + procedureName + " \n "
			+ "\n With inner exception message: " + innerException.Message,
			innerException
			)
		{
			this.database = database;
			this.procedureName = procedureName;
			this.innerException = innerException;
			this.instanceName = database.InstanceName;
			this.shortMessage =
				"Database Exception Against Database: " + database.InstanceName +
				"\n With inner exception message: " + innerException.Message;
		}

		public DatabaseExecutionException(Database database, string procedureName, SqlCommand command, Exception innerException)
			:
			base(
			"Database Exception Against Database: " + database.InstanceName + " \n "
			+ "\n With procedure: " + procedureName + " \n "
			+ "\n With parameters: " + GetParameterString(command)
			+ "\n With inner exception message: " + innerException.Message,
			innerException
			)
		{
			this.database = database;
			this.procedureName = procedureName;
			this.command = command;
			this.innerException = innerException;
			this.instanceName = database.InstanceName;
			this.shortMessage =
				"Database Exception Against Database: " + database.InstanceName +
				"\n With inner exception message: " + innerException.Message;
		}

		public DatabaseExecutionException(Database database, string procedureName, string contextInfo, Exception innerException)
			:
			base(
			"Database Exception Against Database: " + database.InstanceName + " \n "
			+ "\n With procedure: " + procedureName + " \n "
			+ "\n With context info: " + contextInfo + " \n "
			+ "\n With inner exception message: " + innerException.Message,
			innerException
			)
		{
			this.database = database;
			this.procedureName = procedureName;
			this.innerException = innerException;
			this.instanceName = database.InstanceName;
			this.shortMessage =
				"Database Exception Against Database: " + database.InstanceName +
				"\n With inner exception message: " + innerException.Message;
		}

		#region Exception serialization support
		protected DatabaseExecutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// get the custom property out of the serialization stream and set the object's properties
			this.database = (Database)info.GetValue("Database", typeof(Database));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// add the custom property into the serialization stream
			info.AddValue("Database", this.database);

			// call the base exception class to ensure proper serialization
			base.GetObjectData(info, context);
		}
		#endregion
	}
}
