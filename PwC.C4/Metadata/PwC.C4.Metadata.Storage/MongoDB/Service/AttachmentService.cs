using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Interface;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Storage.MongoDb.Persistance;

namespace PwC.C4.Metadata.Storage.MongoDb.Service
{
    public class AttachmentService : IAttachmentService
    {

        private LogWrapper log = new LogWrapper();

        #region Singleton

        private static AttachmentService _instance = null;
        private static readonly object LockHelper = new object();
        private static string _connName = null;
        private static string _entityName = null;
        public AttachmentService(string connectName, string entityName = null)
        {
            _connName = connectName;
            _entityName = entityName;
        }

        public static IAttachmentService Instance(string connectName, string entityName = null)
        {
            
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new AttachmentService(connectName, entityName);
                }
            }
            return _instance;
        }

#if DEBUG

        public static AttachmentService DebugInstance(string connectName, string entityName = null)
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new AttachmentService(connectName, entityName);
                }
            }
            return _instance;
        }

#endif

        #endregion

        public Guid SaveEntityAttachment<T>(string fileName, string fileExtName, string userId, Stream stream)
        {

            try
            {
                var table = MetadataHelper.GetEntityName<T>(_entityName);
                var result = AttachmentsDao.InsertAttachments(_connName,table, fileName, fileExtName, userId, stream);
                return result;
            }
            catch (Exception ee)
            {
                log.Error("Save Entity Attachment to Mongodb error", ee);
                return Guid.Empty;
            }

        }

        public List<Attachment> GetEntityAttachments<T>(List<Guid> fileIds,bool withStream=false, bool isCreateFileToLocal = false)
        {
            var table = MetadataHelper.GetEntityName<T>(_entityName);
            var bslist = new List<BsonValue>();
            fileIds.ForEach(c => bslist.Add(c.ToString()));
            var result = AttachmentsDao.GetAttachments(_connName, table, bslist, withStream, isCreateFileToLocal);
            return result;
        }

        public List<Attachment> GetEntityAttachments<T>(List<string> fileIds, bool withStream = false, bool isCreateFileToLocal = false)
        {
            var table = MetadataHelper.GetEntityName<T>(_entityName);
            var bslist = new List<BsonValue>();
            fileIds.ForEach(c => bslist.Add(c));
            var result = AttachmentsDao.GetAttachments(_connName, table, bslist, withStream, isCreateFileToLocal);
            return result;
        }
    }
}
