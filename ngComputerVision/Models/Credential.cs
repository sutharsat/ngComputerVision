using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class Credential
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string credentialType { get; set; }
        public string subscriptionKey { get; set; }
        public string endpoint { get; set; }


    }
}
