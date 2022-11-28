using Microsoft.AspNetCore.Mvc;
using ngComputerVision.Contracts.Entities;
using ngComputerVision.DTOModels;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ngComputerVision.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClaimController : ControllerBase
    {
        private readonly IOCRRepository _ocrRepository;
        private readonly IPIIRepository _PIIRepository;
        private readonly IHealthRepository _ihealthEntityRepository;

        public ClaimController( IHealthRepository healthEntityRepository, IPIIRepository pIIRepository, IOCRRepository ocrRepository)
        {
            _PIIRepository = pIIRepository;
            _ihealthEntityRepository = healthEntityRepository;
            _ocrRepository = ocrRepository;
        }
        //mapping PII and Health collection
        [HttpGet("{id:length(24)}")]
        // public async Task<ActionResult<List<PII>>> Get(string id)
        public async Task<ActionResult<ResultDTO>> Get(string id)
        {
            ResultDTO resultDTO = new ResultDTO();
            string ocrText = "";
            //PII
            Dictionary<string, double> personDictionary = new Dictionary<string, double>();
             List<PII> categoryBasedPII = new List<PII>();
            List<Entity> categoryBasedHealth = new List<Entity>();
            //  List<PII> filteredCategoryBasedPII = new List<PII>();
            
           
            var entityHealthResult = await _ihealthEntityRepository.GetHealthEntityWithId(id);
            var PIIResult = await _PIIRepository.GetPIIResultWithId(id);
             var ocrResult = await _ocrRepository.GetOCRResultByID(id);
            var analyseResult = ocrResult.analyzeResult;
            foreach (PII piientity in PIIResult.PIIEntities)
             {

                // var analyseResult = ocrResult.analyzeResult;
                 foreach (var readResult in analyseResult.readResults)
                 {
                     foreach (var line in readResult.lines)
                     {
                       // foreach (var line in line.words)
                       // { 
                            if (line.text.Contains(piientity.Text) /*&& piientity.Text.Contains(line.text)*/)

                         {
                             piientity.BoundingBox = line.boundingBox;
                         }
                      //  }
                    }
                 }
                 categoryBasedPII.Add(piientity);
             }
            foreach (Entity healthentity in entityHealthResult.entities)
            {

               // var analyseResult = ocrResult.analyzeResult;
                foreach (var readResult in analyseResult.readResults)
                {
                    foreach (var line in readResult.lines)
                    {
                       // foreach (var line in line.words)
                       // {
                            if (line.text.Contains(healthentity.Text) /*|| healthentity.Text.Contains(line.text)*/)
                            {
                                healthentity.BoundingBox = line.boundingBox;
                            }
                      //  }
                    }
                }
                categoryBasedHealth.Add(healthentity);
            }
            foreach (var res in analyseResult.readResults)
            {
                foreach (var lines in res.lines)
                {
                    ocrText = ocrText + " " + lines.text;




                }
            }
            resultDTO.PIIEntitiesResponse = categoryBasedPII;
            resultDTO.HealthEntitiesResponse = entityHealthResult.entities;
            resultDTO.ocrText = ocrText.ToString();
            return resultDTO;




        }

    }
}



