using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PwC.C4.Infrastructure.Exceptions
{
	/// <summary>
	/// Encapsulates a basic database-centric exception.
	/// </summary>
	[Serializable]
	public class DataException : Exception
	{

		#region DBInstanceName
		protected string instanceName;

		/// <summary>
		/// Database instance name 
		/// </summary>
		public string DBInstanceName
		{
			get { return instanceName; }
			set { instanceName = value; }
		}
		#endregion

		#region ProcedureName
		protected string procedureName;

		/// <summary>
		/// Stored procedure or statement name
		/// </summary>
		public string ProcedureName
		{
			get { return procedureName; }
			set { procedureName = value; }
		}
		#endregion

		#region ShortMessage
		protected string shortMessage;

		/// <summary>
		/// Short message used for hashing and grouping of database exceptions
		/// </summary>
		public string ShortMessage
		{
			get { return shortMessage; }
			set { shortMessage = value; }
		}
		#endregion

		#region InnerException
		protected Exception innerException;

		public Exception InnerException
		{
			get { return innerException; }
			set { innerException = value; }
		}
		#endregion

		internal DataException(string message)
			: base(message)
		{
		}

		internal DataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#region Exception serialization support
		protected DataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// get the custom property out of the serialization stream and set the object's properties
			this.instanceName = info.GetString("DBInstanceName");
			this.procedureName = info.GetString("ProcedureName");
			this.shortMessage = info.GetString("ShortMessage");
			this.innerException = (Exception)info.GetValue("_InnerException", typeof(Exception));
		}

		/// <summary>
		/// Serializes exception
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// add the custom property into the serialization stream
			info.AddValue("DBInstanceName", this.instanceName);
			info.AddValue("ProcedureName", this.procedureName);
			info.AddValue("ShortMessage", this.shortMessage);
			info.AddValue("_InnerException", this.innerException);

			// call the base exception class to ensure proper serialization
			base.GetObjectData(info, context);
		}
		#endregion
	};
}
