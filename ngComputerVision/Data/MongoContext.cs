using Microsoft.Extensions.Options;
using ngComputerVision.Contracts;
using MongoDB.Driver;
using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ngComputerVision.Models;
using Microsoft.Extensions.Configuration;

namespace ngComputerVision.Core.Data
{
    public class MongoContext
    {
        private readonly IConfiguration _config;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        public IMongoCollection<Claims> _claimsCollection;
        public IMongoCollection<EntityResult> _entityCollection;
        public IMongoCollection<Credential> _credentialCollection;

        public MongoContext(IOptions<DatabaseSettings> dbOptions, IConfiguration config)
        {
            var settings = dbOptions.Value;
            _client = new MongoClient(config["MongoConnection:ConnectionString"]);
            _database = _client.GetDatabase(settings.DatabaseName);
            _claimsCollection = _database.GetCollection<Claims>(
           settings.ClaimsCollectionName);
            _entityCollection = _database.GetCollection<EntityResult>(
           settings.EntityResultCollectionName);
            _credentialCollection = _database.GetCollection<Credential>(
           settings.CredentialCollectionName);
        }
        public async Task<List<Claims>> GetAsync() =>
       await _claimsCollection.Find(_ => true).ToListAsync();
        public async Task<Claims?> GetAsync(string id) =>
            await _claimsCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Claims newClaims) =>
                await _claimsCollection.InsertOneAsync(newClaims);
        //setting for Entity
       
        public async Task CreateAsync(EntityResult newEntityResult) =>
                await _entityCollection.InsertOneAsync(newEntityResult);
        //setting for Credentials
        public  List<Credential?> GetCredentialAsync()
        {
            return  _credentialCollection.Find(_ => true).ToList();
        }
    }
}