using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace PwC.C4.Ants.Service.Models
{
    public class FileUpload
    {
        public Guid RecordId { get; set; }
        public bool Result { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public string FileGuid { get; set; }

    }
  
}