using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace PwC.C4.Rush.WcfService.Models
{
    public class ApplicationCodeWebServer
    {
        public ObjectId _id { get; set; }
        public string ApplicationCode { get; set; }
        public string WebServer { get; set; }

        public string FormName { get; set; }
    }
}
