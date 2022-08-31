
using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Models;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly MongoContext _context;
        public EntityRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }

        public Task<List<Entity>> GetEntity()
        {
            throw new System.NotImplementedException();
        }

        public Task<Entity> GetEntityWithId(string id)
        {
            throw new System.NotImplementedException();
        }

     public async Task PostEntity(EntityResult newEntityResult) =>
        await _context.CreateAsync(newEntityResult);

       
    }

}
