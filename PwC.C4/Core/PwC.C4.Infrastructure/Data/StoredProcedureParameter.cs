namespace PwC.C4.Infrastructure.Data
{
	public class StoredProcedureParameter
	{
		#region Key
		private string key;

		/// <summary>
		/// This can be null. If null, the position of this argument within the argument list will be used to pass the value.
		/// </summary>
		public string Key
		{
			get { return key; }
			set { key = value; }
		}
		#endregion

		#region Value
		private object value;

		public object Value
		{
			get { return value; }
			set { this.value = value; }
		}
		#endregion

		#region Parameter Direction
		private ParameterDirectionWrap parameterDirection;

		public ParameterDirectionWrap ParameterDirection
		{
			get { return parameterDirection; }
			set { parameterDirection = value; }
		}
		#endregion

		#region Size
		private int? size;

		public int? Size
		{
			get { return size;}
			set { size = value;}
		}
		#endregion

		/// <summary>
		/// Create a parameter to be passed by parameter name.
		/// </summary>
		public StoredProcedureParameter(string key, object value, ParameterDirectionWrap parameterDirection, int size) 
		{
			this.key = key;
			this.value = value;
			this.parameterDirection = parameterDirection;
			this.size = size;
		}
		/// <summary>
		/// Create a parameter to be passed by parameter name.
		/// </summary>
		public StoredProcedureParameter(string key, object value, ParameterDirectionWrap parameterDirection)
		{
			this.key = key;
			this.value = value;
			this.parameterDirection = parameterDirection;
			this.size = null;
		}

		/// <summary>
		/// Create an parameter to be passed by parameter index.
		/// </summary>
		public StoredProcedureParameter(object value, ParameterDirectionWrap parameterDirection, int size)
		{
			this.key = null;
			this.value = value;
			this.parameterDirection = parameterDirection;
			this.size = size;
		}
		/// <summary>
		/// Create an parameter to be passed by parameter index.
		/// </summary>
		public StoredProcedureParameter(object value, ParameterDirectionWrap parameterDirection)
		{
			this.key = null;
			this.value = value;
			this.parameterDirection = parameterDirection;
			this.size = null;
		}
		/// <summary>
		/// Create an input parameter to be passed by parameter index.
		/// </summary>
		public StoredProcedureParameter(object value)
		{
			this.key = null;
			this.value = value;
			this.parameterDirection = ParameterDirectionWrap.Input;
			this.size = null;
		}
	}
}
