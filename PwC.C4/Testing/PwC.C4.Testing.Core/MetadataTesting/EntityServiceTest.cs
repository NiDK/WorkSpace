using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Service;
using PwC.C4.Metadata.Storage;
using PwC.C4.Metadata.Storage.MongoDb.Persistance;
using PwC.C4.TemplateEngine.Extensions;
using PwC.C4.TemplateEngine.Model.Emnu;
using PwC.C4.Testing.Core.Model;

namespace PwC.C4.Testing.Core.MetadataTesting
{
    [TestClass]
    public class EntityServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var c = "dbconn.C4C4BaseMongoDb";
            var e = "DataTimeTest";
            var dic = new Dictionary<string,string>();
            dic.Add("LocalTime",DateTime.Now.ToLocalTime().ToString());
            dic.Add("Now", DateTime.Now.ToString());
            dic.Add("UtcNow", DateTime.UtcNow.ToString());
            dic.Add("ToString", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var p = ProviderFactory.GetProvider<IEntityService>(c, e);
            string id;
            var s = p.SaveEntity<DynamicMetadata>(Guid.NewGuid().ToString(), "CurrentStaff", dic, out id);
        }

        public class ColumnData
        {
            public string ColValue { get; set; }
            public string ColType { get; set; }

        }
        [TestMethod]
        public void TestMethod2()
        {
            var js =
                "[{\"NameCode\":{\"ColValue\":\"a634bcec-591d-498c-8e66-d10c9ada3511\",\"ColType\":\"string\"},\"Name\":{\"ColValue\":\"周润发\",\"ColType\":\"string\"},\"Subject\":{\"ColValue\":\"语文\",\"ColType\":\"string\"},\"Age\":{\"ColValue\":\"12\",\"ColType\":\"double\"},\"Average\":{\"ColValue\":\"90.5\",\"ColType\":\"double\"},\"ExamDate\":{\"ColValue\":\"30/3/2016\",\"ColType\":\"datetime\"},\"CreateTime\":{\"ColValue\":\"20/10/2016 10:12\",\"ColType\":\"datetime\"},\"IsMan\":{\"ColValue\":\"True\",\"ColType\":\"bool\"}},{\"NameCode\":{\"ColValue\":\"a634bcec-591d-498c-8e66-d10c9ada3512\",\"ColType\":\"string\"},\"Name\":{\"ColValue\":\"刘德华\",\"ColType\":\"string\"},\"Subject\":{\"ColValue\":\"数学\",\"ColType\":\"string\"},\"Age\":{\"ColValue\":\"11\",\"ColType\":\"double\"},\"Average\":{\"ColValue\":\"89.6\",\"ColType\":\"double\"},\"ExamDate\":{\"ColValue\":\"14/11/2016\",\"ColType\":\"datetime\"},\"CreateTime\":{\"ColValue\":\"9/3/2016 11:12\",\"ColType\":\"datetime\"},\"IsMan\":{\"ColValue\":\"True\",\"ColType\":\"bool\"}},{\"NameCode\":{\"ColValue\":\"a634bcec-591d-498c-8e66-d10c9ada3513\",\"ColType\":\"string\"},\"Name\":{\"ColValue\":\"刘亦菲\",\"ColType\":\"string\"},\"Subject\":{\"ColValue\":\"英语\",\"ColType\":\"string\"},\"Age\":{\"ColValue\":\"10\",\"ColType\":\"double\"},\"Average\":{\"ColValue\":\"75.6\",\"ColType\":\"double\"},\"ExamDate\":{\"ColValue\":\"14/10/2016\",\"ColType\":\"datetime\"},\"CreateTime\":{\"ColValue\":\"12/10/2016 11:12\",\"ColType\":\"datetime\"},\"IsMan\":{\"ColValue\":\"False\",\"ColType\":\"bool\"}},{\"NameCode\":{\"ColValue\":\"a634bcec-591d-498c-8e66-d10c9ada3514\",\"ColType\":\"string\"},\"Name\":{\"ColValue\":\"黄晓明\",\"ColType\":\"string\"},\"Subject\":{\"ColValue\":\"体育\",\"ColType\":\"string\"},\"Age\":{\"ColValue\":\"20\",\"ColType\":\"double\"},\"Average\":{\"ColValue\":\"88.8\",\"ColType\":\"double\"},\"ExamDate\":{\"ColValue\":\"12/4/2016\",\"ColType\":\"datetime\"},\"CreateTime\":{\"ColValue\":\"10/11/2016 12:23\",\"ColType\":\"datetime\"},\"IsMan\":{\"ColValue\":\"True\",\"ColType\":\"bool\"}},{\"NameCode\":{\"ColValue\":\"a634bcec-591d-498c-8e66-d10c9ada3515\",\"ColType\":\"string\"},\"Name\":{\"ColValue\":\"邓超\",\"ColType\":\"string\"},\"Subject\":{\"ColValue\":\"计算机\",\"ColType\":\"string\"},\"Age\":{\"ColValue\":\"21\",\"ColType\":\"double\"},\"Average\":{\"ColValue\":\"46.56\",\"ColType\":\"double\"},\"ExamDate\":{\"ColValue\":\"22/12/2016\",\"ColType\":\"datetime\"},\"CreateTime\":{\"ColValue\":\"25/9/2016 20:12\",\"ColType\":\"datetime\"},\"IsMan\":{\"ColValue\":\"True\",\"ColType\":\"bool\"}}]";
            var list = JsonHelper.Deserialize<List<Dictionary<string, ColumnData>>>(js);
            foreach (var item in list)
            {
                Dictionary<string, string> dic = item.ToDictionary(item1 => item1.Key, item1 => item1.Value.ColValue);
                var c = "dbconn.ExcelSql";
                var e = "PersonReport";
                var p = PwC.C4.Metadata.Storage.ProviderFactory.GetProvider<IEntityService>(c, e);
                string id;
                var s = p.SaveEntity<DynamicMetadata>(dic["NameCode"],"NoStaff", dic, out id);
            }



        }

