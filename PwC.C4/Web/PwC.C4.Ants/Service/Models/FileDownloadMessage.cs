using System.ServiceModel;

namespace PwC.C4.Ants.Service.Models
{
    [MessageContract]
    public class FileDownloadMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public FileMetaData FileMetaData;
    }
}