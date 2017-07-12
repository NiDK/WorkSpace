using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PwC.C4.Dfs.Common.Model
{
    [DataContract]
    public class DfsFileInfo
    {
        #region Properties

        public DfsPath Path { get; set; }

        public string Keyspace
        {
            get { return Path.Keyspace; }
        }

        public string AppCode { get; set; }

        public string Id
        {
            get { return Path.FileId; }
        }

        public string Extension
        {
            get { return Path.FileExtension; }
        }

        public string Name { get; set; }
        public string Encoding { get; set; }
        public long Length { get; set; }

        public DateTime Timestamp { get; set; }

        public int BlockCount
        {
            get { return Blocks != null ? Blocks.Length : 0; }
        }

        [DataMember]
        public DfsBlockInfo[] Blocks { get; set; }

        #endregion

        #region ctor

        public DfsFileInfo() { }

        public DfsFileInfo(int blockCount)
        {
            Blocks = new DfsBlockInfo[blockCount];
        }

        public DfsFileInfo(DfsBlockInfo[] blocks)
        {
            Blocks = blocks;
        }

        internal static DfsFileInfo Populate(DfsPath path, DfsRecord record)
        {
            if (record != null)
            {
                var info = FromJson(record.Manifest);

                info.Path = path;

                info.AppCode = record.AppCode;

                info.Name = record.Name;
                info.Encoding = record.Encoding;
                info.Length = record.Length;

                info.Timestamp = record.Timestamp;

                info.Blocks = info.Blocks ?? new DfsBlockInfo[]
                {
                    new DfsBlockInfo()
                };

                PopulateBlockInfo(path, info.Blocks);
                return info;
            }

            return null;
        }

        internal static void PopulateBlockInfo(DfsPath path, DfsBlockInfo[] blocks)
        {
            blocks[0].Path = path.ToString();

            var clone = path.Clone();
            for (int i = 1; i < blocks.Length; ++i)
            {
                clone.FileId = path.FileId + i;
                blocks[i].Path = clone.ToString();
            }
        }

        #endregion

        public void SetBlockHash(int index, string hash)
        {
            Blocks[index].Hash = hash;
        }

        #region Serialization

        private static readonly JsonSerializer _serializer = new JsonSerializer();
        public static readonly Type Type = typeof(DfsFileInfo);

        internal string ToJson()
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        internal static DfsFileInfo FromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                using (var reader = new StringReader(json))
                {
                    return (DfsFileInfo)_serializer.Deserialize(reader, Type);
                }
            }

            return new DfsFileInfo();
        }

        #endregion
    }
}
