using System.Collections.Generic;

namespace PwC.C4.Infrastructure.Helper
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        string _propertyName;
        public EqualityComparer()
        {
            _propertyName = "ID";
        }
        public EqualityComparer(string propertyName)
        {
            _propertyName = propertyName;
        }

        public bool Equals(T x, T y)
        {
            return typeof(T).GetProperty(_propertyName).GetValue(x, null).Equals(typeof(T).GetProperty(_propertyName).GetValue(y, null));
        }

        public int GetHashCode(T obj)
        {
            return typeof(T).GetProperty(_propertyName).GetValue(obj, null).GetHashCode();
        }
    }
}
