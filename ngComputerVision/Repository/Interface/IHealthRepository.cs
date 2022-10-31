using ngComputerVision.Contracts.Entities;
using ngComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface IHealthRepository
    {
       // Task<List<Entity>> GetEntity();

        Task PostEntity(EntityResult newEntityResult);
        Task<EntityResult?> GetHealthEntityWithId(string id);


    }
    
}
