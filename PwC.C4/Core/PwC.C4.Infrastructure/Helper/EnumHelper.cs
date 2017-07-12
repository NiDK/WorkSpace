using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Attributes;

namespace PwC.C4.Infrastructure.Helper
{
    public static class EnumHelper
    {
        public static string GetLable(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var description = value.ToString();
            var fi = value.GetType().GetField(description);
            if (fi != null)
            {
                var attributes = (EnumLableAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumLableAttribute), false);
                description = attributes.Length > 0 ? attributes[0].Lable : String.Empty;
            }
            else
            {
                description = String.Empty;
                //throw new ArgumentException("参数无效。有可能没有定义该枚举类型值。", "value");
            }

            return description;
        }

        public static string GetDescription(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var description = value.ToString();
            var fi = value.GetType().GetField(description);
            if (fi != null)
            {
                var attributes = (EnumDescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumDescriptionAttribute), false);
                description = attributes.Length > 0 ? attributes[0].Description : String.Empty;
            }
            else
            {
                description = String.Empty;
            }

            return description;
        }


        public static string GetExplain(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var enumvaluestr = value.ToString();
            var fi = value.GetType().GetField(enumvaluestr);
            if (fi != null)
            {
                var attributes = (EnumeExplainAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumeExplainAttribute), false);
                return attributes.Length > 0 ? attributes[0].Explain : String.Empty;
            }
            return String.Empty;
        }

        public static bool GetDisplay(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var description = value.ToString();
            var fi = value.GetType().GetField(description);
            if (fi != null)
            {
                var attributes = (EnumeDisplayAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumeDisplayAttribute), false);
                return attributes.Length > 0 && attributes[0].Display;
            }
            else
            {
                return false;
            }

        }

        public static string GetIndexFieldName(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var description = value.ToString();
            var fi = value.GetType().GetField(description);
            if (fi != null)
            {
                var attributes = (EnumeIndexFieldNameAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumeIndexFieldNameAttribute), false);
                if (attributes.Length <= 0) throw new ArgumentException("参数无效。有可能没有定义该枚举类型值。", "value");
                return attributes[0].IndexFieldName;
            }
            throw new ArgumentException("参数无效。有可能没有定义该枚举类型值。", "value");


        }


        public static string GetDataFieldName(this System.Enum value)
        {

            if (value == null)
                throw new ArgumentNullException("value");

            var description = value.ToString();
            var fi = value.GetType().GetField(description);
            if (fi != null)
            {
                var attributes = (EnumeDataFieldNameAttribute[]) fi.GetCustomAttributes(
                    typeof (EnumeDataFieldNameAttribute), false);
                if (attributes.Length > 0)
                {
                    return attributes[0].DataFieldName;
                }
            }
            throw new ArgumentException("参数无效。有可能没有定义该枚举类型值。", "value");


        }

        public static int Key(this System.Enum var)
        {
            return Convert.ToInt32(var);
        }
    }

}
