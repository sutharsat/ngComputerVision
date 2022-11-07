using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Models;
using System;

namespace ngComputerVision.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class Search
    {
        [BsonId]
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("Image")]
        public byte[] claimimage { get; set; }

        public string person { get; set; } 

        public string dateOfBirth { get; set; } 
        public string phoneNumber { get; set; }

        public string email { get; set; }



    }
}