        [TestMethod]
        public void TestMethod3()
        {
            var data = new Dictionary<string,string>();
            data.Add("NameCode", "a634bcec-591d-498c-8e66-d10c9ada3511");
            data.Add("Name", "周润");
            data.Add("Subject",null);
            data.Add("Age",null);
            data.Add("Average", null);
            data.Add("ExamDate", null);
            data.Add("CreateTime", null);
            data.Add("IsMan", null);
            var c = "dbconn.ExcelSql";
            var e = "PersonReport";
            var p = PwC.C4.Metadata.Storage.ProviderFactory.GetProvider<IEntityService>(c, e);
            string id;
            var s = p.SaveEntity<DynamicMetadata>(data["NameCode"], "NoStaff", data, out id);
        }

        [TestMethod]
        public void TestCreateQ()
        {
            var se =
                JsonHelper.Deserialize<List<SearchItem>>(
                    "[{\"Name\":\"IsDeleted\",\"Value\":\"0\",\"Operator\":\"Intequal\",\"Method\":\"AND\"},{\"Method\":\"AND\",\"SubSearchItems\":[{\"Name\":\"CreateBy\",\"Value\":\"12578\",\"Operator\":\"Intequal\",\"Method\":\"OR\"},{\"Name\":\"A1\",\"Value\":\"12578\",\"Operator\":\"Intequal\",\"Method\":\"OR\"}]}]");
            //var q = se.ToQuery("Entity_AppCode");

        }

        [TestMethod]
        public void TestSearch()
        {
            var c = "dbconn.GiftApproval";
            var e = "Entity_AppCode";
            var p = PwC.C4.Metadata.Storage.ProviderFactory.GetProvider<IEntityService>(c, e);
            var se =
              JsonHelper.Deserialize<List<SearchItem>>(
                  "[{\"Name\":\"IsDeleted\",\"Value\":\"0\",\"Operator\":\"Intequal\",\"Method\":\"AND\"},{\"Method\":\"AND\",\"SubSearchItems\":[{\"Name\":\"CreateBy\",\"Value\":\"12578\",\"Operator\":\"like\",\"Method\":\"OR\"},{\"Name\":\"A1\",\"Value\":\"12578\",\"Operator\":\"like\",\"Method\":\"OR\"}]}]");
            long to;
            var data = p.GetEntitesTranslated<DynamicMetadata>(se,
                new Dictionary<string, OrderMethod>() {{"CreateBy", OrderMethod.Ascending}},
                new List<string>() {"CreateBy", "SubmissionDate" }, 0, 100, null, null, null, out to);
        }

