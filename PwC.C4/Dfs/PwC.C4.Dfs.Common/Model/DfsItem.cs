using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Exceptions;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Common.Model
{
    public class DfsItem
    {
        #region Path

        public DfsPath Path { get; private set; }

        public string Keyspace
        {
            get { return Path.Keyspace; }
        }

        public string AppCode
        {
            get { return Path.AppCode; }
        }

        public string FileId
        {
            get { return Path.FileId; }
        }

        public string FileExtension
        {
            get { return Path.FileExtension; }
        }

        #endregion

        #region FileData Members

        public string FileType { get; private set; }
        public string FileName { get; private set; }
        public object FileData { get; private set; }

        public long Length
        {
            get
            {
                return IsStream ? FileDataStream.Length : FileDataBytes.Length;
            }
        }

        public Stream FileDataStream
        {
            get { return FileData as Stream; }
        }

        public byte[] FileDataBytes
        {
            get { return FileData as byte[]; }
        }

        public bool IsStream
        {
            get { return FileDataStream != null; }
        }

        #endregion

        #region Metadata

        public int MetadataCount
        {
            get
            {
                return Metadata != null ? Metadata.Count : 0;
            }
        }

        public Dictionary<string, object> Metadata { get; set; }

        public string Encoding { get; set; }

        public DateTime Timestamp { get; set; }

        #endregion

        #region ctor

        public DfsItem(string fileType, string fileName, Stream fileData, string encoding, string appCode, Dictionary<string, object> metadata=null)
        {
            ArgumentHelper.AssertValuesNotEmpty(fileType, fileName);
            ArgumentHelper.AssertNotNull(fileData);
            ArgumentHelper.AssertIsTrue(fileData.Position == 0, "File pointer must be at the begining of the file");

            Initialize(fileType, fileName, string.Empty, fileData, encoding, appCode, metadata);
        }

        /// <summary>
        /// Initialize an instance of DfsItem
        /// If fileId is not designated(null or empty), a Guid is generated as the file's id
        /// </summary>
        public DfsItem(string fileType, string fileName, string fileId, Stream fileData, string encoding, string appCode, Dictionary<string, object> metadata = null)
        {
            ArgumentHelper.AssertValuesNotEmpty(fileType, fileName, fileId);
            ArgumentHelper.AssertNotNull(fileData);
            ArgumentHelper.AssertIsTrue(fileData.Position == 0, "File pointer must be at the begining of the file");

            Initialize(fileType, fileName, fileId, fileData, encoding, appCode, metadata);
        }

        public DfsItem(string fileType, string fileName, byte[] fileData, string encoding,string appCode, Dictionary<string, object> metadata = null)
        {
            ArgumentHelper.AssertValuesNotEmpty(fileType, fileName);
            ArgumentHelper.AssertNotNull(fileData);

            Initialize(fileType, fileName, string.Empty, fileData, encoding, appCode, metadata);
        }

        /// <summary>
        /// Initialize an instance of DfsItem
        /// If fileId is not designated(null or empty), a Guid is generated as the file's id
        /// </summary>
        public DfsItem(string fileType, string fileName, string fileId, byte[] fileData, string encoding, string appCode, Dictionary<string, object> metadata = null)
        {
            ArgumentHelper.AssertValuesNotEmpty(fileType, fileName, fileId);
            ArgumentHelper.AssertNotNull(fileData);

            Initialize(fileType, fileName, fileId, fileData, encoding, appCode, metadata);
        }

        public DfsItem(DfsPath path, string fileName, byte[] fileData,
            Dictionary<string, object> metadata, string encoding, DateTime timestamp)
        {
            ArgumentHelper.AssertNotNull(path);
            Initialize(path, fileName, fileData, metadata, encoding, timestamp);
        }

        public DfsItem(DfsPath path, string fileName, Stream fileData,
            Dictionary<string, object> metadata, string encoding, DateTime timestamp)
        {
            ArgumentHelper.AssertNotNull(path);
            Initialize(path, fileName, fileData, metadata, encoding, timestamp);
        }

        #endregion

        #region Initialize

        private void Initialize(DfsPath path, string fileName, object fileData, 
                                Dictionary<string, object> metadata, string encoding, DateTime timestamp)
        {
            Path = path;
            FileName = fileName;
            FileData = fileData;
            Metadata = metadata;
            Encoding = encoding;
            Timestamp = timestamp;
        }

        private void Initialize(string fileType, string fileName, string fileId, object fileData, string encoding, string appCode,Dictionary<string,object> metadata=null)
        {
            FileType = fileType;
            FileName = fileName;
            FileData = fileData;
            Encoding = encoding;
            if (metadata != null)
                Metadata = metadata;
            Path = new DfsPath(DetermineKeyspace(fileType), appCode, DetermineFileId(fileId), RetrieveExtension(fileName));

            EnsureFileLength();
        }

        #endregion

        #region Read

        internal byte[] Read(long offset, int length)
        {
            var buffer = new byte[length];

            int read = IsStream ? FileDataStream.Read(buffer, 0, length)
                                : CopyTo(offset, length, buffer);

            return buffer;
        }

        private int CopyTo(long offset, int length, byte[] buffer)
        {
            Array.Copy(FileDataBytes, offset, buffer, 0, length);
            return length;
        }

        [Obsolete("Just read FileDataStream")]
        public byte[] GetFileBinary()
        {
            if (IsStream)
            {
                var stream = FileDataStream;
                var length = (int)Length;
                var binary = new byte[length];

                int read = 0;
                while (read < length)
                    read += stream.Read(binary, read, length - read);

                return binary;
            }

            return FileDataBytes;
        }

        #endregion

        public string GetDfsPath()
        {
            return Path.ToString();
        }

        #region Helper

        private static readonly Regex regex = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);
        private static string VerifyFileId(string id)
        {
            if (!regex.IsMatch(id))
            {
                throw new DfsException("Invalid File Id: " + id + ", only chars in [a-z0-9_] are allowed");
            }

            return id;
        }

        private void EnsureFileLength()
        {
            ArgumentHelper.AssertIsTrue(Length > 0, "File is empty");
            ArgumentHelper.AssertIsTrue(Length <= DfsConfig.Instance.GetMaxFileSize(Keyspace), "File is too large");
        }

        private static string DetermineFileId(string fileId)
        {
            return string.IsNullOrEmpty(fileId) ? Guid.NewGuid().ToString("N") 
                                                : VerifyFileId(fileId);
        }

        private static string DetermineKeyspace(string fileType)
        {
            string keyspace = DfsConfig.Instance.GetKeyspace(fileType);

            if (string.IsNullOrEmpty(keyspace))
            {
                throw new DfsException(string.Format("Keyspace for file type '{0}' not found!", fileType));
            }

            return keyspace;
        }

        private static string RetrieveExtension(string file)
        {
            var extension = System.IO.Path.GetExtension(file);

            if (string.IsNullOrEmpty(extension))
            {
                throw new DfsException("Invalid file name, extension is required: " + file);
            }

            return extension.Substring(1);
        }

        #endregion
    }
}
