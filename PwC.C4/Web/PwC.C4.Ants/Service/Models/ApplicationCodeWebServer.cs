using MongoDB.Bson;

namespace PwC.C4.Ants.Service.Models
{
    public class ApplicationCodeWebServer
    {
        public ObjectId _id { get; set; }
        public string ApplicationCode { get; set; }
        public string WebServer { get; set; }
    }
}