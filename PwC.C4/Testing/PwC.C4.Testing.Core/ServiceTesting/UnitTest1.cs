using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.DataService.Model;
using PwC.C4.Dfs.Common;

namespace PwC.C4.Testing.Core.Service
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

           var  service = new FileRepositoryService();
            service.RepositoryDirectory = "storage";
            using (ServiceHost host = new ServiceHost(service))
            {
                var wcfSettings = WcfSettings.Instance.GetWcfSetting("C4DfsServerService");
                host.AddServiceEndpoint(typeof(IFileRepositoryService),
                    wcfSettings.Binding, wcfSettings.Endpoint.Uri);
                if (host.State != CommunicationState.Opening)
                    host.Open();
            }
        }

        //[TestMethod]
        //public void TestSendEmail()
        //{
        //    var client = new InfrastructureClient();
        //    var mail = new MailQueueModel
        //    {
        //        AppCode = "TestClient",
        //        Content = "MailContent",
        //        ImmediateFlag = "Y",
        //        MailFrom = "chenhui.yu@cn.pwc.com",
        //        MailTo = "muxi.li@cn.pwc.com",
        //        ReplyTo = "",
        //        Subject = "Test Mail",
        //        SendDate = DateTime.Now
        //    };
        //    var mail1 = new MailQueueModel
        //    {
        //        AppCode = "TestClient",
        //        Content = "MailContent1",
        //        ImmediateFlag = "Y",
        //        MailFrom = "chenhui.yu@cn.pwc.com",
        //        MailTo = "muxi.li@cn.pwc.com",
        //        ReplyTo = "",
        //        Subject = "Test Mail1",
        //        SendDate = DateTime.Now
        //    };
        //    var result = client.InsertToMailQueueBatch(new List<MailQueueModel>() {mail, mail1 });
        //}
        [TestMethod]
        public void TestWcfEnable()
        {
            var client = new ArrayTestingClient();
            var a = client.ConnectTesting();
            Assert.AreEqual(1231901312,a);
        }
        [TestMethod]
        public void TestArrayTransfer()
        {
            var c  = new ArrayTestingClient();
            var nd = c.ObjectArrayTransfer();
            var d = new Dictionary<string, object>();
            var array = new ArrayList() { "A", "B", "C" };
            d.Add("Array", array);
            Assert.AreEqual(d, nd);
        }
    }


}
