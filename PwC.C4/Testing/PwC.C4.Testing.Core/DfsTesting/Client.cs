using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Testing.Core.Dfs
{

    [TestClass]
    public class Client
    {

        [TestMethod]
        public void TestMethod1()
        {
            long totalCount;
            var index = 30;
            var data = C4.Dfs.Client.Dfs.GetDfsRecords(20, 10, "", out totalCount);
        }
        [TestMethod]
        public void TestMethod2()
        {

            //var conn = "mongodb://dfsuser:pass1234@hksqluwv645:27017/PwCC4Dfs";
            //var client = new MongoClient(conn);
            //var dbName = MongoUrl.Create(conn).DatabaseName;
            //var server = client.GetServer();
            //var db = server.GetDatabase(dbName);

            //var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
            //var appCode = "InspireC4";
            //var fileId = "cd32078354b14be4af0d1fd021c7f196";
            //var fileSize = "s";
            //var checkQ = new List<IMongoQuery>
            //    {
            //        Query.EQ("AppCode", appCode),
            //        Query.EQ("_id", fileId)
            //    };

            //var cu = coll.Find(Query.And(checkQ));
            //cu.SetFields("IsConverted", "DfsPath");
            //var checkConverted = cu.FirstOrDefault();
            //if (checkConverted != null)
            //{
            //    object isC;
            //    object fileDfsPath = "";
            //    if (checkConverted.TryGetValue("IsConverted", out isC))
            //    {
            //        if ((bool)isC)
            //        {
            //            var query = new List<IMongoQuery>
            //                {
            //                    Query.EQ("AppCode", appCode),
            //                    Query.EQ("_id", fileId),
            //                    Query.EQ("ConvertionInfo.ConvertMode", fileSize)
            //                };

            //            var q = Query.And(query);
            //            var collson = db.GetCollection("DfsFiles");
            //            var myCursor = collson.Find(q);
            //            var ret = myCursor.FirstOrDefault();
            //            if (ret != null)
            //            {
            //                var converList = ret.GetValue("ConvertionInfo");
            //                var arr = converList.AsBsonArray;
            //                foreach (BsonValue t in arr)
            //                {
            //                    var model = t.ToBsonDocument();
            //                    var size = model.GetValue("ConvertMode").ToString();
            //                    if (size == fileSize)
            //                    {
            //                        var dd = model.GetValue("ConvertDfsPath");
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                return "DataError";
            //            }

            //        }
            //        else
            //        {
            //            checkConverted.TryGetValue("DfsPath", out fileDfsPath);
            //        }
            //    }
            //    else
            //    {
            //        fileDfsPath = "DataError";
            //    }
            //}

            var data = PwC.C4.Dfs.Client.Dfs.Get("dfs://Image/InspireC4/cd32078354b14be4af0d1fd021c7f196.jpg", "s");

        }
    }
}
