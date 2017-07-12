using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PwC.C4.Ants.Service.Interface;
using PwC.C4.Ants.Service.Models;

namespace PwC.C4.Ants.Service.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FileService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FileService.svc or FileService.svc.cs at the Solution Explorer and start debugging.
    public class FileService : IFileService
    {
        public void DoWork()
        {
        }

        public string UploadFile(FileUploadMessage request)
        {
            throw new NotImplementedException();
        }

        public FileDownloadReturnMessage DownloadFile(FileDownloadMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
