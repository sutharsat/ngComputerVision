using Azure;
using Azure.AI.TextAnalytics;
using Azure.Core;
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
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController
    {
       
       
        private readonly IOCRRepository _ocrRepository;
        private readonly ISearchRepository _searchRepository;
        

        public SearchController(IOCRRepository ocrRepository, ISearchRepository searchRepository)
        {

            _ocrRepository = ocrRepository;
            _searchRepository = searchRepository;
           
            //OCR Image to Text Detection API
           

        }

        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<ActionResult<ResultDTO>> Get(string id)
        //{
        //    newSearch.claimimage = imageFileBytes;
        //    await _searchRepository.PostSearch(newSearch);
        //}
        }
}
   

