using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using PwC.C4.TemplateEngine.Common;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.TemplateEngine.Model
{
    public class SafeDynamic:DynamicObject
    {
        private dynamic _data = null;
        private bool _isAnonymous = false;

        public SafeDynamic()
        {
            
        }

        public SafeDynamic(dynamic data, bool isAnonymous = false)
        {
            this._data = data;
            this._isAnonymous = isAnonymous;
        }

        public SafeDynamic(XElement xElement)
        {
            this._data = DynamicXmlConverter.Parse(xElement);
        }

        public SafeDynamic(string jsonData)
        {
            this._data = DynamicJsonConverter.Parse(jsonData);
        }

        public SafeDynamic(string data, ConvertType type)
        {
            switch (type)
            {
                case ConvertType.Xml:
                    this._data = DynamicXmlConverter.Parse(data);
                    break;
                case ConvertType.Json:
                    this._data = DynamicJsonConverter.Parse(data);
                    break;
            }
        }

        private dynamic GetExpandoObjectValue(string name)
        {
            foreach (var property in ((IDictionary<String, Object>)_data).Where(property => property.Key == name))
            {
                return property.Value;
            }
            return string.Empty;
        }

        private dynamic GetAnonymousValue(string name)
        {
            var typs = _data.GetType().GetProperties();

            foreach (var typ in typs)
            {
                if (typ.Name != name) continue;
                return typ.GetValue(_data);
            }
            return string.Empty;
        }

        #region Override DynamicObject 的方法
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return base.GetDynamicMemberNames();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_data is System.Dynamic.ExpandoObject)
            {
                result = GetExpandoObjectValue(binder.Name);
                return true;
            }
            else if(_isAnonymous)
            {
                result = GetAnonymousValue(binder.Name);
                return true;
            }
            try
            {
                return _data.TryGetMember(binder, out result);
            }
            catch (Exception)
            {
                result = string.Empty;
                return true;
            }
            
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return _data.TrySetMember(binder, value);
        }
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return _data.TryInvoke(binder, args, out result);
        }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return _data.TryInvokeMember(binder, args, out result);
        }
        #endregion
        public dynamic GetValue<T>(string name)
        {
            var typs = _data.GetType().GetProperties();

            foreach (var typ in typs)
            {
                if (typ.Name != name) continue;
                var baseData = typ.GetValue(_data);

                if (typeof(DateTime) == typeof(T))
                {
                    if (baseData is DateTime)
                        return baseData;
                    DateTime resDate;
                    if (DateTime.TryParse(baseData, out resDate))
                    {
                        return resDate;
                    }
                    return DateTime.MinValue;
                }
                else if (typeof(string) == typeof(T))
                {
                    return baseData.ToString();
                }
                else if (typeof(int) == typeof(T))
                {
                    if (baseData is int)
                        return baseData;
                    int resInt;
                    if (Int32.TryParse(baseData, out resInt))
                    {
                        return resInt;
                    }
                    return 0;
                }
                else
                {
                    return baseData;
                }
            }
            return string.Empty;
        }
    }
}
