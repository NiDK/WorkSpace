using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IC4CacheService
    {
        #region Log

        [OperationContract]
        string Get(string appcode,string key);

        [OperationContract]
        bool Set(string appcode, string key, string value);

        [OperationContract]
        bool DeleteKey(string appcode, string key);


        #endregion
    }


}
