using AutoMapper;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace JobSeaAPI.Controllers
{
    [ApiController]
    [Route("jobSea/[Controller]")]
    public class JobApplicationController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerCustom _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobApplicationsRepository _applicationsRepo;
        protected APIResponse _response;
        
        public JobApplicationController(IMapper mapper, ILoggerCustom logger, IHttpContextAccessor httpContextAccessor, 
               IConfiguration configuration, ITokenService tokenService, IJobApplicationsRepository applicationsRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
        }

        // Gets Applications for a specific user, based on specific criteria
        [HttpGet("GetAllApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy="User")]

        public async Task<ActionResult<APIResponse>> GetApplications()
        {
            try
            {
                Claim userIdClaim = User.FindFirst("userId");
                List<Application> applications = new();

                if (Int32.TryParse(userIdClaim?.Value, out int userId))
                {
                    applications = _applicationsRepo.GetAllApplications(userId);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string>()
                    {
                        "User Id is not valid."
                    };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.Result = applications;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
