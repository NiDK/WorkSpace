using System;

namespace PwC.C4.Sketch.Models
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