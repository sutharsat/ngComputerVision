
using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Models;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly MongoContext _context;
        public SearchRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }



        public async Task PostSearch(Search newSearch) =>
         await _context.CreateAsync(newSearch);


    }

}
