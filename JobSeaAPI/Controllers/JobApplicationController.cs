using AutoMapper;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
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
            _response = new ();
        }

        // Gets Applications for a specific user, based on specific criteria
        [HttpGet("GetAllApplications/{userIdRequest}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]

        public async Task<ActionResult<APIResponse>> GetApplications(int userIdRequest)
        {
            try
            {
               
                int userId = _tokenService.ValidateUserIdToken(User.FindFirst("userId"), userIdRequest);

                if (userId == 0)
                {
                    _response.Result = null;
                    _response.Errors = new List<string>() { "User not found." };
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else if (userId == -1)
                {
                    return Forbid("You do not have access to this user's information.");
                }
                List<Application> applications = _applicationsRepo.GetAllApplications(userId);
                _response.Result = applications;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK; 
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetApplicationUpdates/{applicationId}/{userIdRequest}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> GetUpdates(int applicationId, int userIdRequest)
        {
            int userId = _tokenService.ValidateUserIdToken(User.FindFirst("userId"), userIdRequest);
            if(userId == 0)
            {
                _response.Result = null;
                _response.Errors = new List<string>() { "User not found." };
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            else if (userId == -1)
            {
                return Forbid("You do not have access to this user's information.");
            }

            List<Update> updates = _applicationsRepo.GetAllUpdates(userIdRequest, applicationId);
            List<UpdateDTO> updatesDTO = _mapper.Map<List<UpdateDTO>>(updates);
            _response.Result = updatesDTO;
            _response.Errors = null;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("CreateApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDTO newApplication)
        {
            try
            {
                Claim userIdClaim = User.FindFirst("userId");
                int userId = _tokenService.ValidateUserIdToken(userIdClaim, newApplication.UserId);
                if (userId == 0)
                {
                    _response.Result = null;
                    _response.Errors = new List<string>() { "User not found." };
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else if (userId == -1)
                {
                    return Forbid();
                }
                _response.Result = _applicationsRepo.CreateApplication(newApplication);
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (DbUpdateException ex)
            {
                _response.Result = null;
                _response.Errors = new List<string>() { ex.InnerException.ToString(),"Error fulfilling this request." };
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }

        [HttpGet("GetStatusOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStatusOptions()
        {
            try
            {
                List<Status> statuses = _applicationsRepo.GetStatuses();
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Errors = null;
                _response.Result = statuses;
                return Ok(_response);
            }
            catch (DbUpdateException ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.InnerException.ToString() };
                _response.Result= null;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

    }
}
