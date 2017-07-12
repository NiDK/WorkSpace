using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Ants.Service.Interface;
using PwC.C4.Ants.Service.Models;
using PwC.C4.Common.Provider;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Storage;
using ProviderFactory = PwC.C4.Metadata.Storage.ProviderFactory;

namespace PwC.C4.Ants.Service.Provider
{
    public class FileService : IFileService
    {

        LogWrapper _log = new LogWrapper();

        #region Singleton

        private static FileService _instance = null;
        private static readonly object LockHelper = new object();

        public FileService()
        {
        }

        public static IFileService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new FileService();
                }
            }
            return _instance;
        }

        #endregion

        public string UploadFile(FileUploadMessage request)
        {
            var fileGuid = "";
            if (request?.Metadata != null && request.FileByteStream!=null)
            {
                fileGuid = ProviderFactory.GetProvider<IAttachmentService>(request.Metadata.ConnString,request.Metadata.EntityName)
                            .SaveEntityAttachment<DynamicMetadata>(request.Metadata.FileName, request.Metadata.FileExtName,
                                CurrentUser.StaffId, request.FileByteStream).ToString();
            }
            return fileGuid;
        }

        public FileDownloadReturnMessage DownloadFile(FileDownloadMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
