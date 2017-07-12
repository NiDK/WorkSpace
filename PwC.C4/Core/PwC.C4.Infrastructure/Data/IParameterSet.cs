using System;
using System.Data;
using System.Data.SqlClient;
using PwC.C4.Infrastructure.Exceptions;

namespace PwC.C4.Infrastructure.Data
{
    public enum ParameterDirectionWrap
    {
        Input = 1,
        Output = 2,
        InputOutput = 3,
        ReturnValue = 6
    }

    public interface IParameterSet
    {
        /// <summary>
        /// Adds parameter to <see cref="IParameterSet"/>
        /// </summary>
        /// <param name="key">
        /// Unique key corresponding to stored procedure parameter
        /// </param>
        /// <param name="value">
        /// Value for stored procedure parameter. 
        /// If null, the DBNull value will be used as parameter value for stored procedure
        /// </param>
        void AddWithValue(string key, object value);

        /// <summary>
        /// Adds parameter to <see cref="IParameterSet"/>
        /// </summary>
        /// <param name="key">
        /// Unique key corresponding to stored procedure parameter
        /// </param>
        /// <param name="value">
        /// Value for stored procedure parameter. 
        /// If null, the DBNull value will be used as parameter value for stored procedure
        /// </param>
        /// <param name="check">
        /// Whether to check the parameter value or not
        /// </param>

        void AddWithValue(string key, object value, bool check);

        /// <summary>
        /// Adds parameter to <see cref="IParameterSet"/>
        /// </summary>
        /// <param name="key">
        /// Unique key corresponding to stored procedure parameter
        /// </param>
        /// <param name="value">
        /// Value for stored procedure parameter. 
        /// If null, the DBNull value will be used as parameter value for stored procedure
        /// </param>
        /// <param name="direction">
        /// Parameter direction (Input, InputOutput, Output, ReturnValue)
        /// For output parameters of sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        void AddWithValue(string key, object value, ParameterDirectionWrap direction);

        /// <summary>
        /// Adds parameter to <see cref="IParameterSet"/>
        /// </summary>
        /// <param name="key">
        /// Unique key corresponding to stored procedure parameter
        /// </param>
        /// <param name="value">
        /// Value for stored procedure parameter. 
        /// If null, the DBNull value will be used as parameter value for stored procedure
        /// </param>
        /// <param name="direction">
        /// Parameter direction (Input, InputOutput, Output, ReturnValue)
        /// For output parameters of sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        /// <param name="size">
        /// Size of parameter. Will override inferred parameter size
        /// </param>
        void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size);

        /// <summary>
        /// Adds typed DBNull valued parameter to <see cref="IParameterSet"/>
        /// </summary>
        /// <param name="key">
        /// Unique key corresponding to stored procedure parameter
        /// </param>
        /// <param name="direction">
        /// Parameter direction (Input, InputOutput, Output, ReturnValue)
        /// For output parameters of sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        /// <param name="dbType">
        /// Type of this parameter.   Specified by System.Data.DbType enumration
        /// </param>
        void AddTypedDbNull(string key, ParameterDirectionWrap direction, DbType dbType);

        /// <summary>
        /// Get value of specified parameter 
        /// </summary>
        /// <param name="key">Unique parameter key</param>
        /// <returns>Value of parameter</returns>
        object GetValue(string key);
    }

    internal class ParameterSet : IParameterSet
    {
        private SqlParameterCollection pm;

        internal ParameterSet(SqlParameterCollection sqlParameterCollection)
        {
       
            pm = sqlParameterCollection;
        }

        public object GetValue(string key)
        {
            return pm[key].Value;
        }

        public void AddTypedDbNull(string key, ParameterDirectionWrap direction, DbType dbType)
        {
            AddWithValue(key, DBNull.Value, direction, null, dbType);
        }

        public void AddWithValue(string key, object value)
        {
            AddWithValue(key, value, false);
        }

        public void AddWithValue(string key, object value, bool check)
        {
            if (value == null)
                value = DBNull.Value;

            if (check && !SqlHelper.CheckSqlParameter(value.ToString()))
                throw new SqlParameterException(key, value.ToString());

            // Need to prevent adding a duplicate key
            int index = pm.IndexOf(key);
            if (index == -1)
            {
                pm.AddWithValue(key, value);
            }
            else
            {
                pm[index].Value = value;
            }
        }

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction)
        {
            AddWithValue(key, value, direction, null, null);
        }

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size)
        {
            AddWithValue(key, value, direction, size, null);
        }

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size, DbType? dbType)
        {
            if (value == null)
                value = DBNull.Value;

            SqlParameter sp = new SqlParameter(key, value);

            if (dbType.HasValue)
            {
                sp.DbType = dbType.Value;
            }

            System.Data.ParameterDirection pDir;

            switch (direction)
            {
                case ParameterDirectionWrap.Input:
                    pDir = System.Data.ParameterDirection.Input;
                    break;
                case ParameterDirectionWrap.InputOutput:
                    pDir = System.Data.ParameterDirection.InputOutput;
                    break;
                case ParameterDirectionWrap.Output:
                    pDir = System.Data.ParameterDirection.Output;
                    break;
                case ParameterDirectionWrap.ReturnValue:
                    pDir = System.Data.ParameterDirection.ReturnValue;
                    break;
                default:
                    pDir = System.Data.ParameterDirection.Input;
                    break;
            }

            // Need to prevent adding a duplicate key
            int index = pm.IndexOf(key);
            if (index == -1)
            {
                sp.Direction = pDir;

                if (size.HasValue)
                    sp.Size = size.Value;

                this.pm.Add(sp);
            }
            else
            {
                pm[index].Direction = pDir;
                if (size.HasValue)
                {
                    pm[index].Size = size.Value;
                }
                pm[index].Value = value;
            }
        }
    }

}
