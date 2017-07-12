using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search;
using PwC.C4.Metadata.Search.Converter;
using PwC.C4.Metadata.Storage.MongoDB;
using PwC.C4.Metadata.Storage.MongoDb.Models;

namespace PwC.C4.Metadata.Storage.MongoDb.Persistance
{
    internal static class CommonDataDao
    {
        static readonly LogWrapper Log = new LogWrapper();

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

        public static bool CheckRecordExist(string conn,string tableName, string dataId)
        {
            return CheckRecordExist(conn, tableName, "_id", dataId);
        }

        public static bool CheckRecordExist(string conn, string tableName,string column, string dataId)
        {
            var db = GetDatabase(conn);
            var coll = db.GetCollection(tableName);
            var q = new Dictionary<string, object>() { { column, dataId } };
            var data = coll.FindOne(new QueryDocument(q));
            return data != null && data.Any();
        }

        public static int InsertMetadata(string conn, string tableName, string pkName, string dataId, string modifyUserId, Dictionary<string, object> datas, string dataBeforSaveHandlingScript = null)
        {
            try
            {
                datas.Set("_id", dataId);
                datas.Set(pkName, dataId);
                if (!datas.ContainsKey("CreateBy"))
                    datas.Set("CreateBy", modifyUserId);
                if (!datas.ContainsKey("CreateDate"))
                    datas.Set("CreateDate", DateTime.Now);
                if (!datas.ContainsKey("ModifyBy"))
                    datas.Set("ModifyBy", modifyUserId);
                if (!datas.ContainsKey("ModifyDate"))
                    datas.Set("ModifyDate", DateTime.Now);
                if (!datas.ContainsKey("IsDeleted"))
                    datas.Set("IsDeleted", 0);
                var db = GetDatabase(conn);

                var docData = new BsonDocument(datas);
                if (!string.IsNullOrEmpty(dataBeforSaveHandlingScript))
                {
                    var code = db.GetScript(dataBeforSaveHandlingScript);
                    docData = db.GetResultsAsBsonDocument(code.Code, new BsonArray() {docData});
                }

                var coll = db.GetCollection(tableName);
                var rest = coll.Insert(docData);

                var okIndex = rest.Response.IndexOfName("ok");
                if (okIndex >= 0 && rest.Response[okIndex] > 0)
                {
                    CommonService.InsertMetadataLog(AppSettings.Instance.GetAppCode(), tableName, dataId,
                        MetadataLogType.Add, JsonHelper.Serialize(datas), modifyUserId);
                    return 1;
                }
                return 0;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    string.Format("Save metadata to mongodb error,table name:{0},datas:{1}",
                        tableName, JsonHelper.Serialize(datas));
                Log.Error(errorMessage, ee);
                return (int) EntityUpdateState.SystemError;
            }

        }

        public static int UpdateMetadata(string conn, string tableName, string pkName, string dataId, string modifyUserId,
            Dictionary<string, object> datas, string dataBeforSaveHandlingScript)
        {
            try
            {
                datas.Set("_id", dataId);
                datas.Set(pkName, dataId);
                if (!datas.ContainsKey("ModifyBy"))
                    datas.Set("ModifyBy", modifyUserId);
                if (!datas.ContainsKey("ModifyDate"))
                    datas.Set("ModifyDate", DateTime.Now);
                var db = GetDatabase(conn);

                var docData = new BsonDocument(datas);
                if (!string.IsNullOrEmpty(dataBeforSaveHandlingScript))
                {
                    var code = db.GetScript(dataBeforSaveHandlingScript);
                    docData =db.GetResultsAsBsonDocument(code.Code, new BsonArray() {docData});
                }

                var coll = db.GetCollection(tableName);
                var q = new Dictionary<string,object>() { { pkName, dataId }, { "IsDeleted", 0 } };
                var rest = coll.Update(new QueryDocument(q), new UpdateDocument("$set", docData));
                var okIndex = rest.Response.IndexOfName("ok");
                if (okIndex >= 0 && rest.Response[okIndex] > 0)
                {
                    CommonService.InsertMetadataLog(AppSettings.Instance.GetAppCode(), tableName, dataId, MetadataLogType.Edit,
                       JsonHelper.Serialize(datas), modifyUserId);
                    return 1;
                }
                return 0;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    string.Format("Save metadata to mongodb error,table name:{0},PK Name:{1},DataId:{2},datas:{3}",
                        tableName, pkName, dataId, JsonHelper.Serialize(datas));
                Log.Error(errorMessage, ee);
                return (int) EntityUpdateState.SystemError;
            }

        }

        public static List<BsonDocument> GetCommonData(string conn, string tableName, IList<SearchItem> searchItems,
            Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
            out long totalCount, string dataAfterGetHandlingScript)
        {
            var db = GetDatabase(conn);
            var coll = db.GetCollection<BsonDocument>(tableName);
            MongoCursor<BsonDocument> myCursor;
            if (searchItems != null && searchItems.Count > 0)
            {
                var where = searchItems.ToQuery(tableName);
                myCursor = coll.Find(where);
            }
            else
            {
                myCursor = coll.FindAll();
            }
            myCursor.SetSortOrder(orders.ToSort());

            if (!string.IsNullOrEmpty(dataAfterGetHandlingScript))
            {
                myCursor.SetFields("RecordId");
            }
            else if (columns != null && columns.Count > 0)
            {
                myCursor.SetFields(columns.ToArray());
            }
            if (pageSize != -1)
            {
                myCursor.SetLimit(pageSize);
            }
            var result = myCursor.SetSkip(pageIndex).ToList();

            totalCount = myCursor.Count();


            if (!string.IsNullOrEmpty(dataAfterGetHandlingScript))
            {
                var ids = result.Select(c => c.GetValue("RecordId")).ToList();
                var recordIds = new BsonArray(ids);
                var code = db.GetScript(dataAfterGetHandlingScript);
                return db.GetResultsAs<BsonDocument>(code.Code, recordIds);
            }

            return result;
        }

