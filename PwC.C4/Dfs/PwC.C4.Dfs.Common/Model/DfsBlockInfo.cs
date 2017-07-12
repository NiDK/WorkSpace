using System.Runtime.Serialization;

namespace PwC.C4.Dfs.Common.Model
{
    [DataContract]
    public class DfsBlockInfo
    {
        public string Path { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public long Offset { get; set; }

        [DataMember]
        public int Length { get; set; }

        public DfsBlockInfo() { }
    }
}
