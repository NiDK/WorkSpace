using System.IO;
using System.ServiceModel;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Dfs.Common
{
	
	[ServiceContract]
	public interface IFileRepositoryService
	{
		[OperationContract]
		Stream GetFile(string virtualPath);

        [OperationContract]
        Stream GetFileByDfsPath(DfsPath dfsPath);

        [OperationContract]
		void PutFile(FileUploadMessage msg);

		[OperationContract]
		void DeleteFile(string virtualPath);

		[OperationContract]
		StorageFileInfo[] List(string virtualPath);
	}
}
