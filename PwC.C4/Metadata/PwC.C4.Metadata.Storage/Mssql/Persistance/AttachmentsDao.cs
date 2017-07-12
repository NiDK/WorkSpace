using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Storage.Mssql.Persistance
{
    internal static class AttachmentsDao
    {
        static readonly LogWrapper Log = new LogWrapper();

        public static bool InsertAttachments(string conn,string entity, Guid fileId, string fileName,string fileExtName,string userId)
        {
            var db = Database.GetDatabase(conn);
            var data = SafeProcedure.ExecuteNonQuery(db, "dbo.Metadata_Attachments_Insert", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@entityName", entity);
                parameters.AddWithValue("@fileId", fileId);
                parameters.AddWithValue("@fileName", fileName.Replace("'","''"));
                parameters.AddWithValue("@fileExtName", fileExtName.Replace("'", "''"));
                parameters.AddWithValue("@userId", userId);
            });
            return data > 0;
        }

        internal static List<Attachment> GetAttachments(string conn, string entity,List<Guid> fileIds)
        {
            Database db = Database.GetDatabase(conn);
            List<Attachment> list = SafeProcedure.ExecuteAndGetInstanceList<Attachment>(db,
                "dbo.Metadata_Attachments_Get",
                MapperUserInfo,
                new SqlParameter[]
                {
                    new SqlParameter("@fileIds", fileIds.ToGuidIdTable()),
                    new SqlParameter("@entity",entity)
                }
                );
            return list;
        }

        private static void MapperUserInfo(IRecord record, Attachment entity)
        {
            entity.FileId = record.Get<Guid>("FileId");
            entity.FileName = record.Get<string>("FileName");
            entity.FileExtName = record.Get<string>("FileExtName");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.CreateTime = record.Get<DateTime>("CreateTime");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }

    }
}
