using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Common.Model;
using PwC.C4.Common.Provider;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Testing.Core
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void TestGetInst()
        {
            PwC.C4.Infrastructure.Logger.LogWrapper.InitLog();
            EmailServiceProvider.SingleEmailSend("ConfirmationMail", "chenhui.yu@cn.pwc.com", "");
        }

        [TestMethod]
        public void TestLog()
        {
            LogWrapper.InitLog();

            var log = new LogWrapper();
            log.Error("Test data");
        }

        [TestMethod]
        public void TestWorkflowInstanceInfo()
        {
            var data = PwC.C4.Common.Service.WorkflowService.Instance().GetWorkflowInstanceInfo("TestSfaffHande", new int[] {215151});
        }

        [TestMethod]
        public void TestGoAnywhereWithoutWorkflow()
        { 
            var data = PwC.C4.Common.Service.WorkflowService.Instance().GoAnywhereWithoutWorkflow(new WorkflowModel()
            {
                ActionCode = "Test",
                EntityName = "Form_Context_Help_Form",
                WorkFlowCode = "TestSfaffHande",
                RecordId = "f09af2bfebcc4cdcae99a571e5eeafb2",
                WorkflowInstanceId = 215151,
                UserId = "CN401180",
                TargetState = "Completed"
            });
        }
    }
}
