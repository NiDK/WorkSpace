using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PwC.C4.Configuration.Data;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Converter.Model;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Converter.Persistance
{
    public static class BaseDao
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


        public static bool IsNew(List<string> appCodes)
        {
            var list = new List<BsonValue>();
            appCodes.ForEach(c =>
            {
                list.Add(c);
            });
            var db = GetDatabase();
            var coll = db.GetCollection<Dictionary<string, object>>("DfsFiles");
            var query = new List<IMongoQuery>
            {
                Query.EQ("Status", "Done"),
                Query.EQ("IsConverted", false),
                Query.EQ("IsDeleted", false),
                Query.In("AppCode", list),
                Query.In("Keyspace", new List<BsonValue>() {"Image", "Video"})
            };
            var q = Query.And(query);
            var myCursor = coll.Find(q);
            myCursor.SetFields("_id");
            myCursor.SetLimit(1);
            var data = myCursor.FirstOrDefault();
            return data != null;
        }

        public static List<string> GetUnconvetionFiles(List<string> appCodes,int limit)
        {
            var list = new List<BsonValue>();
            appCodes.ForEach(c =>
            {
                list.Add(c);
            });
            var db = GetDatabase();
            var coll = db.GetCollection("DfsFiles");
            var query = new List<IMongoQuery>
            {
                Query.EQ("Status", "Done"),
                Query.EQ("IsConverted", false),
                Query.EQ("IsDeleted", false),
                Query.In("AppCode", list),
                Query.In("Keyspace", new List<BsonValue>() {"Image", "Video"})
            };
            var builder = new SortByBuilder();
            var q = Query.And(query);
            var myCursor = coll.Find(q);
            builder.Ascending("StartTime");
            myCursor.SetFields("DfsPath");
            myCursor.SetSortOrder(builder);
            myCursor.SetLimit(limit);
            var result = new List<string>();
            var data = myCursor.ToList();
            data.ForEach(c =>
            {
                var dat = c.GetValue("DfsPath");
                result.Add(dat.ToString());
            });
            return result;
        }


        public static void UpdateConvertion(string fileId, ConvertionInfo convertionInfo,
            string convertResult)
        {
            var db = GetDatabase();
            var coll = db.GetCollection("DfsFiles");
            var finishDic = new Dictionary<string, object>()
            {
                {"ConvertionInfo", convertionInfo }
            };
            coll.Update(new QueryDocument("_id", fileId),
                new UpdateDocument("$addToSet", finishDic.ToBsonDocument()));
        }

        public static void CompleteConvert(string fileId)
        {
            var db = GetDatabase();
            var coll = db.GetCollection("DfsFiles");
            var finishDic = new Dictionary<string, object>()
            {
                {"IsConverted", true }
            };
            coll.Update(new QueryDocument("_id", fileId),
                new UpdateDocument("$set", finishDic.ToBsonDocument()));
        }

        public static void ReConvertAllData()
        {
            var db = GetDatabase();
            var coll= db.GetCollection("DfsFiles");
            var finishDic = new Dictionary<string, object>()
            {
                {"IsConverted", false }
            };
            coll.Update(new QueryDocument("Status", "Done"), new UpdateDocument("$set", finishDic.ToBsonDocument()), UpdateFlags.Multi);
        }
    }
}