        [TestMethod]
        public void TestGenerateHtml()
        {
            //var c = "dbconn.GiftApproval";
            //var e = "Entity_AppCode";
            //var p = PwC.C4.Metadata.Storage.ProviderFactory.GetProvider<IEntityService>(c, e);
            //var data = p.GetEntityTranslated<DynamicMetadata>("a692bcd2-5bda-48c4-95d9-40119dfeb0c9");
            //var entity = HtmlExtend.GetMetadataEntityControl<DynamicMetadata>(PageMode.Preview,
            //    "a692bcd2-5bda-48c4-95d9-40119dfeb0c9", data, false, e);
            //var da = entity.GenerateHtml("A4", "form-controlRadio", null, PageMode.Preview);
            try
            {

                //var credential = MongoCredential.CreateMongoCRCredential("Compliance", "com", "com");

                //var settings = new MongoClientSettings
                //{
                //    Credentials = new[] { credential },
                //    ConnectionMode = ConnectionMode.Automatic,
                //    ConnectTimeout = new TimeSpan(0, 0, 0, 10),
                //    UseSsl = false,
                //    Server = new MongoServerAddress("CNPEKSQLDWV016", 27317)

                //};



                //var mongoClient = new MongoClient(settings);
                var mongoClient = new MongoClient("mongodb://demo:demo@CNPEKSQLDWV016:27317");
                var db = mongoClient.GetServer().GetDatabase("Compliance");
                var data = db.GetCollection<Dictionary<string, object>>("Form_Other_DB");
                var myCursor = data.Find(new QueryDocument("_id", "576d0453e3d61b6ec0eb52cc"));
                var entity = myCursor.First();
            }
            catch (Exception ee)
            {
                
                throw ee;
            }

        }
        [TestMethod]
        public void TestDatasource()
        {
           
            var metadataservice = new PwC.C4.Metadata.Service.DataSourceService();
            var list = metadataservice.GetDataSourceBy(DataSourceType.Mapping, "Category");
        }

        [TestMethod]
        public void TestCreateOrUpdateType()
        {
            var type = new DataSourceTypeInfo()
            {
                //当ID在数据库中存在时且State为0则更新，反之则添加。
                Id = Guid.Parse("CEB684F1-F144-43F1-A188-093894F0EBAC"),
                CreateBy = "Chenhui yu Tesing",
                Type = "Simple",
                Name = "TestType04",
                Desc = "Testing Desc",
                Order = 0,
                State = 0
            };
            var data = DataSourceService.Instance().UpdateDataSourceType(type);
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public void TestGetDatasourceDetail()
        {
            int totalCount = 0;
            var data = DataSourceService.Instance()
                .GetDataSourceDetailsByPaging("FuncSubArea", out totalCount, "usit", 0, -1, "Finance Strategy and Business Ops");
            //var data1 = DataSourceService.Instance()
            //    .GetDataSourceDetailsByPaging("KeyWord", out totalCount, "AdvRQPortal");
        }


        [TestMethod]
        public void TestCreateOrUpdateDetail()
        {
            var oldData = DataSourceService.Instance().GetDataSourceBy(DataSourceType.Mapping, "TestType04", "1");
            var type = new DataSourceDetail()
            {
                //当ID在数据库中存在时且State为0则更新，反之则添加。
                //Id = Guid.Parse("9DBA38F3-7B74-4D69-A397-C4689C1434D2"),
                CreateBy = "Chenhui yu Tesing",
                Key = "Test123",
                Value = "Test333",
                Group = "1",
                //DataSourceTypeId 与 DataSourceTypeName 任选其一
                DataSourceTypeId = Guid.Parse("CEB684F1-F144-43F1-A188-093894F0EBAC"),
                //DataSourceTypeName = "TestType04",
                Order = 0,
                State = 0
            };
            var data = DataSourceService.Instance().UpdateDateSourceDetail(type);

            var newData = DataSourceService.Instance().GetDataSourceBy(DataSourceType.Mapping, "TestType04", "1");
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public void TestDeleteDataSource()
        {
            var data = DataSourceService.Instance()
                .DeleteDataSource(DataSource.Type, new Guid("CEB684F1-F144-43F1-A188-093894F0EBAC"),
                    "Chenhui yu Deleteing");
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public void TestGetDataSource()
        {
            var data = DataSourceService.Instance()
                .GetDataSourceTypeList();
        }
    }
}
