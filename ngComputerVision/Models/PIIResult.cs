using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class PIIResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public List<PII> PIIEntities { get; set; }
        
       
    }
}
