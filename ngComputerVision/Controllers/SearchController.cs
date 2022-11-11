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
    public class SearchController : Controller
    {

        private readonly IOCRRepository _ocrRepository;
        private readonly IHealthRepository _healthRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IPIIRepository _PIIRepository;
        private readonly ISearchRepository _searchRepository;


        public SearchController(IOCRRepository ocrRepository, IHealthRepository healthRepository, ICredentialRepository credentialRepository, IPIIRepository PIIRepository, ISearchRepository searchRepository)
        {
            _ocrRepository = ocrRepository;
            _healthRepository = healthRepository;
            _credentialRepository = credentialRepository;
            _PIIRepository = PIIRepository;
            _searchRepository = searchRepository;

        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<string> PostAsync()
        {
            String piiFormData = Request.Form["data"];
            string msg = "";


            try
            {
                SearchValueDTO? piiFields = System.Text.Json.JsonSerializer.Deserialize<SearchValueDTO>(piiFormData);
                Search saveSearchValue = new Search();
                saveSearchValue.person = piiFields.person;
                saveSearchValue.phoneNumber = piiFields.phoneNumber;
                saveSearchValue.address = piiFields.address;
                saveSearchValue.email = piiFields.email;
                saveSearchValue.organization = piiFields.organization;
                saveSearchValue.claimId = piiFields.claimId;
                saveSearchValue.dateTime = piiFields.dateTime;
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
                            saveSearchValue.searchImageValue = imageFileBytes;
                            await _searchRepository.PostSearch(saveSearchValue);
                            msg = "claim has been approved and saved successfully.";
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("exception occured while saving data" + ex.Message);
                            msg = "Exception occured while saving data";
                        }
                    }
                }

                return msg;

            }
            catch (Exception ex)
            {
                //searchValueDTO.person = "Error occurred. Try again";
                //searchValueDTO.person = "unk";

                Console.WriteLine(ex.Message);
                msg = "Invalid data";
                return msg;
            }

        }





        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SearchResultDTO>> Get(string id)
        {
            SearchResultDTO searchResultDTO = new SearchResultDTO();
            return searchResultDTO;
        }
    }
}







