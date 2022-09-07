using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;
using ngComputerVision.Models;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using ngComputerVision.DTOModels;
using ngComputerVision.Repository;
using ngComputerVision.Contracts.Entities;
using ngComputerVision.Repository.Interface;
using Azure.AI.TextAnalytics;
using Azure;

namespace ngComputerVision.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OCRController : Controller
    {
        static string subscriptionKey;
        static string endpoint;
        static string uriBase;
        static string analyticCredentials;
        static string analyticEndpoint2;
        private readonly IClaimRepository _claimRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IPIIRepository _PIIRepository;
        private string endpoint3;

        //endpoint connection for text analytics api
        private static AzureKeyCredential credentials;
        private AzureKeyCredential credentialPII;
        private static Uri endpoint2;
        private static Uri endpointPII;
        //private static Uri endpoint3;
        private string analyticuriBase;
        private string azureKeyPIICredential;
        private string azurePIIEndpoint;

        public OCRController(IClaimRepository claimRepository, IEntityRepository entityRepository, ICredentialRepository credentialRepository, IPIIRepository PIIRepository)
        {



            _claimRepository = claimRepository;
            _entityRepository = entityRepository;
            _credentialRepository = credentialRepository;
            _PIIRepository = PIIRepository;
            //OCR API
            subscriptionKey = _credentialRepository.GetCredential("OCRAPI").subscriptionKey.ToString();
            endpoint = _credentialRepository.GetCredential("OCRAPI").endpoint.ToString();
            uriBase = endpoint + "vision/v3.2/read/syncAnalyze";

            //Text Analytic API
            analyticCredentials = _credentialRepository.GetCredential("ANALYTICAPI").subscriptionKey.ToString();
            analyticEndpoint2 = _credentialRepository.GetCredential("ANALYTICAPI").endpoint.ToString();
            analyticuriBase = analyticEndpoint2 + "/language/analyze-text/jobs";

            //PII Detect API
            azureKeyPIICredential = _credentialRepository.GetCredential("PIIAPI").subscriptionKey.ToString();
            azurePIIEndpoint = _credentialRepository.GetCredential("PIIAPI").endpoint.ToString();


            credentials = new AzureKeyCredential(analyticCredentials);
            credentialPII = new AzureKeyCredential(azureKeyPIICredential);
            endpoint2 = new Uri(analyticEndpoint2);
            endpointPII = new Uri(azurePIIEndpoint);

        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<OcrResultDTO> Post()
        {
            string ocrText = "";
            string PII_id = "";

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
                            result = JsonConvert.DeserializeObject<Claims>(myJSONResult);
                            //call the ocr repository's Create method here and pass the result to that method. that will save the result in database.

                            Claims? newClaim = System.Text.Json.JsonSerializer.Deserialize<Claims>(myJSONResult);



                            foreach (var res in result.analyzeResult.readResults)
                            {
                                foreach (var lines in res.lines)
                                {
                                    ocrText = ocrText + " " + lines.text;




                                }
                            }


                            /* JObject json = JObject.Parse(ocrText);*/


                            await _claimRepository.PostClaim(newClaim);

                            ocrResultDTO.DetectedText = ocrText.ToString();
                            ocrResultDTO.Language = "en";
                            // string firstNamekey;
                            var client = new TextAnalyticsClient(endpoint2, credentials);
                            await healthExample(client, ocrResultDTO.DetectedText);
                             var clients = new TextAnalyticsClient(endpointPII, credentialPII);
                            PII_id = await RecognizePIIExample(clients, ocrResultDTO.DetectedText);
                            Console.WriteLine("generated id is" + PII_id);
                            ocrResultDTO.GeneratedId = PII_id;
                        }
                        catch (Exception ex)
                        {
                            ocrResultDTO.DetectedText = "Unknown error occured.Please check your image size and try again.";
                        }
                    }
                }
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
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=en"; //&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;
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

        [HttpGet]
        public async Task<List<AvailableLanguageDTO>> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";
            var client = new HttpClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(endpoint);
                var response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();

                AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);

                List<AvailableLanguageDTO> availableLanguage = new List<AvailableLanguageDTO>();

                foreach (KeyValuePair<string, LanguageDetails> translation in deserializedOutput.Translation)
                {
                    AvailableLanguageDTO language = new AvailableLanguageDTO();
                    language.LanguageID = translation.Key;
                    language.LanguageName = translation.Value.Name;

                    availableLanguage.Add(language);
                }
                return availableLanguage;
            }
        }
        // method for extracting information from healthcare-related text 
        public async Task healthExample(TextAnalyticsClient client, string document)
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
                            // view recognized healthcare entities
                            Console.WriteLine($"  Entity: {entity.Text}");
                            Console.WriteLine($"  Category: {entity.Category}");
                            Console.WriteLine($"  Offset: {entity.Offset}");
                            Console.WriteLine($"  Length: {entity.Length}");
                            Console.WriteLine($"  NormalizedText: {entity.NormalizedText}");
                            if (!string.IsNullOrEmpty(entity.SubCategory))
                                Console.WriteLine($"  SubCategory: {entity.SubCategory}");
                            Console.WriteLine($"  Confidence score: {entity.ConfidenceScore}");
                            Console.WriteLine("");
                            newEntity.Text = entity.Text;
                            HealthcareEntityCategory category = entity.Category;
                            newEntity.Category = category.ToString();
                            newEntity.Offset = entity.Offset;
                            newEntity.Length = entity.Length;
                            newEntity.NormalizedText = entity.NormalizedText;
                            newEntity.ConfidenceScore = entity.ConfidenceScore;
                            result.Add(newEntity);
                        }
                        newEntityResult.entities = result;
                        Console.WriteLine($"  Found {entitiesInDoc.EntityRelations.Count} relations in the current document:");
                        Console.WriteLine("");


                    }
                    else
                    {
                        Console.WriteLine("  Error!");
                        Console.WriteLine($"  Document error code: {entitiesInDoc.Error.ErrorCode}.");
                        Console.WriteLine($"  Message: {entitiesInDoc.Error.Message}");
                    }
                    Console.WriteLine("");
                }

            }
            await _entityRepository.PostEntity(newEntityResult);

        }
        // Example method for detecting sensitive information (PII) from text 
        public async Task<string> RecognizePIIExample(TextAnalyticsClient client,string document)
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
                 Console.WriteLine($"Recognized {entities.Count} PII entit{(entities.Count > 1 ? "ies" : "y")}:");
                 foreach (PiiEntity entity in entities)
                 {
                    PII newPII = new PII();
                    newPII.Text = entity.Text;
                    PiiEntityCategory category = entity.Category;
                    newPII.Category = category.ToString();
                    newPII.Offset = entity.Offset;
                    newPII.Length = entity.Length;
                    //newPII.NormalizedText = entity.NormalizedText;
                    newPII.ConfidenceScore = entity.ConfidenceScore;
                    resultPII.Add(newPII);
                    
                 }
                newPIIResult.PIIEntities = resultPII;
            }
             else
             {
                 Console.WriteLine("No entities were found.");
             }
            await _PIIRepository.PostPII(newPIIResult);
            return newPIIResult.id.ToString();
        }

         
    }
}
    



    


