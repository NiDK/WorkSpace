using System.IO;
using System.ServiceModel;

namespace PwC.C4.Ants.Service.Models
{
    [MessageContract]
    public class FileDownloadReturnMessage
    {
        public FileDownloadReturnMessage(FileMetaData metaData, Stream stream)
        {
            this.DownloadedFileMetadata = metaData;
            this.FileByteStream = stream;
        }

        [MessageHeader(MustUnderstand = true)]
        public FileMetaData DownloadedFileMetadata;
        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;
    }
}