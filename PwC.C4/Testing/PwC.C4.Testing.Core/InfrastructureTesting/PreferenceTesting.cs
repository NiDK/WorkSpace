using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Infrastructure.Cache;

namespace PwC.C4.Testing.Core.InfrastructureTesting
{
    [TestClass]
    public class PreferenceTesting
    {
        [TestMethod]
        public void TestGetInst()
        {
            //var stst = AppDomain.CurrentDomain.BaseDirectory;
            //Preference.Set("Testings", "lala");
            var inof = Preference.Get("Testings");
        }
    }
}
