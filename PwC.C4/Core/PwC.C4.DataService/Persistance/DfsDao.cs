using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using static MongoDB.Bson.BsonRegularExpression;

namespace PwC.C4.DataService.Persistance
{
    internal static class DfsDao
    {
        static readonly LogWrapper Log = new LogWrapper();

        private static Dictionary<string, MongoDatabase> _databases;

        internal static MongoDatabase GetDatabase()
        {

            var databaseName = DatabaseInstance.C4Dfs;
            if (_databases == null)
            {
                _databases = new Dictionary<string, MongoDatabase>();
            }
            if (_databases.ContainsKey(databaseName))
            {
                return _databases[databaseName];
            }
            else
            {
                var conn = ConnectionStringProvider.GetConnectionString(databaseName);
                var client = new MongoClient(conn);
                var dbName = MongoUrl.Create(conn).DatabaseName;
                var server = client.GetServer();
                var db = server.GetDatabase(dbName);
                if (_databases.ContainsKey(databaseName))
                {
                    _databases[databaseName] = db;

                }
                else
                {
                    _databases.Add(databaseName, db);
                }

                return db;
            }

        }

        public static Dictionary<string, object> RetrieveRecord(string appCode, string fileId, IList<string> properties)
        {
            try
            {
                var db = GetDatabase();
                var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
                var myCursor = coll.Find(new QueryDocument("_id", fileId));
                if (properties != null && properties.Any())
                {
                    myCursor.SetFields(properties.ToArray());
                }
                myCursor.SetFields("_id", "AppCode", "Name", "Encoding", "Length", "Keyspace", "Metadata", "Status",
                    "StartTime", "DfsPath", "FinishTime", "IsConverted");
                var entity = myCursor.First();
                return entity;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"get RetrieveRecord from mongodb error,Collection name:{appCode},fileId:{fileId},columns:{JsonHelper.Serialize(properties)}";
                Log.Error(errorMessage, ee);
                return new Dictionary<string, object>();
            }

        }

        public static List<Dictionary<string, object>> GetDataRecords(string appCode, int pageIndex, int pageSize,
            string keyword, out long totalCount, string keyspace = null, string staffId = null)
        {
            try
            {
                var db = GetDatabase();
                var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
                var query = new List<IMongoQuery>();
                if (!string.IsNullOrEmpty(staffId))
                {
                    query.Add(Query.EQ("Uploader", staffId));
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    query.Add(Query.Matches("Name", new BsonRegularExpression(new Regex("^"+keyword+"$"))));
                }
                if (!string.IsNullOrEmpty(keyspace))
                {
                    query.Add(Query.EQ("Keyspace", keyspace));
                }
                query.Add(Query.EQ("Status", "Done"));
                query.Add(Query.EQ("IsDeleted", false));
                var builder = new SortByBuilder();
                var q = Query.And(query);
                var myCursor = coll.Find(q);
                builder.Descending("FinishTime");
                myCursor.SetFields("_id", "AppCode", "Name", "Encoding", "Length", "Keyspace", "Metadata", "Status",
                    "StartTime", "DfsPath", "FinishTime", "IsConverted");
                myCursor.SetSortOrder(builder);

                if (pageSize != -1)
                {
                    myCursor.SetLimit(pageSize);
                }
                var result = myCursor.SetSkip(pageIndex).ToList();

                totalCount = myCursor.Count();

                return result;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"get RetrieveRecord from mongodb error,Collection name:{appCode},pageIndex:{pageIndex},pageSize:{pageSize},keyword:{keyword},staffId:{staffId}";
                Log.Error(errorMessage, ee);
                totalCount = 0;
                return new List<Dictionary<string, object>>();
            }

        }

        public static string GetDfsPathById(string appCode, string fileId)
        {
            var db = GetDatabase();
            var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
            var q = Query.And(Query.EQ("_id", fileId));
            var myCursor = coll.Find(q);
            myCursor.SetFields("_id", "AppCode", "Name", "Encoding", "Length", "Keyspace", "Metadata", "Status",
                  "StartTime", "DfsPath", "FinishTime", "IsConverted");
            var dic = myCursor.FirstOrDefault();
            if (dic != null && dic.ContainsKey("DfsPath"))
            {
                return dic["DfsPath"].ToString();
            }
            return "";
        }

