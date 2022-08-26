using ngComputerVision.Contracts.Entities;
using ngComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Repository.Interface
{
    public interface ICredentialRepository
    {
        public Credential GetCredential(string credentialType);

        


    }
    
}
