using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Ants.Service.Models;

namespace PwC.C4.Ants.Service.Interface
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract(IsOneWay = true)]
        string UploadFile(FileUploadMessage request);
        [OperationContract(IsOneWay = false)]
        FileDownloadReturnMessage DownloadFile(FileDownloadMessage request);
    }
}
