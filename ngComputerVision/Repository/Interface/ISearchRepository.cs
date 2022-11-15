using ngComputerVision.Contracts.Entities;
using ngComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface ISearchRepository
    {
        Task PostSearch(Search newSearch);
    }
    
}
