using System.IO;
using System.ServiceModel;

namespace PwC.C4.Dfs.Common
{
	[MessageContract]
	public class FileUploadMessage
	{
        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string DfsPath { get; set; }

        [MessageBodyMember(Order=1)]
		public Stream DataStream { get; set; }
	}
}
