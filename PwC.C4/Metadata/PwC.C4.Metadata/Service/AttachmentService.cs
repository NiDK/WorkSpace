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
using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
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
using PwC.C4.Metadata.Persistance;

namespace PwC.C4.Metadata.Service
{
    public class AttachmentService : IAttachmentService
    {

        private LogWrapper log = new LogWrapper();

        #region Singleton

        private static AttachmentService _instance = null;
        private static readonly object LockHelper = new object();

        public AttachmentService()
        {
        }

        public static IAttachmentService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new AttachmentService();
                }
            }
            return _instance;
        }

#if DEBUG

        public static AttachmentService DebugInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new AttachmentService();
                }
            }
            return _instance;
        }

#endif

        #endregion

        public bool SaveEntityAttachment<T>(Guid fileId, string fileName, string fileExtName, string userId)
        {
            var table = MetadataHelper.GetEntityName<T>();
            var result = AttachmentsDao.InsertAttachments(table, fileId, fileName, fileExtName, userId);
            return result;
        }

        public List<Attachment> GetEntityAttachments<T>(List<Guid> fileIds)
        {
            var table = MetadataHelper.GetEntityName<T>();
            var result = AttachmentsDao.GetAttachments(table, fileIds);
            return result;
        }

        public bool PackagingZip(List<Attachment> atts, string zipName, string password = null)
        {
            try
            {
                var zipPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                AppSettings.Instance.GetPackagePath(), zipName);
                var filePath = AppSettings.Instance.GetUploadPath();
                var outStream = new FileStream(zipPath, FileMode.Create, FileAccess.ReadWrite);
                using (ZipFile zipFile = ZipFile.Create(outStream))
                {
                    zipFile.Password = password;
                    zipFile.BeginUpdate();
                    foreach (var file in atts)
                    {
                        zipFile.Add(Path.Combine(filePath, file.FileId + file.FileExtName), file.FileName);
                    }
                    zipFile.CommitUpdate();
                }
                outStream.Flush();
                outStream.Close();
                return true;
            }
            catch (Exception ee)
            {
                log.Error("Package Error",ee);
                return false;
            }
            
        }
    }
}
