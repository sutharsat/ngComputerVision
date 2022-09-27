using Azure.AI.TextAnalytics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Models;
using System;
using System.Collections.Generic;

namespace ngComputerVision.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class PII
    {
        

        [BsonElement("PIIName")]
        public string Text { get; set; }

        public string Category { get; set; }

      // public int Offset { get; set; }
        
      //  public int Length { get; set; }
        //public string NormalizedText { get; set; }
        public double ConfidenceScore { get; set; }
       // public string SubCategory { get; set; }        
        public List<int> BoundingBox { get; internal set; }
    }
}