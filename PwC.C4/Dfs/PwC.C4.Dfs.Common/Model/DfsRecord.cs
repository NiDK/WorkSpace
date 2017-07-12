using System;
using System.Collections.Generic;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Dfs.Common.Model
{
    public class DfsRecord:DynamicMetadata
    {
        #region Properties


        public bool IsNullOrEmpty => Properties == null || Properties.Count == 0;

        public string AppCode
        {
            get { return SafeGet<string>(Column.AppCode);  }
            set { SafeSet(Column.AppCode, value); }
        }

        public string Keyspace
        {
            get { return SafeGet<string>(Column.Keyspace); }
            set { SafeSet(Column.Keyspace, value); }
        }

        public string Name
        {
            get { return SafeGet<string>(Column.Name); }
            set { SafeSet(Column.Name, value); }
        }

        public string Encoding
        {
            get { return SafeGet<string>(Column.Encoding); }
            set { SafeSet(Column.Encoding, value); }
        }

        public long Length
        {
            get { return SafeGet<long>(Column.Length); }
            set { SafeSet(Column.Length, value); }
        }

        public byte[] Data
        {
            get { return SafeGet<byte[]>(Column.Data); }
            set { SafeSet(Column.Data, value); }
        }

        public string Manifest
        {
            get { return SafeGet<string>(Column.Manifest); }
            set { SafeSet(Column.Manifest, value); }
        }

        public string DfsPath
        {
            get { return SafeGet<string>(Column.DfsPath); }
            set { SafeSet(Column.DfsPath, value); }
        }

        public DateTime UploadTime
        {
            get { return SafeGet<DateTime>(Column.FinishTime); }
            set { SafeSet(Column.FinishTime, value); }
        }

        public Dictionary<string, object> Metadata
        {
            get { return SafeGet<Dictionary<string, object>>(Column.Metadata); }
            set { SafeSet(Column.Metadata, value); }
            //get
            //{
            //    Dictionary<string, object> result = null;

            //    foreach (var kvp in Properties)
            //    {
            //        string key = kvp.Key;
            //        if (key.StartsWith("_")) // Metadata keys start with "_"
            //        {
            //            if (result == null)
            //            {
            //                result = new Dictionary<string, object>();
            //            }

            //            result[key] = kvp.Value;
            //        }
            //    }

            //    return result;
            //}
        }


        private long _Timestamp
        {
            get { return SafeGet<long>(Column.Timestamp); }
            set { SafeSet(Column.Timestamp, value); }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        #endregion

        #region ctor

        public DfsRecord(Dictionary<string,object> columns)
        {
            Properties = new Dictionary<string, object>(columns);
            _timestamp = RetrieveTimestamp();
        }

        public DfsRecord()
        {
            Properties = new Dictionary<string, object>();
        }

        private DateTime RetrieveTimestamp()
        {
            if (!IsNullOrEmpty)
            {
                return new DateTime(this._Timestamp, DateTimeKind.Utc);
            }
            return DateTime.MinValue;
        }

        #endregion

        public static class Column
        {
            public static readonly int RequiredColumns = 3;
            public static readonly int RequiredInfoColumns = 2;

            public const string Keyspace = "Keyspace";
            public const string AppCode = "AppCode"; // Required
            public const string Name = "Name"; // Required
            public const string Encoding = "Encoding";
            public const string Length = "Length";
            public const string Data = "Binary"; // Required
            public const string Manifest = "Manifest";
            public const string DfsPath = "DfsPath";
            public const string FinishTime = "FinishTime";
            public const string Metadata = "Metadata";
            public const string Timestamp = "Timestamp";
            public static readonly List<byte[]> InfoColumns;

            static Column()
            {
                InfoColumns = new List<byte[]>() {
                    ToBytes(Keyspace),
                    ToBytes(AppCode),
                    ToBytes(Name),
                    ToBytes(Encoding),
                    ToBytes(Length),
                    ToBytes(Manifest),
                     ToBytes(DfsPath),
                     ToBytes(FinishTime),
                    ToBytes(Timestamp)
                };
            }

            private static byte[] ToBytes(string str)
            {
                return System.Text.Encoding.UTF8.GetBytes(str);
            }
        }
    }
}
