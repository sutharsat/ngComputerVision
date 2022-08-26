using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.Models;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ngComputerVision.Repository
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly MongoContext _context;
        public CredentialRepository(MongoContext mongoContext)
        {
            _context = mongoContext;
        }
 public Credential GetCredential(string credentialType)
        {
            var allCredential=  _context.GetCredentialAsync();
            return allCredential.FirstOrDefault(x=>x.credentialType== credentialType);
        }
    }

}
