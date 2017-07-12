using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using PwC.C4.Configuration.Logging;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Common
{

    

    [ServiceBehavior(IncludeExceptionDetailInFaults=true,
		InstanceContextMode=InstanceContextMode.Single)]
	public class FileRepositoryService : IFileRepositoryService
    {
        LogWrapper log = new LogWrapper();
        #region Events

        public event FileEventHandler FileRequested;
		public event FileEventHandler FileUploaded;
		public event FileEventHandler FileDeleted;

		#endregion

		#region IFileRepositoryService Members

		/// <summary>
		/// Gets or sets the repository directory.
		/// </summary>
		public string RepositoryDirectory { get; set; }

		/// <summary>
		/// Gets a file from the repository
		/// </summary>
		public Stream GetFile(string dfsPath)
		{
            var dfs = DfsPath.Parse(dfsPath);
		    return GetFileByDfsPath(dfs);

		}

        public Stream GetFileByDfsPath(DfsPath dfsPath)
        {
            try
            {
                string filePath = Path.Combine(RepositoryDirectory, dfsPath.AppCode);
                filePath = Path.Combine(filePath, dfsPath.Keyspace);
                filePath = Path.Combine(filePath, dfsPath.FileId + "." + dfsPath.FileExtension);
                //log.Error("FilePath:" + filePath);
                if (!File.Exists(filePath))
                    log.Error("File was not found,Path:"+ filePath);

                //SendFileRequested(filePath);

                var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return file;
            }
            catch (Exception ee)
            {
                log.Error("GetFileByDfsPath error dfsPath:" + dfsPath.ToString(), ee);
                return null;
            }

        }

        /// <summary>
        /// Uploads a file into the repository
        /// </summary>
        public void PutFile(FileUploadMessage msg)
	    {
            try
            {
                var dirPath = Path.Combine(RepositoryDirectory, msg.DfsPath);
                var filePath = Path.Combine(dirPath, msg.FileName);
                var dir = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(dir))
                {

                }
                else
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (var outputStream = new FileStream(filePath, FileMode.Create))
                    {
                        msg.DataStream.CopyTo(outputStream);
                    }

                    //SendFileUploaded(filePath);
                }
            }
            catch (Exception ee)
            {
                log.Error("PutFile error data:" + JsonHelper.Serialize(msg), ee);
            }
	       

	    }

	    /// <summary>
		/// Deletes a file from the repository
		/// </summary>
		public void DeleteFile(string virtualPath)
		{
			string filePath = Path.Combine(RepositoryDirectory, virtualPath);

			if (File.Exists(filePath))
			{
				//SendFileDeleted(virtualPath);
				File.Delete(filePath);
			}
		}

		/// <summary>
		/// Lists files from the repository at the specified virtual path.
		/// </summary>
		/// <param name="virtualPath">The virtual path. This can be null to list files from the root of
		/// the repository.</param>
		public StorageFileInfo[] List(string virtualPath)
		{
			string basePath = RepositoryDirectory;

			if (!string.IsNullOrEmpty(virtualPath))
				basePath = Path.Combine(RepositoryDirectory, virtualPath);

			DirectoryInfo dirInfo = new DirectoryInfo(basePath);
			FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

			return (from f in files
				   select new StorageFileInfo()
				   {
					   Size = f.Length,
					   VirtualPath = f.FullName.Substring(f.FullName.IndexOf(RepositoryDirectory) + RepositoryDirectory.Length + 1)
				   }).ToArray();
		}

		#endregion


		#region Events

		/// <summary>
		/// Raises the FileRequested event.
		/// </summary>
		protected void SendFileRequested(string vPath)
		{
			if (FileRequested != null)
				FileRequested(this, new FileEventArgs(vPath));
		}

		/// <summary>
		/// Raises the FileUploaded event
		/// </summary>
		protected void SendFileUploaded(string vPath)
		{
			if (FileUploaded != null)
				FileUploaded(this, new FileEventArgs(vPath));
		}

		/// <summary>
		/// Raises the FileDeleted event.
		/// </summary>
		protected void SendFileDeleted(string vPath)
		{
			if (FileDeleted != null)
				FileDeleted(this, new FileEventArgs(vPath));
		}

		#endregion
	}
}
