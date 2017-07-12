using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Nest;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Const;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.Converter;
using PwC.C4.Metadata.Storage;
using PwC.C4.Metadata.Storage.MongoDB;

namespace PwC.C4.Testing.Metadata
{
    [TestClass]
    public class DriverTest
    {
        [TestMethod]
        public void ConnectTest()
        {
            var node = new Uri("http://cnpekappdwv033:9200");

            var settings = new ConnectionSettings(
                node,
                defaultIndex: "compliance"
                );

            var client = new ElasticClient(settings);
            var searchRequest = new SearchRequest
            {
                From = 0,
                Size = 50
            };
            var rest = client.Search<object>(searchRequest);
        }

        static readonly LogWrapper Log = new LogWrapper();

        private static Dictionary<string, MongoDatabase> databases;

        internal static MongoDatabase GetDatabase()
        {
            var conn = AppSettings.Instance.GetConntectStringV2("dbconn.C4C4BaseMongoDb");
            if (databases == null)
            {
                databases = new Dictionary<string, MongoDatabase>();
            }
            if (databases.ContainsKey(conn))
            {
                return databases[conn];
            }
            else
            {

                var client = new MongoClient(conn);
                var dbName = MongoUrl.Create(conn).DatabaseName;
                var server = client.GetServer();
                var db = server.GetDatabase(dbName);
                databases.Add(conn, db);
                return db;
            }
        }


        [TestMethod]
        public void CreateTestData()
        {
            var db = GetDatabase();
           var coll = db.GetCollection("TestGroupBy");
            for (int n = 10000; n < 20000; n++)
            {
                var group = "Meizu";
                if (n > 11000 && n < 12000)
                {
                    group = "Apple";
                }
                else if (n > 12000 && n < 13000)
                {
                    group = "Letv";
                }
                else if (n > 13000 && n < 14000)
                {
                    group = "OnePuls";
                }
                else if (n > 14000 && n < 15000)
                {
                    group = "Huawei";
                }
                else if (n > 15000 && n < 16000)
                {
                    group = "Xiaomi";
                }
                else if (n > 16000 && n < 17000)
                {
                    group = "Lianxiang";
                }
                else if (n > 17000 && n < 18000)
                {
                    group = "Kupai";
                }
                else if (n > 18000 && n < 19000)
                {
                    group = "Nuojiya";
                }
                else
                {
                    group = "Motuoluola";
                }
                var dic = new Dictionary<string, object>();
                dic.Add("Name", n);
                dic.Add("Id", Guid.NewGuid());
                dic.Add("CreateTime", DateTime.UtcNow.AddHours(n));
                dic.Add("Group", group);
                coll.Insert(dic);
            }

        }

        [TestMethod]
        public void TestGroup()
        {
            GroupTest("TestGroupBy", new List<string>() {"Group"},
                new Dictionary<string, OrderMethod>() {{"Group", OrderMethod.Descending}}, 0, 5,
                new List<SearchItem>()
                {
                    new SearchItem()
                    {
                        Method = SearchItemMethod.And,
                        Name = "Name",
                        Operator = SearchItemOperator.Intequal,
                        Value = "998"
                    }
                });
        }

       
        public void GroupTest(string entityName,List<string> groupBy,Dictionary<string,OrderMethod> sort,int index,int pageSize,List<SearchItem> searchItem)
        {
            try
            {
                var db = GetDatabase();
                var coll = db.GetCollection(entityName);
                var groupItem = new BsonDocument();
                var projectItem = new BsonDocument();
                projectItem.Set("_id", 0);
                groupBy.ForEach(g =>
                {
                    groupItem.Set(g, "$" + g);
                    projectItem.Set(g, "$_id." + g);

                });
                projectItem.Set("Count", 1);

                var q = searchItem.ToMongoQuery(entityName);
                var s = new BsonDocument();
                foreach (var keyValuePair in sort)
                {
                    s.Set(keyValuePair.Key, keyValuePair.Value == OrderMethod.Ascending ? -1 : 1);
                }
                var group = new BsonDocument
            {
                {
                    "$group",
                    new BsonDocument
                    {
                        {
                            "_id",groupItem
                        },
                        {
                            "Count", new BsonDocument
                            {
                                {
                                    "$sum", 1
                                }
                            }
                        }
                    }
                }
            };
                var match = new BsonDocument
                {
                    {
                        "$match",q.ToBsonDocument()
                    }
                };
                var project = new BsonDocument
                {
                    {
                        "$project",projectItem
                    }
                };
                var sorst = new BsonDocument()
            {
                { "$sort",s}
            };
                var skip = new BsonDocument()
            {
                {"$skip" ,index}
            };
                var limit = new BsonDocument()
            {
                {"$limit",pageSize }
            };
                var pipeline = new[] { match, group, project, sorst, skip, limit };
                var result = coll.Aggregate(new AggregateArgs() { Pipeline = pipeline });
                
                var value = new List<Dictionary<string, object>>();
                foreach (var bsonDocument in result)
                {
                    var dic = bsonDocument.ToDic();
                    value.Add(dic);
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        [TestMethod]
        public void TestGroupData()
        {
            long totalcount = 0;
            var p = ProviderFactory.GetProvider<IEntityService>("dbconn.USIT", "Form_frmFPR");
            var data = p.GetDataByGroup(new List<string>() { "FuncArea" }, out totalcount, null, new Dictionary<string, OrderMethod>() { { "FuncArea", OrderMethod.Descending } });
        }

        [TestMethod]
        public void CreateMongoSearchData()
        {
            var menu1 = Guid.NewGuid().ToString();
            var menu2 = Guid.NewGuid().ToString();
            var menu3 = Guid.NewGuid().ToString();
            var db = GetDatabase();
            var coll = db.GetCollection("TestMongoSearch");
            var datas = new List<BsonDocument>();
            var data1 = new Dictionary<string, object>()
            {
                {"Menu1",menu1},
                {"Menu2", menu2},
                {"Menu3", ""},
                {"Status", "Relese"}
            };
            datas.Add(data1.ToBsonDocument());
            var data2 = new Dictionary<string, object>()
            {
                {"Menu1",menu1},
                {"Menu2", ""},
                {"Menu3", menu3},
                {"Status", "Relese"}
            };
            datas.Add(data2.ToBsonDocument());
            var data3 = new Dictionary<string, object>()
            {
                {"Menu1",""},
                {"Menu2", menu2},
                {"Menu3", ""},
                {"Status", "Relese"}
            };
            datas.Add(data3.ToBsonDocument());
            var data4 = new Dictionary<string, object>()
            {
                {"Menu1",""},
                {"Menu2", menu2},
                {"Menu3", menu3},
                {"Status", "Relese"}
            };
            datas.Add(data4.ToBsonDocument());
            var data5 = new Dictionary<string, object>()
            {
                {"Menu1",""},
                {"Menu2", ""},
                {"Menu3", menu3},
                {"Status", "Relese"}
            };
            datas.Add(data5.ToBsonDocument());
            var data6 = new Dictionary<string, object>()
            {
                {"Menu1",menu1},
                {"Menu2", ""},
                {"Menu3", ""},
                {"Status", "Relese"}
            };
            datas.Add(data6.ToBsonDocument());
            coll.InsertBatch(datas);
        }
    }

}
