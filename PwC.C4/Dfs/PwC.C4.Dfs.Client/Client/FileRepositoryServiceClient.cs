using System;
using System.IO;
using System.ServiceModel;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Dfs.Client.Client
{
    internal class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
	{
	    private static readonly WcfSetting Wcf = WcfSettings.Instance.GetWcfSetting("C4DfsServerService");
		public FileRepositoryServiceClient()
			: base(Wcf.Binding,Wcf.Endpoint)
		{
		}

		#region IFileRepositoryService Members

		public System.IO.Stream GetFile(string virtualPath)
		{
			return base.Channel.GetFile(virtualPath);
		}

	    public Stream GetFileByDfsPath(DfsPath dfsPath)
	    {
            return base.Channel.GetFileByDfsPath(dfsPath);
        }

        public void PutFile(FileUploadMessage msg)
		{
			base.Channel.PutFile(msg);
		}

		public void DeleteFile(string virtualPath)
		{
			base.Channel.DeleteFile(virtualPath);
		}

		public StorageFileInfo[] List()
		{
			return List(null);
		}

		public StorageFileInfo[] List(string virtualPath)
		{
			return base.Channel.List(virtualPath);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (this.State == CommunicationState.Opened)
				this.Close();
		}

		#endregion
	}
}
