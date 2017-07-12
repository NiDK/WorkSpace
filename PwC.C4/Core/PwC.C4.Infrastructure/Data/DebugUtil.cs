using System;
using System.Data.SqlClient;

namespace PwC.C4.Infrastructure.Data
{
    /// <summary>
    /// Debug Output Utility class
    /// </summary>
    public static class DebugUtil
    {
        /// <summary>
        /// Returns string representation of object array
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetParameterString(object[] parameters)
        {
            string ret = string.Empty;

            if (parameters == null) return ret;

            try
            {
                foreach (object obj in parameters)
                {
                    ret += "{" + GetDebugParamValue(obj) + "}";
                }
            }
            catch (Exception) { return "!error!"; }

            return ret;
        }

        /// <summary>
        /// Returns string representation of int array
        /// </summary>
        /// <param name="parameters">Int array</param>
        /// <returns>String</returns>
        public static string GetParameterString(int[] parameters)
        {
            string ret = string.Empty;

            if (parameters == null) return ret;

            try
            {
                foreach (int param in parameters)
                {
                    ret += "{" + param.ToString() + "}";
                }
            }
            catch (Exception) { return "!error!"; }

            return ret;
        }

        /// <summary>
        /// Returns string representation of object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDebugParamValue(object value)
        {
            if (value == null) return "null";

            try
            {
                // Truncate long strings
                if (value is string)
                {
                    string strVal = (string)value;
                    if (strVal == null)
                        return string.Empty;

                    if (strVal.Length > 20)
                        return strVal.Substring(0, 20) + "...";
                    else
                        return strVal;
                }

                if (value.GetType().IsPrimitive || value.GetType().IsValueType || value.GetType().IsEnum ) 
                    return value.ToString();

                return "object:" + value.GetType().Name;
            }
            catch (Exception) { return "!error!"; }
        }

        /// <summary>
        /// Returns string representation of SqlCommand parameters
        /// </summary>
        /// <param name="c">SQL Command</param>
        /// <returns>String</returns>
        public static string GetParameterString(SqlCommand c)
        {
            string ret = string.Empty;

            if (c == null) return ret;

            try
            {
                foreach (SqlParameter p in c.Parameters)
                {
                    ret += "{@" + p.ParameterName + "}={" + DebugUtil.GetDebugParamValue(p.Value) + "}";
                }
            }
            catch (Exception) { return "!error!"; }

            return ret;
        }

    }
}
