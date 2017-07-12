using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.DataService.Persistance;

namespace PwC.C4.DataService
{
    public class C4CacheService : IC4CacheService
    {
        public string Get(string appcode, string key)
        {
            return PreferenceDao.Get(appcode,key);
        }

        public bool Set(string appcode, string key, string value)
        {
            return PreferenceDao.Set(appcode, key, value);
        }

        public bool DeleteKey(string appcode, string key)
        {
            return PreferenceDao.DeleteKey(appcode, key);
        }
    }
}