        public static List<BsonDocument> GetCommonDataWithSearch(string conn, string tableName,string keyColumn, IList<SearchItem> searchItems,
           Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
           out long totalCount, string dataAfterGetHandlingScript,string searchProvider =null)
        {

            var sm = new SearchManager(conn, tableName,null, searchProvider);
            var datas = sm.GetDataIds(keyColumn, searchItems, orders, pageIndex, pageSize, out totalCount);
            var newArrayIn = new BsonArray();
            datas.ForEach(c =>
            {
                var obj = MongoTypeUtilities.BsonValueConverter("string", c); ;
                newArrayIn.Add(obj);
            });

            var db = GetDatabase(conn);
            var coll = db.GetCollection<BsonDocument>(tableName);
            var myCursor = coll.Find(Query.In(keyColumn, newArrayIn));
            myCursor.SetSortOrder(orders.ToSort());

            if (!string.IsNullOrEmpty(dataAfterGetHandlingScript))
            {
                myCursor.SetFields(keyColumn);
            }
            else if (columns != null && columns.Count > 0)
            {
                myCursor.SetFields(columns.ToArray());
            }
           
            var result = myCursor.ToList();

            if (!string.IsNullOrEmpty(dataAfterGetHandlingScript))
            {
                var ids = result.Select(c => c.GetValue(keyColumn)).ToList();
                var recordIds = new BsonArray(ids);
                var code = db.GetScript(dataAfterGetHandlingScript);
                return db.GetResultsAs<BsonDocument>(code.Code, recordIds);
            }

            return result;
        }

        public static BsonDocument GetEntity(string conn, string tableName, string pkName, string dataId, IList<string> properties)
        {
            try
            {
                var db = GetDatabase(conn);
                var coll = db.GetCollection<BsonDocument>(tableName);
                var myCursor = coll.Find(new QueryDocument("_id", dataId.ToString()));
                if (properties != null && properties.Any())
                {
                    myCursor.SetFields(properties.ToArray());
                }
                var entity = myCursor.First();
                return entity;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"get GetEntity from mongodb error,table name:{tableName},PK Name:{pkName},DataId:{dataId},columns:{JsonHelper.Serialize(properties)}";
                Log.Error(errorMessage, ee);
                return new BsonDocument();
            }
        }

        public static List<Dictionary<string, object>> GetDataByGroup(string conn, string entityName,
            IList<SearchItem> searchItems, List<string> groupBy, Dictionary<string, OrderMethod> sort, int index,
            int pageSize, out long totalCount)
        {
            try
            {
                var db = GetDatabase(conn);
                var coll = db.GetCollection(entityName);

                var agglist = new List<BsonDocument>();
                if (searchItems != null && searchItems.Any())
                {
                    var q = searchItems.ToMongoQuery(entityName);
                    var match = new BsonDocument
                    {
                        {
                            "$match", q.ToBsonDocument()
                        }
                    };
                    agglist.Add(match);
                }
                if (groupBy.Any())
                {
                    var groupItem = new BsonDocument();
                    var projectItem = new BsonDocument();
                    projectItem.Set("_id", 0);
                    groupBy.ForEach(g =>
                    {
                        groupItem.Set(g, "$" + g);
                        projectItem.Set(g, "$_id." + g);

                    });
                    projectItem.Set("Count", 1);
                    var group = new BsonDocument
                    {
                        {
                            "$group",
                            new BsonDocument
                            {
                                {
                                    "_id", groupItem
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
                    var project = new BsonDocument
                    {
                        {
                            "$project", projectItem
                        }
                    };
                    agglist.Add(group);
                    agglist.Add(project);
                }
                
                if (sort != null && sort.Any())
                {
                    var s = new BsonDocument();
                    foreach (var keyValuePair in sort)
                    {
                        s.Set(keyValuePair.Key, keyValuePair.Value == OrderMethod.Ascending ? -1 : 1);
                    }
                    var sorst = new BsonDocument()
                    {
                        {"$sort", s}
                    };
                    agglist.Add(sorst);
                }
                if (index > 0)
                {
                    var skip = new BsonDocument()
                    {
                        {"$skip", index}
                    };
                    agglist.Add(skip);
                }


                if (pageSize > 0)
                {
                    var limit = new BsonDocument()
                    {
                        {"$limit", pageSize}
                    };
                    agglist.Add(limit);
                }

                var result = coll.Aggregate(new AggregateArgs() {Pipeline = agglist.ToArray()});
                if (result != null)
                {
                    var value = new List<Dictionary<string, object>>();
                    totalCount = result.Count();
                    foreach (var bsonDocument in result)
                    {
                        var dic = bsonDocument.ToDic();
                        value.Add(dic);
                    }
                    return value;
                }
                totalCount = 0;
                return new List<Dictionary<string, object>>();
            }
            catch (Exception ex)
            {
                Log.Error("GetDataByGroup error", ex);
                totalCount = 0;
                return new List<Dictionary<string, object>>();
            }
        }




    }
}
