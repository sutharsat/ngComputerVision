using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ngComputerVision.Contracts.Entities;
using ngComputerVision.DTOModels;
using ngComputerVision.Models;
using ngComputerVision.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ngComputerVision.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OCRController : Controller
    {
        static string ocrapisubscriptionKey;
        static string ocrAPIEndpoint;
        static string ocrAPIURI;
        static string healthapisubscriptionKey;
        static string healthApiEndpoint;
        private readonly IOCRRepository _ocrRepository;
        private readonly IHealthRepository _healthRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IPIIRepository _PIIRepository;
        private readonly ISearchRepository _searchRepository;
        private static AzureKeyCredential healthApiCredential;
        private AzureKeyCredential piiApiCredential;
        private static Uri healthApiEndpointURI;
        private static Uri piiApiEndpointURI;
        private string healthApiEndpointURI2;
        private string piiapisubscriptionKey;
        private string piiApiEndpoint;

        public OCRController(IOCRRepository ocrRepository, IHealthRepository healthRepository, ICredentialRepository credentialRepository, IPIIRepository PIIRepository,ISearchRepository searchRepository)
        {
            _ocrRepository = ocrRepository;
            _healthRepository = healthRepository;
            _credentialRepository = credentialRepository;
            _PIIRepository = PIIRepository;
            _searchRepository = searchRepository;
            //OCR Image to Text Detection API
            ocrapisubscriptionKey = _credentialRepository.GetCredential("OCRAPI").subscriptionKey.ToString();
            ocrAPIEndpoint = _credentialRepository.GetCredential("OCRAPI").endpoint.ToString();
            ocrAPIURI = ocrAPIEndpoint + "vision/v3.2/read/syncAnalyze";

            //Health Related Info Analytic API
            healthapisubscriptionKey = _credentialRepository.GetCredential("ANALYTICAPI").subscriptionKey.ToString();
            healthApiCredential = new AzureKeyCredential(healthapisubscriptionKey);
            healthApiEndpoint = _credentialRepository.GetCredential("ANALYTICAPI").endpoint.ToString();
            healthApiEndpointURI = new Uri(healthApiEndpoint);
            healthApiEndpointURI2 = healthApiEndpoint + "/language/analyze-text/jobs";

            //PII(Personal Info) Detection API
            piiapisubscriptionKey = _credentialRepository.GetCredential("PIIAPI").subscriptionKey.ToString();
            piiApiCredential = new AzureKeyCredential(piiapisubscriptionKey);
            piiApiEndpoint = _credentialRepository.GetCredential("PIIAPI").endpoint.ToString();
            piiApiEndpointURI = new Uri(piiApiEndpoint);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<OcrResultDTO> Post()
        {
            string ocrText = "";
            OcrResultDTO ocrResultDTO = new OcrResultDTO();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[Request.Form.Files.Count - 1];

                    if (file.Length > 0)
                    {
                        try
                        {
                            var memoryStream = new MemoryStream();
                            file.CopyTo(memoryStream);
                            byte[] imageFileBytes = memoryStream.ToArray();
                            memoryStream.Flush();

                            string myJSONResult = await ReadTextFromStream(imageFileBytes);
                            var result = new Claims();
                           // var search = new Search();
                            result = JsonConvert.DeserializeObject<Claims>(myJSONResult);
                            //call the ocr repository's Create method here.This will save the OCR results in database.

                            Claims? newClaim = System.Text.Json.JsonSerializer.Deserialize<Claims>(myJSONResult);

                           // Search? newSearch = System.Text.Json.JsonSerializer.Deserialize<Search>(myJSONResult);

                            foreach (var res in result.analyzeResult.readResults)
                            {
                                foreach (var lines in res.lines)
                                {
                                    ocrText = ocrText + " " + lines.text;




                                }
                            }


                            /* JObject json = JObject.Parse(ocrText);*/

                            newClaim.claimimage = imageFileBytes;
                          //  newSearch.claimimage = imageFileBytes;
                           // await _searchRepository.PostSearch(newSearch);

                            await _ocrRepository.PostClaim(newClaim);

                            ocrResultDTO.DetectedText = ocrText.ToString();
                            ocrResultDTO.Language = "en";
                            // string firstNamekey;
                            Console.WriteLine("generatedId for vision response" + newClaim.id.ToString());

                            //method for APIs
                            var healthAPIClient = new TextAnalyticsClient(healthApiEndpointURI, healthApiCredential);
                            await ExtractSaveHealthRelatedInfo(healthAPIClient, ocrResultDTO.DetectedText, newClaim.id.ToString());
                            var PIIApiClient = new TextAnalyticsClient(piiApiEndpointURI, piiApiCredential);
                            await ExtractSavePIIRelatedInfo(PIIApiClient, ocrResultDTO.DetectedText, newClaim.id.ToString());
                            Console.WriteLine("generated corelatingId is" + newClaim.id);
                            ocrResultDTO.GeneratedId = newClaim.id;
                        }
                        catch (Exception ex)
                        {
                            ocrResultDTO.DetectedText = "Unknown error occured.Please check your image size and try again.";
                        }
                    }
                }
                Console.WriteLine($"  OCR: {ocrResultDTO.DetectedText} ");
                return ocrResultDTO;
                
            }
            catch
            {
                ocrResultDTO.DetectedText = "Error occurred. Try again";
                ocrResultDTO.Language = "unk";
                return ocrResultDTO;
            }
           
        }

        static async Task<string> ReadTextFromStream(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocrapisubscriptionKey);
                string requestParameters = "language=en"; //&detectOrientation=true";
                string uri = ocrAPIURI + "?" + requestParameters;
                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                string contentString = await response.Content.ReadAsStringAsync();
                string result = JToken.Parse(contentString).ToString();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        // method for extracting information from healthcare-related text 
        public async Task ExtractSaveHealthRelatedInfo(TextAnalyticsClient client, string document, string ocrVisionId)
        {
            List<Entity> result = new List<Entity>();
            EntityResult newEntityResult = new EntityResult();
            List<string> batchInput = new List<string>()
            {
                document
            };
            AnalyzeHealthcareEntitiesOperation healthOperation = await client.StartAnalyzeHealthcareEntitiesAsync(batchInput);
            await healthOperation.WaitForCompletionAsync();

            await foreach (AnalyzeHealthcareEntitiesResultCollection documentsInPage in healthOperation.Value)
            {
                Console.WriteLine($"Results of Azure Text Analytics for health async model, version: \"{documentsInPage.ModelVersion}\"");
                Console.WriteLine("");

                foreach (AnalyzeHealthcareEntitiesResult entitiesInDoc in documentsInPage)
                {
                    if (!entitiesInDoc.HasError)
                    {
                        foreach (var entity in entitiesInDoc.Entities)
                        {
                            Entity newEntity = new Entity();
                           // if (!string.IsNullOrEmpty(entity.SubCategory))
                                newEntity.Text = entity.Text;
                            HealthcareEntityCategory category = entity.Category;
                            newEntity.Category = category.ToString();
                          //  newEntity.Offset = entity.Offset;
                          //  newEntity.Length = entity.Length;
                          //  newEntity.NormalizedText = entity.NormalizedText;
                            newEntity.ConfidenceScore = entity.ConfidenceScore;
                            result.Add(newEntity);
                            Console.WriteLine($"  HealthEntity: {entity.Text},Category: {entity.Category},Confidence score: {entity.ConfidenceScore} ");
                            
                        }
                        newEntityResult.entities = result;
                        newEntityResult.correlatingId = ocrVisionId;
                    }
                }
            }
            await _healthRepository.PostEntity(newEntityResult);

        }
        //method for detecting sensitive information (PII) from text 
        public async Task ExtractSavePIIRelatedInfo(TextAnalyticsClient client, string document, string ocrVisionId)
        {
            List<PII> resultPII = new List<PII>();
            PIIResult newPIIResult = new PIIResult();
            List<string> batchInput = new List<string>()
            {
                document
            };
            PiiEntityCollection entities = client.RecognizePiiEntities(document).Value;
            if (entities.Count > 0)
            {
                foreach (PiiEntity entity in entities)
                {
                    PII newPII = new PII();
                    newPII.Text = entity.Text;
                    PiiEntityCategory category = entity.Category;
                    newPII.Category = category.ToString();
                   // newPII.Offset = entity.Offset;
                   // newPII.Length = entity.Length;
                    //newPII.NormalizedText = entity.NormalizedText;
                    newPII.ConfidenceScore = entity.ConfidenceScore;
                    resultPII.Add(newPII);
                    Console.WriteLine($"  PIIEntity: {entity.Text},Category: {entity.Category},Confidence score: {entity.ConfidenceScore} ");
                    
                }
                newPIIResult.PIIEntities = resultPII;
                newPIIResult.correlatingId = ocrVisionId;
            }           
            await _PIIRepository.PostPII(newPIIResult);
        }
    }
}







