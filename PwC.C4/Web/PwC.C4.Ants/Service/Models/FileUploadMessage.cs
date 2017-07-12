using System.IO;
using System.ServiceModel;

namespace PwC.C4.Ants.Service.Models
{
    [MessageContract]
    public class FileUploadMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public FileMetaData Metadata;
        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;
    }
}