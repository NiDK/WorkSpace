using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Membership;
using PwC.C4.Membership.Config;
using PwC.C4.Membership.Model.Enum;

namespace PwC.C4.Testing.Core.MembershipTesting
{
    [TestClass]
    public class MembershipTesting
    {
        [TestMethod]
        public void TestConfig()
        {
            var m = MembershipSettings.Instance.GetAuthProviderSettings(AuthProvider.ApplicationCenter);
        }

        [TestMethod]
        public void ProviderTesting()
        {
            var m = ProviderFactory.GetProvider<IUserProvider>("Sketch");
        }
    }
}
