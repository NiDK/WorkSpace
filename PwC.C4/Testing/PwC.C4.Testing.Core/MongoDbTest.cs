using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Storage;
using PwC.C4.Metadata.Storage.MongoDb.Service;

namespace PwC.C4.UnitTest
{
    [TestClass]
    public class MongoDbTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var pro = ProviderFactory.GetProvider<IAttachmentService>();
            var result = pro.SaveEntityAttachment<TestModel>("info", ".txt", "UserId", new FileStream(@"C:\Info.txt", FileMode.Open));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var pro = ProviderFactory.GetProvider<IAttachmentService>();
            var result =
                pro
                    .GetEntityAttachments<TestModel>( 
                        new List<Guid>() {new Guid("d79d69f9-22f8-42e3-9700-2be56840b9f3")}, true);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var pro = ProviderFactory.GetProvider<IEntityService>();
            var gid = Guid.NewGuid();
            var dic = new Dictionary<string, string> { { "Status", "1" }, { "RecordId", gid.ToString() } };
            var result = pro.SaveEntity<TestModel>(gid, "a的中文兼容性", dic);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var pro = ProviderFactory.GetProvider<IEntityService>();
            var gid = new Guid("D87C4637-E453-41FB-B42F-98B6F38148EF");
            var result = pro.GetEntity<TestModel>(gid, new List<string>() { "Status" });
        }

        [TestMethod]
        public void TestMethod5()
        {
            var pro = ProviderFactory.GetProvider<IEntityService>();
            //var gid = new Guid("1fe93ffa-8df6-47c3-b693-19fc824cef33");
            var searchItem = new List<SearchItem>();
            long total = 0;
            List<string> deCol;
            //searchItem.Add(new SearchItem()
            //{
            //    Method = Metadata.Model.Const.SearchItemMethod.And,
            //    Name = "FormId",
            //    Operator = PwC.C4.Metadata.Model.Const.SearchItemOperator.Intequal,
            //    Value = "12"
            //});
            searchItem.Add(new SearchItem()
            {
                Method = Metadata.Model.Const.SearchItemMethod.And,
                Name = "FormId",
                Operator = PwC.C4.Metadata.Model.Const.SearchItemOperator.Intequal,
                Value = "1"
            });
            var dic = new Dictionary<string, OrderMethod> {{"ModifyBy", OrderMethod.Ascending}};

            var result = pro
                .GetEntites<TestModel>(searchItem, dic, null, 0, 99,
                    out total, out deCol);
        }

    }
}
