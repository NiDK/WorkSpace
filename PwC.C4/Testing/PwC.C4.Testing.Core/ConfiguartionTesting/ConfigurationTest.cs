using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Configuration.Data;

namespace PwC.C4.Testing.Core.ConfiguartionTesting
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void TestConnConfig()
        {
           var common = ConnectionStringProvider.GetConnectionString("dbconn.C4DataKeeper");
        }
    }
}
