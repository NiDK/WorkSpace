using System.Runtime.Serialization;

namespace PwC.C4.Ants.Service.Models
{
    [DataContract(Namespace = "http://schemas.acme.it/2009/04")]
    public class FileMetaData
    {
        public FileMetaData(
            string fileName,
            string fileExtName,string entityName,string connString)
        {
            this.FileName = fileName;
            this.FileExtName = fileExtName;
            this.EntityName = entityName;
            this.ConnString = connString;
        }


        [DataMember(Name = "FileName", Order = 0, IsRequired = true)] public string FileName;
        [DataMember(Name = "FileExtName", Order = 1, IsRequired = true)] public string FileExtName;
        [DataMember(Name = "EntityName", Order = 2, IsRequired = true)] public string EntityName;
        [DataMember(Name = "ConnString", Order = 3, IsRequired = true)] public string ConnString;
    }
}