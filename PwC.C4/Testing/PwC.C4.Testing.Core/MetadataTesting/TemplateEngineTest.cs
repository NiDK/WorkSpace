using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PwC.C4.Testing.Core.MetadataTesting
{
    [TestClass]
    public class TemplateEngineTest
    {
        [TestMethod]
        public void HtmlRenderTest()
        {
            var data = PwC.C4.TemplateEngine.Extensions.HtmlExtend.BuildControl("Entity_PartnerConference",
                "HongKongFerrySchedule", "1",
                "", null);
        }
    }
}
