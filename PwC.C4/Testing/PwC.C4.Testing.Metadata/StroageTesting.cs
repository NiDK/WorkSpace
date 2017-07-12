using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Storage;
using PwC.C4.Metadata.Storage.MongoDB;

namespace PwC.C4.Testing.Metadata
{

    //public static class Stroage
    //{
    //    static readonly LogWrapper Log = new LogWrapper();

    //    private static Dictionary<string, MongoDatabase> databases;

    //    internal static MongoDatabase GetDatabase(string databaseName)
    //    {
    //        var conn = AppSettings.Instance.GetConntectStringV2(databaseName);
    //        if (databases == null)
    //        {
    //            databases = new Dictionary<string, MongoDatabase>();
    //        }
    //        if (databases.ContainsKey(conn))
    //        {
    //            return databases[conn];
    //        }
    //        else
    //        {

    //            var client = new MongoClient(conn);
    //            var dbName = MongoUrl.Create(conn).DatabaseName;
    //            var server = client.GetServer();
    //            var db = server.GetDatabase(dbName);
    //            databases.Add(conn, db);
    //            return db;
    //        }
    //    }
    //    public static Dictionary<string,object> GetBsonEntity(string conn, string tableName, string pkName, string dataId, IList<string> properties)
    //    {
    //        try
    //        {
    //            var db = GetDatabase(conn);
    //            var coll = db.GetCollection<BsonDocument>(tableName);
    //            var myCursor = coll.Find(new QueryDocument("_id", dataId.ToString()));
    //            if (properties != null && properties.Any())
    //            {
    //                myCursor.SetFields(properties.ToArray());
    //            }
    //            var entity = myCursor.First();
    //            return entity.ToMetadata().Properties;
    //        }
    //        catch (Exception ee)
    //        {
    //            var errorMessage =
    //                $"get GetEntity from mongodb error,table name:{tableName},PK Name:{pkName},DataId:{dataId},columns:{JsonHelper.Serialize(properties)}";
    //            Log.Error(errorMessage, ee);
    //            return new Dictionary<string, object>();
    //        }
    //    }
    //}

    

    [TestClass]
    public class StroageTesting
    {
        [TestMethod]
        public void Get()
        {
            var p = ProviderFactory.GetProvider<IEntityService>("dbconn.C4C4BaseMongoDb", "ArrayTesting");
            var dy = new DynamicMetadata();
            var ehDic = new List<Dictionary<string, object>>();
            var eh1 = new Dictionary<string, object>
            {
                {"Creator", "Chenhui yu"},
                {"Time", DateTime.UtcNow},
                {"UserCount", 22},
                {"Message", "TestHistory 01"}
            };
            ehDic.Add(eh1);
            var eh2 = new Dictionary<string, object>
            {
                {"Creator", "Chenhui yu"},
                {"Time", DateTime.UtcNow},
                {"UserCount", 10},
                {"Message", "TestHistory 1"}
            };
            ehDic.Add(eh2);
            var eh3 = new Dictionary<string, object>
            {
                {"Creator", "Chenhui yu"},
                {"Time", DateTime.UtcNow},
                {"UserCount", 123},
                {"Message", "TestHistory 3"}
            };
            ehDic.Add(eh3);
            //dy.Properties.Set("EditHistory", ehDic);
            dy.Properties.Set("EditHistory.0", eh3);
            //dy.Properties.Set("EditHistory.1", eh2);
            //dy.Properties.Set("EditHistory.2", eh3);
            var result = p.SaveEntity<DynamicMetadata>("55300189fdbc911b749877d6", "Chenhui yu", dy.Properties);

        }

        [TestMethod]
        public void InsertData()
        {
            var user = new Dictionary<string,string>()
            {
                {"RecordId", Guid.NewGuid().ToString("N")},
                    {"UserName", "Chenhui yu"},
                    {"UserAge", "22"}
            };
            var tags = new List<string>() {"CN","HK","SG"};
            var eh = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>()
                {
                    {"RecordId", Guid.NewGuid().ToString("N")},
                    {"Creator", "Chenhui yu"},
                    {"CreateTime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                    {"Message", "Create this message!"}
                },
                new Dictionary<string, string>()
                {
                    {"RecordId", Guid.NewGuid().ToString("N")},
                    {"Creator", "Chenhui yu"},
                    {"CreateTime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                    {"Message", "Edit this message!"}
                }
            };
            var comm = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>()
                {
                    {"RecordId", Guid.NewGuid().ToString("N")},
                    {"Creator", "Chenhui yu"},
                    {"CreateTime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                    {"Message", "this is a comment!"},
                     {"ReplyTo","Chenhui yu"},
                    {"IsDeleted", "0"}
                },
                new Dictionary<string, string>()
                {
                    {"RecordId", Guid.NewGuid().ToString("N")},
                    {"Creator", "Chenhui yu"},
                    {"CreateTime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                    {"Message", "this is a comment too!"},
                     {"ReplyTo","Chenhui yu"},
                    {"IsDeleted", "0"}
                }
            };
            var data = new Dictionary<string, string>
            {
                {"RecordId", Guid.NewGuid().ToString("N")},
                {"Message", "This is a message"},
                {"CreateTime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                {"UserCount", "1000"},
                {"Tags", JsonHelper.Serialize(tags)},
                {"EditHistory", JsonHelper.Serialize(eh)},
                {"Comment", JsonHelper.Serialize(comm)},
                {"UserInfo", JsonHelper.Serialize(user)}
            };
            var json = JsonHelper.Serialize(data);

            var dic = JsonHelper.Deserialize<Dictionary<string, string>>(json);
            var meta = MetadataSettings.Instance.GetArrayColumns("ArrayTesting");
            var arrayKey = dic.Keys.Concat(meta.Keys);
            foreach (var s in arrayKey)
            {
                var sonDic = JsonHelper.Deserialize<Dictionary<string, string>>(dic[s]);
                MetadataHelper.ToObjects<DynamicMetadata>(sonDic, meta[s]);
            }
            // var p = ProviderFactory.GetProvider<IEntityService>("dbconn.C4C4BaseMongoDb", "ArrayTesting");

        }
    }

}
