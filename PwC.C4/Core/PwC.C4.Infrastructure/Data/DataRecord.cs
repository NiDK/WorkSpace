using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace PwC.C4.Infrastructure.Data
{


    /// <summary>
    /// Wraps an individual database result row.
    /// </summary>
    public interface IRecord
    {
        #region Custom Calls

        bool GetExist(string name);

        /// <summary>
        /// Returns the item of type T inhabiting the field index.  Will throw exception if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        T Get<T>(int i);

        /// <summary>
        /// Returns the item of type T inhabiting the field index.  If DBNull, will return the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetOrDefault<T>(int i, T defaultValue);

        /// <summary>
        /// Returns the item of type T inhabiting the field name.  Will throw exception if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        T Get<T>(string fieldName);

        /// <summary>
        /// Returns an item of type T inhabiting the field name.  If DBNull, will return the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetOrDefault<T>(string fieldName, T defaultValue);

        /// <summary>
        /// Returns an item of type T inhabiting the field index.  If DBNull, will return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        T GetOrNull<T>(int fieldIndex) where T : class;

        /// <summary>
        /// Returns an item of type T inhabiting the field name.  If DBNull, will return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        T GetOrNull<T>(string fieldName) where T : class;

        /// <summary>
        /// Returns a string, or empty if the value is null.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        string GetStringOrEmpty(int i);
        
        /// <summary>
        /// Returns a string, or the supplied default if the string is DBNull.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        string GetStringOrDefault(int i, string defaultString);

        /// <summary>
        /// Returns a string, or null if the string is DBNull.
        /// </summary>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        string GetStringOrNull(int fieldIndex);


		/// <summary>
		/// Decompresses a byte array and returns the corresponding string, or null if the byte array is null.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <returns></returns>
		string GetStringOrNullFromCompressedByteArray(int fieldIndex);

        /// <summary>
        /// Will return an Int32, or the default value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int GetInt32OrDefault(int fieldIndex, int defaultValue);

        /// <summary>
        /// Will return the initialized value of Int32 if the item is DBNull.
        /// </summary>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        int GetInt32OrEmpty(int fieldIndex);

		/// <summary>
		/// Will convert a byte and return an Int32, or the default value if the byte is DBNull.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		int GetInt32OrDefaultFromByte(int fieldIndex, int defaultValue);

		/// <summary>
		/// Will convert a byte and return an Int32, initialized value of Int32 if the item is DBNull.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <returns></returns>
		int GetInt32OrEmptyFromByte(int fieldIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <returns>a byte array or null if the item is DBNull.</returns>
		Byte[] GetByteArrayOrNull(int fieldIndex);

		/// <summary>
		/// Will return an Int16, or the default value if the Int16 is DBNull.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		short GetInt16OrDefault(int fieldIndex, short defaultValue);

        /// <summary>
		/// Will return a DateTime object, or the default value if the DateTime is DBNull.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		DateTime GetDateTimeOrEmpty(int fieldIndex);

		/// <summary>
		/// Will return an SqlXml object or null if value is DBNull or if the underlying data set does not support the XML data type.
		/// </summary>
		/// <param name="fieldIndex"></param>
		/// <returns></returns>
		SqlXml GetSqlXml(int fieldIndex);

        #endregion

        #region IDataRecord implementation
        int FieldCount { get; }
        object this[int i] { get; }
        object this[string name] { get; }
        bool GetBoolean(int i);
        bool GetBooleanFromByte(int i);
        bool GetBooleanOrFalse(int i);
        byte GetByte(int i);
        byte GetByteOrDefault(int i, byte defaultValue);
        long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
        char GetChar(int i);
        long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);
        IDataReader GetData(int i);
        string GetDataTypeName(int i);
        DateTime GetDateTime(int i);
        decimal GetDecimal(int i);
        double GetDouble(int i);
        Type GetFieldType(int i);
        float GetFloat(int i);
        Guid GetGuid(int i);
        short GetInt16(int i);
        int GetInt32(int i);
        int? GetInt32Nullable(int i);
        short? GetInt16Nullable(int i);
        long GetInt64(int i);
        string GetName(int i);
        int GetOrdinal(string name);
        string GetString(int i);
        object GetValue(int i);
        int GetValues(object[] values);

        /// <summary>
        /// Whether or not the value is null in the database.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        bool IsDBNull(int i);

        #endregion
    }

    /// <summary>
    /// Wraps a resultset from a database call.
    /// </summary>
    public interface IRecordSet : IDisposable, IRecord
    {
        #region Custom Calls



        #endregion

        int Depth { get; }
        bool IsClosed { get; }


        int RecordsAffected { get; }
        /// <summary>
        /// Returns the database connection to the pool, or closes a non-pooled connection.
        /// </summary>
        void Close();
        
        /// <summary>
        /// Returns information on the underlying schema.
        /// </summary>
        /// <returns></returns>
        DataTable GetSchemaTable();

        /// <summary>
        /// Move to the next result set.
        /// </summary>
        /// <returns></returns>
        bool NextResult();
        /// <summary>
        /// Read in the next record (or read the 1st record if the result is fresh.)
        /// </summary>
        /// <returns></returns>
        bool Read();
    }

    
    /// <summary>
    /// An encapsulation of an IDataReader
    /// </summary>
    internal class DataRecord : IRecord, IRecordSet
    {
        private IDataReader wr;

        internal DataRecord(IDataReader wrappedReader)
        {
            wr = wrappedReader;
        }

        #region IDataRecord Members

        public int FieldCount
        {
            get { return wr.FieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return wr.GetBoolean(i);
        }

        public bool GetBooleanFromByte(int i)
        {
            return (wr.GetByte(i) == 1);
        }

        public byte GetByte(int i)
        {
            return wr.GetByte(i);
        }

        public byte GetByteOrDefault(int i, byte defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : wr.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return wr.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return wr.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return wr.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
			return wr.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return wr.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return wr.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return wr.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return wr.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return wr.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return wr.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return wr.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return wr.GetInt16(i);
        }

        public int GetInt32FromByte(int i)
		{
			return Convert.ToInt32(wr.GetByte(i));
		}

        public int GetInt32(int i)
        {
            return wr.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return wr.GetInt64(i);
        }

        public string GetName(int i)
        {
            return wr.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return wr.GetOrdinal(name);
        }

        public bool GetExist(string name)
        {
            for (var i = 0; i < wr.FieldCount; i++)
            {
                if (wr.GetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public string GetString(int i)
        {
            return wr.GetString(i);
        }

        public object GetValue(int i)
        {
            return wr.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return wr.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return wr.IsDBNull(i);
        }

        public object this[string name]
        {
            get { return wr[name]; }
        }

        public object this[int i]
        {
            get { return wr[i]; }
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            wr.Close();
        }

        public int Depth
        {
            get { return wr.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return wr.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return wr.IsClosed; }
        }

        public bool NextResult()
        {
            return wr.NextResult();
        }

        public bool Read()
        {
            return wr.Read();
        }

        public int RecordsAffected
        {
            get { return wr.RecordsAffected; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            wr.Dispose();
        }

        #endregion

        #region IRecord String Members


        public string GetStringOrDefault(int i, string defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : GetString(i);
        }

        public string GetStringOrEmpty(int i)
        {
            return GetStringOrDefault(i, String.Empty);
        }

        public string GetStringOrNull(int i)
        {
            return (IsDBNull(i)) ? null : GetString(i);
        }

        public string GetStringOrNullFromCompressedByteArray(int fieldIndex)
        {
            //Byte[] compressedString = GetByteArrayOrNull(fieldIndex);
            //if (compressedString == null || compressedString.Length == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    return UnicodeStringCompressor.Decompress(compressedString);
            //}

            return null;
        }
		

        #endregion

        #region IRecord Members

        public short? GetInt16Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt16(fieldIndex);
        }

        public int? GetInt32Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt32(fieldIndex);
        }

        public int GetInt32OrDefault(int fieldIndex, int defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32(fieldIndex);
        }

        public int GetInt32OrEmpty(int fieldIndex)
        {
            return GetInt32OrDefault(fieldIndex, Int32.MinValue);
        }

		public int GetInt32OrDefaultFromByte(int fieldIndex, int defaultValue)
		{
			return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32FromByte(fieldIndex);
		}

		public int GetInt32OrEmptyFromByte(int fieldIndex)
        {
			return GetInt32OrDefaultFromByte(fieldIndex, Int32.MinValue);
        }

		public short GetInt16OrDefault(int fieldIndex, short defaultValue)
		{
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt16(fieldIndex); 
		}

		public Byte[] GetByteArrayOrNull(int fieldIndex)
		{
			if (IsDBNull(fieldIndex))
			{
				return null;
			}
			Byte[] bytes = new Byte[(GetBytes(fieldIndex, 0, null, 0, int.MaxValue))];
			GetBytes(fieldIndex, 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public DateTime GetDateTimeOrEmpty(int fieldIndex)
		{
			return (IsDBNull(fieldIndex)) ? DateTime.MinValue : GetDateTime(fieldIndex);
		}

		public SqlXml GetSqlXml(int fieldIndex)
		{
			if(IsDBNull(fieldIndex))
				return null;

			SqlDataReader sdr = wr as SqlDataReader;
			if(sdr == null)
				return null;

			return sdr.GetSqlXml(fieldIndex);
		}

        #endregion

        #region IRecord Members

        public T Get<T>(int i)
        {
            if (typeof(string) == typeof(T))
            {
                object defaultvalue = "";
                return IsDBNull(i) ? (T)defaultvalue : (T)this[i];
            }
                
            return (T)this[i];
        }

        public T Get<T>(string fieldName)
        {
            if (typeof(string)==typeof(T))
            {
                object defaultvalue = "";
                return (IsDBNull(this.GetOrdinal(fieldName))) ? (T)defaultvalue : (T)this[fieldName];
            }
            return (T)this[fieldName];
        }


        public T GetOrDefault<T>(int i, T defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : Get<T>(i);
        }

        public T GetOrDefault<T>(string fieldName, T defaultValue)
        {

            return (IsDBNull(this.GetOrdinal(fieldName))) ? defaultValue : Get<T>(fieldName);
        }

        public T GetOrNull<T>(int fieldIndex) where T : class
        {
            return (IsDBNull(fieldIndex)) ? null : Get<T>(fieldIndex);
        }

        public T GetOrNull<T>(string fieldName) where T : class
        {
            return (IsDBNull(this.GetOrdinal(fieldName))) ? null : Get<T>(fieldName);
        }


        #endregion


        #region IRecord Members

  
        public bool GetBooleanOrFalse(int fieldIndex)
        {
            return (IsDBNull(fieldIndex)) ? false : GetBoolean(fieldIndex);
        }

        #endregion
    }

}
