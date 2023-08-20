using AutoMapper;
using Azure;
using Azure.Identity;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Security.Claims;

namespace JobSeaAPI.Controllers
{
    [Route("jobSea/[controller]")]
    [ApiController]
    public class UpdatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerCustom _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobApplicationsRepository _applicationsRepo;
        protected APIResponse _response;
        private readonly IUpdateRepository _updateRepository;
        private readonly IExceptionHandler _exceptionHandler;

        public UpdatesController(IMapper mapper, ILoggerCustom logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ITokenService tokenService, 
            IJobApplicationsRepository applicationsRepository, IUpdateRepository updateRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
            _response = new();
            _updateRepository = updateRepository;
        }
        [HttpPost("users/{userId}/applications/{applicationId}/updates")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> CreateUpdate([FromBody] UpdateCreateDTO updateDTO, int applicationId)
        {
            try
            {
                Application application = _applicationsRepo.GetApplication(applicationId) 
                    ?? throw new JobSeaException(System.Net.HttpStatusCode.BadRequest, "ApplicationId doesn't belong to any entities in the database");

                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), application.UserId, _response);
                if (actionResult is not null) return actionResult;

                Update newUpdate = await _updateRepository.CreateUpdate(updateDTO);
                UpdateDTO newUpdateDTO = _mapper.Map<UpdateDTO>(newUpdate);

                _response.Result = newUpdateDTO;
                _response.Errors = new List<string>();
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                _response.IsSuccess = true;

                return StatusCode(StatusCodes.Status201Created, _response);
            }
            catch(JobSeaException ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.returnExceptionResponse(ex, _response);
            }
        }

        [HttpPut("users/{userId}/applications/{applicationId}/updates/{updateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> UpdateUpdate(int updateId,int applicationId, int userId, [FromBody] UpdateUpdateDTO updateDTO)
        {
            try
            {
                Claim? tokenUserId = User.FindFirst("userId");

                ActionResult actionResult = _tokenService.tokenValidationResponseAction(tokenUserId, userId, _response);
                if (actionResult is not null) return actionResult;

                Update update = await _updateRepository.UpdateUpdate(updateDTO, updateId, applicationId, userId);
                UpdateDTO UpdatedUpdate = _mapper.Map<UpdateDTO>(update);

                _response.Result = UpdatedUpdate;
                _response.Errors = null;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Token = null;

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

        [HttpDelete("users/{UserId}/applications/{ApplicationId/updates/{updateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> DeleteUpdate(int updateId, int applicationId, int userId, [FromBody] UpdateUpdateDTO updateDTO)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userId, _response);
                if (actionResult is not null) return actionResult;

                await _updateRepository.DeleteUpdate(updateId, applicationId, userId);
                return NoContent();
            }
            catch(JobSeaException ex)
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
