using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface IClaimRepository
    {
        Task<List<Claims>> GetClaims();

        Task PostClaim(Claims newClaims);
        Task<Claims?> GetClaimsWithId(string id);


    }
    
}
