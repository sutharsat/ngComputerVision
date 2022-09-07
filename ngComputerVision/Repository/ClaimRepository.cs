using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly MongoContext _context;
        public ClaimRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }

       public async Task<List<Claims>> GetClaims() 
       { 
          return  await _context.GetAsync();
    }
       /* public async Task<PIIResult?> GetClaimsWithId(string id)
        {
            return await _context.GetAsync(id);
        }*/


        public async Task PostClaim(Claims newClaims) =>
        await _context.CreateAsync(newClaims);

        
    }

}
