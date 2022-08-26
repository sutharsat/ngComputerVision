using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class EntityResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public List<Entity> entities { get; set; }
        
       
    }
}
