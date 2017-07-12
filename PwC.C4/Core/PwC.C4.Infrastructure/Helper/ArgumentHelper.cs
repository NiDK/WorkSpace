using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PwC.C4.Infrastructure.Helper
{
    /// <summary>
    /// Common argument checking calls
    /// </summary>
    public static class ArgumentHelper
    {

        #region AssertNotNull
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> exception if any of the values are null 
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <exception cref="ArgumentNullException"/>
        public static void AssertValuesNotNull<T>(params T[] values) where T : class
        {
            foreach (T value in values)
                if (value == null)
                    throw new ArgumentNullException();
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> exception if any of values are null 
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <param name="paramName">Name of parameter(s)</param>
        /// <exception cref="ArgumentNullException"/>
        public static void AssertValuesNotNull<T>(string paramName, params T[] values) where T : class
        {
            foreach (T value in values)
                if (value == null)
                    throw new ArgumentNullException("Argument name: " + paramName);
        }
        public static void AssertNotNull<T>(T value, string paramName) where T : class
        {
            AssertValuesNotNull<T>(paramName, value);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> exception if any of the values are null
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <exception cref="ArgumentNullException"/>
        public static void AssertValuesNotNull<T>(params Nullable<T>[] values) where T : struct
        {
            foreach (Nullable<T> value in values)
                if (!value.HasValue)
                    throw new ArgumentNullException();
        }
        public static void AssertNotNull<T>(T value) where T : class
        {
            AssertValuesNotNull<T>(value);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> exception if any of the values are null 
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <param name="paramName">Name of parameter</param>
        /// <exception cref="ArgumentNullException"/>
        public static void AssertValuesNotNull<T>(string paramName, params Nullable<T>[] values) where T : struct
        {
            foreach (Nullable<T> value in values)
                if (!value.HasValue)
                    throw new ArgumentNullException("Argument name: " + paramName);
        }
        public static void AssertNotNull<T>(Nullable<T> value, string paramName) where T : struct
        {
            AssertValuesNotNull<T>(paramName, value);
        }
        public static void AssertNotNull<T>(Nullable<T> value) where T : struct
        {
            AssertValuesNotNull<T>(value);
        }
        #endregion

        #region AssertNotEmpty
        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> exception if any of the strings are empty or null
        /// </summary>
        /// <param name="values">List of values to check</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void AssertValuesNotEmpty(params string[] values)
        {
            foreach (string value in values)
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length == 0)
                    throw new ArgumentOutOfRangeException(string.Empty, "String is empty.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> exception if any of the strings are empty or null
        /// </summary>
        /// <param name="values">List of values to check</param>
        /// <param name="paramName">Name of parameter</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        // Important: Remove this function to make "AssertNotEmpty(string value)" and "AssertValuesNotEmpty(params string[] values)" work
        //public static void AssertValuesNotEmpty(string paramName, params string[] values)
        //{
        //    foreach (string value in values)
        //    {
        //        if (value == null)
        //            throw new ArgumentNullException("Argument name: " + paramName + ".");
        //        if (value.Length == 0)
        //            throw new ArgumentOutOfRangeException("String is empty. Argument name: " + paramName + ".");
        //    }
        //}

        public static void AssertNotEmpty(string value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
            if (value.Length == 0)
                throw new ArgumentOutOfRangeException(paramName, "String is empty");
        }
        public static void AssertNotEmpty(string value)
        {
            AssertValuesNotEmpty(value);
        }
        #endregion

        #region AssertPositive
        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> exception if any of the values are not positive
        /// </summary>
        /// <param name="values">List of values to check</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void AssertValuesPositive(params int[] values)
        {
            foreach (int value in values)
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Argument not positive. Value: (" + value + ").");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> exception if any of the values are not positive
        /// </summary>
        /// <param name="values">List of values to check</param>
        /// <param name="paramName">Name of parameter</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void AssertValuesPositive(string paramName, params int[] values)
        {
            foreach (int value in values)
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Argument not positive. Argument name: " + paramName + "; Value: " + value + ".");
        }

        public static void AssertPositive(int value, string paramName)
        {
            AssertValuesPositive(paramName, value);
        }

        public static void AssertPositive(int value)
        {
            AssertValuesPositive(value);
        }
        #endregion

        #region AssertInRange
        public static void AssertInRange(string paramName, int value, int rangeBegin, int rangeEnd)
        {
            if (value < rangeBegin || value > rangeEnd)
                throw new ArgumentOutOfRangeException(paramName,
                    string.Format("{0} must be within the range {1} - {2}.  The value given was {3}.", paramName, rangeBegin, rangeEnd, value));
        }



        #endregion

        #region AssertIsTrue and IsFalse
        public static void AssertIsTrue(bool assertion, string message)
        {
            if (!assertion)
                throw new ArgumentException(message);
        }
        public static void AssertIsTrue(bool assertion, string messageTemplate, params object[] parameters)
        {
            if (!assertion)
                throw new ArgumentException(string.Format(messageTemplate, parameters));
        }
        public static void AssertIsFalse(bool assertion, string message)
        {
            AssertIsTrue(!assertion, message);
        }
        public static void AssertIsFalse(bool assertion, string messageTemplate, params object[] parameters)
        {
            AssertIsTrue(!assertion, messageTemplate, parameters);
        }
        #endregion

        #region 正则验证

        public static void AssertIsRegex(string regex, params string[] values)
        {
            Regex r = new Regex(regex);
            foreach (var value in values)
                if (!r.IsMatch(value))
                    throw new ArgumentOutOfRangeException(value,
                      string.Format("{0} must be accorded with the regex。The value given was {0}.", value));

        }
        public static void AssertIsGuid(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var str = values[i];
                try
                {
                    var b = new Guid(str);
                }
                catch (Exception ex)
                {
                    throw new ArgumentOutOfRangeException(str,
                          string.Format("{0}不是合法的GUID。The value given was {0}.", str));
                }
            }
        }

        public static void AssertIsGuidList(IEnumerable<string> values)
        {
            foreach (var value in values)
                AssertIsGuid(value);

        }

        #endregion


        #region 参数类型验证

        public static void AssertType<T>(params object[] objs)
        {
            foreach (var obj in objs)
                if (!(obj is T))
                    throw new ArgumentOutOfRangeException(string.Format("the value isn't {0} Type", typeof(T).Name));


        }
        #endregion

        /// <summary>
        /// 验证email是否是合法的email格式
        /// </summary>
        /// <param name="email"></param>
        public static void AssertIsEmail(string email)
        {
            AssertNotNullOrEmpty(email);
            if(StringOperate.IsEmail(email) == false)
            {
                throw new ArgumentException(string.Format("{0}不是合法的Email格式", email));
            }
        }

        #region 字符串空验证
        public static void AssertNotNullOrEmpty(params string[] para)
        {
            foreach (var obj in para)
                if (string.IsNullOrEmpty(obj))
                    throw new ArgumentNullException(obj, "the string value is null or empty");

        }

        #endregion
    }




    /// <summary>
    /// Common result checking calls
    /// </summary>
    public static class OutputResultHelper
    {
        #region AssertNotNull
        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> exception if any of the values are null 
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <exception cref="InvalidOperationException"/>
        public static void AssertValuesNotNull<T>(params T[] values) where T : class
        {
            foreach (T value in values)
                if (value == null)
                    throw new InvalidOperationException("Some or all values are null. Type: " + typeof(T).FullName);
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> exception if any of values are null 
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="values">List of values to check</param>
        /// <param name="paramName">Name of parameter(s)</param>
        /// <exception cref="InvalidOperationException"/>
        public static void AssertValuesNotNull<T>(string paramName, params T[] values) where T : class
        {
            foreach (T value in values)
                if (value == null)
                    throw new InvalidOperationException("Some or all values are null. Name: " + paramName + "Type: " + typeof(T).FullName);
        }

        public static void AssertNotNull<T>(T value, string paramName) where T : class
        {
            AssertValuesNotNull<T>(paramName, value);
        }
        #endregion

        #region AssertInRange
        public static void AssertInRange(string paramName, int value, int rangeBegin, int rangeEnd)
        {
            if (value < rangeBegin || value > rangeEnd)
                throw new InvalidOperationException(
                    string.Format("Value is null: Parameter '{0}' must be within the range {1} - {2}.  The value given was {3}.", paramName, rangeBegin, rangeEnd, value));
        }
        #endregion
    }

}

