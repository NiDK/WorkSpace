using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Testing.Core.InfrastructureTesting
{
    [TestClass]
    public class LoggingTesting
    {
        [TestMethod]
        public void LogTest()
        {
            PwC.C4.Infrastructure.Logger.LogWrapper.InitLog();
            PwC.C4.Infrastructure.Logger.LogWrapper log = new LogWrapper();
            log.Error("DoAction Error, ActionSuccessful=false or CurrentStateList.Any=false,Input Model:{\"IsInitialize\":true,\"WorkflowInstanceId\":0,\"WorkFlowCode\":\"TestSfaffHande\",\"ActionCode\":\"Submit\",\"EntityName\":\"Form_Context_Help_Form\",\"RecordId\":\"41263a5a697b44a484cb98b574574590\",\"FormId\":0,\"UserId\":\"CN112076\"};Workflow result:{\"ActionSuccessful\":true,\"CurrentStateList\":[{\"InstanceID\":215153,\"StateCode\":\"PendingApproval\",\"StateName\":\"PendingApproval\"}],\"ErrorMessage\":\"\",\"InstanceCompletedFlag\":0,\"InstanceID\":215153,\"WorkflowCode\":\"TestSfaffHande\"}");
        }
    }
}
