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
        private readonly IUserRepository _userRepository;
        public JobApplicationController(IMapper mapper, ILoggerCustom logger, IHttpContextAccessor httpContextAccessor, 
               IConfiguration configuration, ITokenService tokenService, IJobApplicationsRepository applicationsRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
            _userRepository = userRepository;
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
                List<Application> applications = new();
                int userId = _tokenService.ValidateUserIdToken(User.FindFirst("userId"));

                if (userId < 0)
                {
                    _response.Result = null;
                    _response.Errors = new List<string>() { "Invalid user request." };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                applications = _applicationsRepo.GetAllApplications(userId);
                _response.Result = applications;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK; 
                _response.IsSuccess = true;
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetApplicationUpdates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetUpdates()
        {
            throw new NotImplementedException();
        }

        [HttpPost("CreateApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> CreateApplication()
        {
            throw new NotImplementedException();
        }

    }
}
