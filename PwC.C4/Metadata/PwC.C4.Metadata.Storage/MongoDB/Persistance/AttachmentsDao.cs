using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Storage.MongoDb.Persistance
{
    internal static class AttachmentsDao
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

        public static Guid InsertAttachments(string conn, string entity, string fileName, string fileExtName,
            string userId,
            Stream stream)
        {
            try
            {
                var fileId = Guid.NewGuid().ToString();
                var db = GetDatabase(conn);
                db.GridFS.Upload(stream, fileName,
                    new MongoGridFSCreateOptions() {Id = fileId, UploadDate = DateTime.Now});

                var att = new Attachment()
                {
                    CreateBy = userId,
                    CreateTime = DateTime.UtcNow,
                    FileExtName = fileExtName,
                    _id = fileId.ToString(),
                    FileName = fileName,
                    IsDeleted = false
                };

                var attaCollect = db.GetCollection<Attachment>(entity + "_Attachments");
                var insertResult = attaCollect.Insert(att);
                var okIndex = insertResult.Response.IndexOfName("ok");
                if (okIndex >= 0)
                {
                    return insertResult.Response[okIndex] > 0 ? new Guid(att._id) : Guid.Empty;
                }
                return Guid.Empty;

            }
            catch (Exception ee)
            {
                Log.Error("Upload attachment to mongodb error ", ee);
                return Guid.Empty;
            }

        }

        internal static List<Attachment> GetAttachments(string conn, string entity, List<BsonValue> fileIds,
            bool withStream = false, bool isCreateFileToLocal = false)
        {
            try
            {
                var db = GetDatabase(conn);

                var attaCollect = db.GetCollection<Attachment>(entity + "_Attachments");
                var list = attaCollect.FindAs<Attachment>(Query.In("_id", fileIds)).ToList();
                var downloadLink = AppDomain.CurrentDomain.BaseDirectory + "\\" + AppSettings.Instance.GetDownloadLink();
                if (isCreateFileToLocal)
                {
                    if (!Directory.Exists(downloadLink))
                    {
                        Directory.CreateDirectory(downloadLink);
                    }
                }

                list.ForEach(atta =>
                {
                    if (withStream || isCreateFileToLocal)
                    {
                        var file = db.GridFS.FindOne(Query.EQ("_id", atta.FileId.ToString()));

                        using (var stream = file.OpenRead())
                        {
                            var bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, (int) stream.Length);
                            if (withStream)
                            {
                                atta.Stream = StreamHelper.BytesToStream(bytes);
                            }
                            if (isCreateFileToLocal)
                            {
                                var path = Path.Combine(downloadLink, $"{atta.FileId}{atta.FileExtName}");
                                var isExist = FileHelper.CheckFileExist(path);
                                if (!isExist)
                                {
                                    using (var newFs = new FileStream(path, FileMode.Create))
                                    {
                                        newFs.Write(bytes, 0, bytes.Length);
                                    }

                                }
                            }
                        }

                    }

                });

                return list;
            }
            catch (Exception ee)
            {
                Log.Error("Get Attachments from mongodb error", ee);
                return new List<Attachment>();
            }

        }

    }
}
