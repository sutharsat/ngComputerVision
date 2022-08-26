using Azure.AI.TextAnalytics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Models;
using System;

namespace ngComputerVision.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class Entity
    {
        

        [BsonElement("EntityName")]
        public string Text { get; set; }

        public string Category { get; set; }

        public int Offset { get; set; }
        
        public int Length { get; set; }
        public string NormalizedText { get; set; }
        public double ConfidenceScore { get; set; }
        public string SubCategory { get; set; }
        
       
        
    }
}