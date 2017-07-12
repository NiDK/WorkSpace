using MongoDB.Bson;

namespace PwC.C4.Ants.Service.Models
{
    public class LinkTracking
    {
        public ObjectId _id { get; set; }
        public string NotesDocID { get; set; }
        public string NotesLink { get; set; }
        public string FormName { get; set; }
        public string FormID { get; set; }
        public string MongoDBServer { get; set; }
        public string MongoDB { get; set; }
        public string MongoDocID { get; set; }

        public string NewUrl { get; set; }

        public string APPCode { get; set; }


    }
}