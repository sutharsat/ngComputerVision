using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class OCRRepository : IOCRRepository
    {
        private readonly MongoContext _context;
        public OCRRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }

       public async Task<List<Claims>> GetClaims() 
       { 
          return  await _context.GetAsync();
    }
        public async Task<Claims?> GetOCRResultByID(string id)
        {
            return await _context.GetOCRResultByID(id);
        }


        public async Task PostClaim(Claims newClaims) =>
        await _context.CreateAsync(newClaims);

        
    }

}
