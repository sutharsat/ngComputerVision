using Microsoft.AspNetCore.Mvc;
using ngComputerVision.Contracts.Entities;
using ngComputerVision.Core.Data;
using ngComputerVision.DTOModels;
using ngComputerVision.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ngComputerVision.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IPIIRepository _PIIRepository;
        private readonly IEntityRepository _entityRepository;

        public ClaimController(IClaimRepository claimRepository, IEntityRepository entityRepository, IPIIRepository pIIRepository) 
          { 
            _PIIRepository = pIIRepository;
        _entityRepository = entityRepository;
            _PIIRepository = pIIRepository;
            }
        [HttpGet]
        // public async Task<List<Claims>> Get() =>
        // await _claimRepository.GetClaims();
        /* [HttpGet("{id:length(24)}")]
         public async Task<ActionResult<ClaimDTO>> Get(string id)
         {
             ClaimDTO claimDTO = new ClaimDTO();
             var claim = await _PIIRepository.GetPIIResultWithId(id);
             if (claim is null)
             {
                 return NotFound();
             }
             claimDTO.firstname = claim.firstname;
             claimDTO.lastname = claim.lastname;
             claimDTO.dateofbirth=claim.dateofbirth;
             claimDTO.address=claim.address;
             claimDTO.gender= claim.gender;
             claimDTO.medicareID=claim.medicareID;
            claimDTO.phonenumber = claim.phonenumber;
             claimDTO.npi = claim.npi;

             claimDTO.ptan=claim.ptan;


             claimDTO.hospitalname = claim.hospitalname;
             return claimDTO;
         }*/

        [HttpPost("/create")]
        public async Task<IActionResult> Post(Claims newClaim)
        {

            if (newClaim != null)
            {
                await _claimRepository.PostClaim(newClaim);
                return Ok("Claim saved");
                { };

            }
            else
            {
                return BadRequest("An Error Has Occured");
            }
        }
            [HttpGet("{id:length(24)}")]
            public async Task<ActionResult<ResultDTO>> Get(string id)
            {
                ResultDTO resultDTO = new ResultDTO();
                var entityResult =  _entityRepository.GetEntityWithId(id);
              /*  if (claim is null)
                {
                    return NotFound();
                }
                claimDTO.firstname = claim.firstname;
                claimDTO.lastname = claim.lastname;
                claimDTO.dateofbirth = claim.dateofbirth;
                claimDTO.address = claim.address;
                claimDTO.gender = claim.gender;
                claimDTO.medicareID = claim.medicareID;
                claimDTO.phonenumber = claim.phonenumber;
                claimDTO.npi = claim.npi;

                claimDTO.ptan = claim.ptan;


                claimDTO.hospitalname = claim.hospitalname;
              */
                return resultDTO;
            }



        

        /*[HttpPost]
         public async Task Post() =>
           

        //create new claims here named my claim
         await _claimRepository.PostClaim();*/



    }
}


