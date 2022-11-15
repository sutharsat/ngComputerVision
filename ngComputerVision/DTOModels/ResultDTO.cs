using ngComputerVision.Contracts.Entities;
using System.Collections.Generic;

namespace ngComputerVision.DTOModels
{
    public class ResultDTO
    {
        //PII
        public List<PII> PIIEntitiesResponse { get; set; }
        public List<Entity> HealthEntitiesResponse { get; set; }

        
    }
}