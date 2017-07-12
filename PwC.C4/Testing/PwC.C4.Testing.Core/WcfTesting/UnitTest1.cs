using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.DataService.Model;
using PwC.C4.Dfs.Common;

namespace PwC.C4.Testing.Core.WcfTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var it = new Invitation
            {
                SendFlag = "N",
                EmailClientType = "Notes",
                SendDate = null,
                InvitationStartTime =new DateTime(2016,3,29,14,0,0),
                InvitationEndTime = new DateTime(2016, 3, 29, 16, 0, 0),
                Subject = "text-20160328-1-NewTest02",
                Description = "20160328-2",
                Location = "20160328",
                InvitationtimeZone = 1,
                CreatedBy = "feng",
                CreatedDate = DateTime.Now,
                AppCode = "CalInvite",
                InvitationType = "TestType"
            };
            var irs = new List<InvitationRole>();
            //var ir = new InvitationRole
            //{
            //    RoleEmail = "Zhuo Wang/CN/GTS/PwC",
            //    RoleType = "Chairman",
            //    IsRequired = "Y"
            //};
            //irs.Add(ir);
            var ir = new InvitationRole
            {
                RoleEmail = "Xu Xie/CN/GTS/PwC",
                RoleType = "Chairman",
                IsRequired = "Y"
            };
            irs.Add(ir);
            ir = new InvitationRole
            {
                RoleEmail = "Chenhui Yu/CN/GTS/PwC",
                RoleType = "Participant",
                IsRequired = "Y"
            };
            irs.Add(ir);
            //InfrastructureClient client = new InfrastructureClient();
            //var stl = client.InsertInvitation(it, irs);
        }

    }
}
