using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Storage;
using PwC.C4.Testing.Core.Model;

namespace PwC.C4.Testing.Core.MetadataTesting
{
    [TestClass]
    public class MetadataSettingsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = MetadataSettings.Instance.GetEntity("ArrayTesting");
            var stes = MetadataSettings.Instance.GetArrayColumns("ArrayTesting");
        }
    }
}
