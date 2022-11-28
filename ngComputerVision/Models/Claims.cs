using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Models;
using System;

namespace ngComputerVision.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class Claims
    {
        [BsonId]
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("ClaimName")]
        public string status { get; set; }

        public string createdDateTime { get; set; } = null!;

        public string lastUpdatedDateTime { get; set; } = null!;
        public AnalyzeResult analyzeResult { get; set; }

        public byte[] claimimage { get; set; }
      
       
        
    }
}