        public static bool StartInsert(string appCode, string fileId, string uploader,
            Dictionary<string, object> properties)
        {
            try
            {
                properties.Set("_id", fileId);
                properties.Set("IsDeleted", false);
                properties.Set("IsConverted", false);
                properties.Set("Status", "Pending");
                properties.Set("Uploader", uploader);
                properties.Set("StartTime", DateTime.Now);
                var db = GetDatabase();
                var docData = new BsonDocument(properties);
                var coll = db.GetCollection("DfsFiles");
                var rest = coll.Insert(docData);
                return rest.Ok;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"InsertRecord to mongodb error,table name:{appCode},datas:{JsonHelper.Serialize(properties)}";
                Log.Error(errorMessage, ee);
                return false;
            }
        }

        public static void FinishInsert(string appCode, string fileId, string dfsPath)
        {
            try
            {
                var db = GetDatabase();
                var coll = db.GetCollection("DfsFiles");
                var finishDic = new Dictionary<string, object>()
                {
                    {"Status", "Done"},
                    {"DfsPath", dfsPath},
                    {"FinishTime", DateTime.Now}
                };
                coll.Update(new QueryDocument("_id", fileId),
                    new UpdateDocument("$set", finishDic.ToBsonDocument()));
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"FinishInsert from mongodb error,Collection name:{appCode},fileId:{fileId}";
                Log.Error(errorMessage, ee);
            }
        }

        public static void RemoveRecord(string appCode, string fileId)
        {
            try
            {
                var db = GetDatabase();
                var coll = db.GetCollection("DfsFiles");
                var deleteDic = new Dictionary<string, bool>() {{"IsDeleted", true}};
                coll.Update(new QueryDocument("_id", fileId),
                    new UpdateDocument("$set", deleteDic.ToBsonDocument()));
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"RemoveRecord from mongodb error,Collection name:{appCode},fileId:{fileId}";
                Log.Error(errorMessage, ee);
            }
        }

        public static string GetDfsPathBySize(string appCode, string fileId, string fileSize)
        {
            try
            {
                var checkQ = new List<IMongoQuery>
                {
                    Query.EQ("AppCode", appCode),
                    Query.EQ("_id", fileId)
                };
                var db = GetDatabase();
                var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
                var cu = coll.Find(Query.And(checkQ));
                cu.SetFields("IsConverted", "DfsPath");
                var checkConverted = cu.FirstOrDefault();
                if (checkConverted != null)
                {
                    object isC;
                    object fileDfsPath = "";
                    if (checkConverted.TryGetValue("IsConverted", out isC))
                    {
                        if ((bool)isC)
                        {
                            var query = new List<IMongoQuery>
                            {
                                Query.EQ("AppCode", appCode),
                                Query.EQ("_id", fileId),
                                Query.EQ("ConvertionInfo.ConvertMode", fileSize)
                            };

                            var q = Query.And(query);
                            var collson = db.GetCollection("DfsFiles");
                            var myCursor = collson.Find(q);
                            var ret = myCursor.FirstOrDefault();
                            if (ret != null)
                            {
                                var converList = ret.GetValue("ConvertionInfo");
                                var arr = converList.AsBsonArray;
                                foreach (BsonValue t in arr)
                                {
                                    var model = t.ToBsonDocument();
                                    var size = model.GetValue("ConvertMode");
                                    if (size == fileSize)
                                    {
                                        return model.GetValue("ConvertDfsPath").ToString();
                                    }
                                }
                            }
                        }
                    }
                    checkConverted.TryGetValue("DfsPath", out fileDfsPath);
                    if (fileDfsPath != null) return fileDfsPath.ToString();
                }
                return "DataError";
            }
            catch (Exception ee)
            {
                Log.Error("GetDfsPathBySize error!appcode:" + appCode + ",fileId:" + fileId + ",fileSize:" + fileSize,
                    ee);
                return "DataError";
            }
        }
    }
}
