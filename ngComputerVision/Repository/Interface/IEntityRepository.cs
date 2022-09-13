using ngComputerVision.Contracts.Entities;
using ngComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface IEntityRepository
    {
       // Task<List<Entity>> GetEntity();

        Task PostEntity(EntityResult newEntityResult);
        Task<EntityResult?> GetHealthEntityWithId(string id);


    }
    
}
