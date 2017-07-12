using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PwC.C4.Metadata.Storage.MongoDb.Models
{
    public class HashTableDocument
    {
        public ObjectId Id { get; set; }
        [BsonExtraElements]
        public Dictionary<string, object> Values { get; set; }

    }
}
