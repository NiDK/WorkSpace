using System.Collections;
using System.Collections.Generic;

namespace PwC.C4.Testing.Labs.Web.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ArrayTesting" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ArrayTesting.svc or ArrayTesting.svc.cs at the Solution Explorer and start debugging.
    public class ArrayTesting : IArrayTesting
    {
        public void DoWork()
        {
        }

        public int ConnectTesting()
        {
            return 1231901312;
        }

        public Dictionary<string, object> ObjectArrayTransfer()
        {
            var d = new Dictionary<string,object>();
            var array = new ArrayList() { "A","B","C" };
            d.Add("Array", array);
            return d;
        }
    }
}
