using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.Converter;

namespace PwC.C4.Metadata.Search.BaseQuery
{
    public static class MongoDbQuery
    {
        static PwC.C4.Infrastructure.Logger.LogWrapper _log = new LogWrapper();
        private static Dictionary<string, MongoDatabase> databases;
        internal static MongoDatabase GetDatabase(string databaseName)
        {
            var conn = AppSettings.Instance.GetConntectStringV2(databaseName);
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

        internal static List<string> GetDataIds(string connName, string entity, string keyColumn,
            IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, int from,
            int size, out long totalCount)
        {
            var db = GetDatabase(connName);
            var coll = db.GetCollection<BsonDocument>(entity);
            MongoCursor<BsonDocument> myCursor;
            if (searchItems != null && searchItems.Count > 0)
            {
                var where = searchItems.ToMongoQuery(entity);
                if (_log.IsDebugEnabled)
                {
                    _log.Error("MongoDb query string:" + where.ToString());
                }
                myCursor = coll.Find(where);
            }
            else
            {
                myCursor = coll.FindAll();
            }
            myCursor.SetSortOrder(orders.ToMongoSort());

            myCursor.SetFields(keyColumn);
            if (size != -1)
            {
                myCursor.SetLimit(size);
            }
            var result = myCursor.SetSkip(from).ToList();

            totalCount = myCursor.Count();
            var idList = new List<string>();
            if (result.Any())
            {
                result.ForEach(d =>
                {
                    idList.Add(d.GetValue(keyColumn).ToString()); 
                });
            }
            return idList;
        }
    }
}

