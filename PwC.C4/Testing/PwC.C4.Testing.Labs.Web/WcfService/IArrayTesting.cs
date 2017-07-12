using System.Collections.Generic;
using System.ServiceModel;
using PwC.C4.Infrastructure.Annotations;

namespace PwC.C4.Testing.Labs.Web.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IArrayTesting" in both code and config file together.
    [ServiceContract]
    public interface IArrayTesting
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        int ConnectTesting();
        [OperationContract]
        Dictionary<string, object> ObjectArrayTransfer();
    }
}
