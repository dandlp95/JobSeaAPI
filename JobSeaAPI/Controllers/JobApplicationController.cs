using AutoMapper;
using JobSeaAPI.Exceptions;
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
    [Route("jobSea")]
    public class JobApplicationController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobApplicationsRepository _applicationsRepo;
        private readonly IStatusRepository _statusRepository;
        protected APIResponse _response;
        private readonly IUpdateRepository _updateRepository;
        private readonly IExceptionHandler _exceptionHandler;
        public JobApplicationController(IMapper mapper, IHttpContextAccessor httpContextAccessor,  ITokenService tokenService, IJobApplicationsRepository applicationsRepository, 
               IStatusRepository statusRepository, IUpdateRepository updateRepository, IExceptionHandler exceptionHandler)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
            _response = new ();
            _statusRepository = statusRepository;
            _updateRepository = updateRepository;
            _exceptionHandler = exceptionHandler;
        }

        [HttpGet("users/{userId}/applications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]

        public ActionResult<APIResponse> GetApplications(int userId)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userId, _response);
                if(actionResult is not null) return actionResult;

                List<Application> applications = _applicationsRepo.GetAllApplications(userId);
                List<ApplicationDTO> applicationsDTO = _mapper.Map<List<ApplicationDTO>>(applications);
                _response.Result = applicationsDTO;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK; 
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch(Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
        }

        [HttpGet("users/{userId}/applications/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]

        public async Task<ActionResult<APIResponse>> GetApplication(int userId, int applicationId)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userId, _response);
                if (actionResult is not null) return actionResult;

                Application application = _applicationsRepo.GetApplication(applicationId, userId);
                ApplicationDTO applicationDTO = _mapper.Map<ApplicationDTO>(application);
                _response.Result = application;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
        }

        [HttpGet("users/{userId}/applications/{applicationId}/updates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> GetUpdates(int applicationId, int userId)
        {
            try
            {
                Claim? tokenUserId = User.FindFirst("userId");
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(tokenUserId, userId, _response);
                if (actionResult is not null) return actionResult;

                List<Update> updates = _updateRepository.GetUpdates(userId, applicationId);
                List<UpdateDTO> updatesDTO = _mapper.Map<List<UpdateDTO>>(updates);
                _response.Result = updatesDTO;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }

        }

        [HttpPost("users/{userId}/applications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> CreateApplication(int userId, [FromBody] CreateApplicationDTO newApplication)
        {
            try
            {
                Claim? tokenUserId = User.FindFirst("userId");
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(tokenUserId, userId, _response);
                if (actionResult is not null) return actionResult;

                Application application = await _applicationsRepo.CreateApplication(newApplication, userId);
                ApplicationDTO applicationDTO = _mapper.Map<ApplicationDTO>(application);

                _response.Result = applicationDTO;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }

        }

        [HttpDelete("users/{userId}/applications/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> DeleteApplication(int applicationId, int userId)
        {
            try
            {
                Claim? userIdClaim = User.FindFirst("userId");

                ActionResult actionResult = _tokenService.tokenValidationResponseAction(userIdClaim, userId, _response);
                if (actionResult is not null) return actionResult;

                await _applicationsRepo.DeleteApplication(applicationId, userId);
                return NoContent();
            }
            catch (JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
        }

        [HttpPut("users/{userId}/applications/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateJobApplication(int applicationId, int userId, [FromBody] UpdateApplicationDTO applicationDTO)
        {
            try
            {
                Claim? userIdClaim = User.FindFirst("userId");

                ActionResult actionResult = _tokenService.tokenValidationResponseAction(userIdClaim, userId, _response);
                if (actionResult is not null) return actionResult;

                Application application = await _applicationsRepo.UpdateApplication(applicationDTO, applicationId, userId);
                ApplicationDTO updatedApplication = _mapper.Map<ApplicationDTO>(application);

                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Errors = null;
                _response.Result = updatedApplication;

                return Ok(_response);
            }
            catch (JobSeaException ex) 
            { 
                return _exceptionHandler.returnExceptionResponse(ex, _response); 
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
        }
    }
}
