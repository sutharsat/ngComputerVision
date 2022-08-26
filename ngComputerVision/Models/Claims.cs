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
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dateofbirth { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string ptan { get; set; }
        public int medicareID { get; set; }
        public int npi { get; set; }
        public string phonenumber { get; set; }
        public string hospitalname { get; set; }
       
        
    }
}