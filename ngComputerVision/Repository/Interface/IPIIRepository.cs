using ngComputerVision.Contracts.Entities;
using ngComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface IPIIRepository
    {
       // Task<List<Entity>> GetEntity();

        Task PostPII(PIIResult newPIIResult);
        // Task<Entity?> GetEntityWithId(string id);

        Task<PIIResult?> GetPIIResultWithId(string id);
    }
    
}
