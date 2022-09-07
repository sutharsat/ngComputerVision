
using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Models;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class PIIRepository : IPIIRepository
    {
        private readonly MongoContext _context;
        public PIIRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }

       

       public async Task PostPII(PIIResult newPIIResult) =>

            await _context.CreateAsync(newPIIResult);

        public async Task<PIIResult?> GetPIIResultWithId(string id)
        {
            return await _context.GetAsync(id);
        }

    }

}
