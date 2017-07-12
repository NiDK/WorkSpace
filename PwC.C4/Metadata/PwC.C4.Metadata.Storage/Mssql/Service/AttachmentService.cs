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
using PwC.C4.Metadata.Storage.Mssql.Persistance;

namespace PwC.C4.Metadata.Storage.Mssql.Service
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
                var checkname = fileName;
                checkname = System.IO.Path.GetFileName(checkname);
                var gid = Guid.NewGuid();
                var extname = Path.GetExtension(fileName);
                var fileFullName = gid + extname;
                var pathForSaving = AppSettings.Instance.GetUploadPath();
                var newFileName = "";
                var filePath = FileHelper.CheckFileExistReturnNewPath(pathForSaving, fileFullName,
                    out newFileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    var bytes = new byte[stream.Length];
                    var numBytesRead = 0;
                    var numBytesToRead = (int)stream.Length;
                    stream.Position = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = stream.Read(bytes, numBytesRead, Math.Min(numBytesToRead, int.MaxValue));
                        if (n <= 0)
                        {
                            break;
                        }
                        fs.Write(bytes, numBytesRead, n);
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    fs.Close();
                }
                var fileId = Guid.NewGuid();
                var table = MetadataHelper.GetEntityName<T>(_entityName);
                var result = AttachmentsDao.InsertAttachments(_connName, table, fileId, checkname, fileExtName, userId);
                return result ? fileId : Guid.Empty;

            }
            catch (Exception ee)
            {
                log.Error("Save Entity Attachment to sql filetable error", ee);
                return Guid.Empty;
            }
        }

        public List<Attachment> GetEntityAttachments<T>(List<Guid> fileIds, bool withStream = false, bool isCreateFileToLocal = false)
        {
             var table = MetadataHelper.GetEntityName<T>(_entityName);
            var result = AttachmentsDao.GetAttachments(_connName, table, fileIds);
            return result;
        }

        public List<Attachment> GetEntityAttachments<T>(List<string> fileIds, bool withStream = false, bool isCreateFileToLocal = false)
        {
            throw new NotImplementedException("Please use GetEntityAttachments<T>(List<Guid> fileIds, bool withStream = false)");
        }
    }
}
