using System;
using MongoDB.Bson;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.Ants.Service.Models
{
    public class AttachmentModel
    {
        public string ControlName { get; set; }
        public PageMode PageMode { get; set; }
        public dynamic Entity { get; set; }

        public string AttachmentId { get; set; }
        public string ModelClass { get; set; }
    }

    public class ContentAttachment
    {
        //public ContentAttachment();
        public string _id { get; set; }
        public string Filename { get; set; }
        public Int64 Length { get; set; }
        public Int32 ChunkSize { get; set; }
        public DateTime UploadDate { get; set; }

        public string Md5 { get; set; }

        public BsonDocument Metadata { get; set; }
        public string ContentType { get; set; }
    }
}