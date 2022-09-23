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
            //PII
            Dictionary<string, double> personDictionary = new Dictionary<string, double>();
            // List<PII> categoryBasedPII = new List<PII>();
            //  List<PII> filteredCategoryBasedPII = new List<PII>();
            Dictionary<string, double> dateDictionary = new Dictionary<string, double>();
            Dictionary<string, double> addressDictionary = new Dictionary<string, double>();
            Dictionary<string, double> phonenumberDictionary = new Dictionary<string, double>();
            Dictionary<string, double> organizationDictionary = new Dictionary<string, double>();
            //Health
            Dictionary<string, double> treatmentnameDictionary = new Dictionary<string, double>();
            Dictionary<string, double> careenvironmentDictionary = new Dictionary<string, double>();
            Dictionary<string, double> administrativeeventDictionary = new Dictionary<string, double>();
            Dictionary<string, double> genderDictionary = new Dictionary<string, double>();
            Dictionary<string, double> healthcareprofessionDictionary = new Dictionary<string, double>();
            var entityHealthResult = await _ihealthEntityRepository.GetHealthEntityWithId(id);
            var PIIResult = await _PIIRepository.GetPIIResultWithId(id);
             /* var ocrResult = await _ocrRepository.GetOCRResultByID(id);
              foreach (PII piientity in PIIResult.PIIEntities)
              {

                  var analyseResult = ocrResult.analyzeResult;
                  foreach (var readResult in analyseResult.readResults)
                  {
                      foreach (var line in readResult.lines)
                      {
                          if (line.text.Contains(piientity.Text))
                          {
                              piientity.BoundingBox = line.boundingBox;
                          }
                      }
                  }
                  categoryBasedPII.Add(piientity);
              }
              filteredCategoryBasedPII = categoryBasedPII
              .GroupBy(customer => customer.Category)
              .Select(group => group.First()).ToList(); ;


              return filteredCategoryBasedPII;
          }*/
            foreach (PII pII in PIIResult.PIIEntities)
            {
                if (pII.Category.Equals("Person"))
                {
                    if (!personDictionary.ContainsKey(pII.Text))
                    {
                        personDictionary.Add(pII.Text, pII.ConfidenceScore);
                    }

                }

                else if (pII.Category.Equals("DateTime"))
                {
                    if (!dateDictionary.ContainsKey(pII.Text))
                    {
                        dateDictionary.Add(pII.Text, pII.ConfidenceScore);
                    }
                }
                else if (pII.Category.Equals("Address"))
                {
                    if (!addressDictionary.ContainsKey(pII.Text))
                    {
                        addressDictionary.Add(pII.Text, pII.ConfidenceScore);
                    }
                }
                else if (pII.Category.Equals("PhoneNumber"))
                {
                    if (!phonenumberDictionary.ContainsKey(pII.Text))
                    {
                        phonenumberDictionary.Add(pII.Text, pII.ConfidenceScore);
                    }
                }
                else if (pII.Category.Equals("Organization"))
                {
                    if (!organizationDictionary.ContainsKey(pII.Text))
                    {
                        organizationDictionary.Add(pII.Text, pII.ConfidenceScore);

                    }
                }
            }
            foreach (Entity entity in entityHealthResult.entities)
            {
                if (entity.Category.Equals("TreatmentName"))
                {
                    if (!treatmentnameDictionary.ContainsKey(entity.Text))
                    {
                        treatmentnameDictionary.Add(entity.Text, entity.ConfidenceScore);

                    }
                }

                else if (entity.Category.Equals("Gender"))
                {
                    if (!genderDictionary.ContainsKey(entity.Text))
                    {
                        genderDictionary.Add(entity.Text, entity.ConfidenceScore);

                    }
                }
                else if (entity.Category.Equals("CareEnvironment"))
                {
                    if (!careenvironmentDictionary.ContainsKey(entity.Text))
                    {
                        careenvironmentDictionary.Add(entity.Text, entity.ConfidenceScore);

                    }
                }
                else if (entity.Category.Equals("AdministrativeEvent"))
                {
                    if (!administrativeeventDictionary.ContainsKey(entity.Text))
                    {
                        administrativeeventDictionary.Add(entity.Text, entity.ConfidenceScore);

                    }
                }
                else if (entity.Category.Equals("HealthcareProfession"))
                {
                    if (!healthcareprofessionDictionary.ContainsKey(entity.Text))
                    {
                        healthcareprofessionDictionary.Add(entity.Text, entity.ConfidenceScore);
                    }
                }
            }
            //For PII
            if (personDictionary.Count >= 1)
            {
                resultDTO.firstname = personDictionary.ElementAt(1).Key;
                resultDTO.firstnamecs = personDictionary.ElementAt(1).Value;

                resultDTO.lastname = personDictionary.ElementAt(0).Key;
                resultDTO.lastnamecs = personDictionary.ElementAt(0).Value;
            }
            if (phonenumberDictionary.Count >= 1)
            {
                resultDTO.phonenumber = phonenumberDictionary.ElementAt(0).Key;
                resultDTO.phonenumbercs = phonenumberDictionary.ElementAt(0).Value;
            }
            if (dateDictionary.Count >= 1)
            {
                resultDTO.dateofbirth = dateDictionary.ElementAt(0).Key;
                resultDTO.dateofbirthcs = dateDictionary.ElementAt(0).Value;
            }
            if (addressDictionary.Count >= 1)
            {
                resultDTO.address = addressDictionary.ElementAt(0).Key;
                resultDTO.addresscs = addressDictionary.ElementAt(0).Value;
            }
            if (organizationDictionary.Count >= 1)
            {
                resultDTO.hospitalname = organizationDictionary.ElementAt(0).Key;
                resultDTO.hospitalnamecs = organizationDictionary.ElementAt(0).Value;
            }
            //For Health
            if (treatmentnameDictionary.Count >= 1)
            {
                resultDTO.treatmentname = treatmentnameDictionary.ElementAt(0).Key;
                resultDTO.treatmentnamecs = treatmentnameDictionary.ElementAt(0).Value;
            }
            if (healthcareprofessionDictionary.Count >= 1)
            {
                resultDTO.healthcareprofession = healthcareprofessionDictionary.ElementAt(0).Key;
                resultDTO.healthcareprofessioncs = healthcareprofessionDictionary.ElementAt(0).Value;
            }
            if (careenvironmentDictionary.Count >= 1)
            {
                resultDTO.careenvironment = careenvironmentDictionary.ElementAt(0).Key;
                resultDTO.careenvironmentcs = careenvironmentDictionary.ElementAt(0).Value;
            }
            if (administrativeeventDictionary.Count >= 1)
            {
                resultDTO.administrativeevent = administrativeeventDictionary.ElementAt(0).Key;
                resultDTO.administrativeeventcs = administrativeeventDictionary.ElementAt(0).Value;
            }
            if (genderDictionary.Count >= 1)
            {
                resultDTO.gender = genderDictionary.ElementAt(0).Key;
                resultDTO.gendercs = genderDictionary.ElementAt(0).Value;
            }
            return resultDTO;
        }

    }
}



