using AutoMapper;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata.Ecma335;
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
        private readonly IStatusRepository _statusRepository;
        protected APIResponse _response;
        private readonly IUserRepository _userRepository;
        private readonly IUpdateRepository _updateRepository;
        public JobApplicationController(IMapper mapper, ILoggerCustom logger, IHttpContextAccessor httpContextAccessor, 
               IConfiguration configuration, ITokenService tokenService, IJobApplicationsRepository applicationsRepository, 
               IStatusRepository statusRepository,IUserRepository userRepository, IUpdateRepository updateRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
            _userRepository = userRepository;
            _response = new ();
            _statusRepository = statusRepository;
            _updateRepository = updateRepository;
        }

        // Gets Applications for a specific user, based on specific criteria
        [HttpGet("user/{userIdRequest}/applications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]

        public async Task<ActionResult<APIResponse>> GetApplications(int userIdRequest)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userIdRequest, _response);
                if(actionResult is not null) return actionResult;

                List<Application> applications = _applicationsRepo.GetAllApplications(userIdRequest);
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
        [HttpGet("user/{userIdRequest}/Application/{applicationId}/Updates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> GetUpdates(int applicationId, int userIdRequest)
        {
            ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userIdRequest, _response);
            if (actionResult is not null) return actionResult;

            List<Update> updates = _updateRepository.GetUpdates(userIdRequest, applicationId);
            List<UpdateDTO> updatesDTO = _mapper.Map<List<UpdateDTO>>(updates);
            _response.Result = updatesDTO;
            _response.Errors = null;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("Application")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDTO newApplication)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), newApplication.UserId, _response);
                if (actionResult is not null) return actionResult;

                _response.Result = await _applicationsRepo.CreateApplication(newApplication);
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

        [HttpDelete("Application/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> DeleteApplication(int applicationId)
        {
            try
            {
                Claim userIdClaim = User.FindFirst("userId");
                Application applicationToDelete = _applicationsRepo.GetApplication(applicationId);

                ActionResult actionResult = _tokenService.tokenValidationResponseAction(userIdClaim, applicationToDelete.UserId, _response);
                if (actionResult is not null) return actionResult;

                await _applicationsRepo.DeleteApplication(applicationId);
                return Ok();
            }
            catch(DbUpdateException ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.InnerException.ToString() };
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("StatusOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStatusOptions()
        {
            try
            {
                List<Status> statuses = _statusRepository.GetStatuses();
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

        [HttpPut("Applications/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateJobApplication(int applicationId, [FromBody] UpdateApplicationDTO applicationDTO)
        {
            try
            {
                Application application = await _applicationsRepo.UpdateApplication(applicationDTO, applicationId);
                ApplicationDTO updatedApplication = _mapper.Map<ApplicationDTO>(application);

                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Errors = null;
                _response.Result = updatedApplication;
                return Ok(_response);
            }
            catch (DbUpdateException ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.InnerException.ToString() };
                _response.Result = null;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
