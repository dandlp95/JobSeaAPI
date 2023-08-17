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
        private readonly IStatusRepository _statusRepository;
        protected APIResponse _response;
        private readonly IUserRepository _userRepository;
        private readonly IUpdateRepository _updateRepository;

        public UpdatesController(IMapper mapper, ILoggerCustom logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ITokenService tokenService, 
            IJobApplicationsRepository applicationsRepository,IStatusRepository statusRepository, IUserRepository userRepository, IUpdateRepository updateRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _applicationsRepo = applicationsRepository;
            _userRepository = userRepository;
            _response = new();
            _statusRepository = statusRepository;
            _updateRepository = updateRepository;
        }
        [HttpPost("Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> CreateUpdate([FromBody] UpdateCreateDTO updateDTO)
        {
            Application application = _applicationsRepo.GetApplication(updateDTO.ApplicationId);
            if(application is null)
            {
                _response.Result = null;
                _response.Errors = new List<string>() { "Application Id doesn't match any current applications." };
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

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

        [HttpPut("Updates/{updateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> UpdateUpdate(int updateId, [FromBody] UpdateUpdateDTO updateDTO)
        {
            try
            {
                int userId = getUpdateUserId(updateId);
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userId, _response);
                if (actionResult is not null) return actionResult;
                Update update = await _updateRepository.UpdateUpdate(updateDTO);
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
              
                Exception? innerException = ex.InnerException;

                _response.Result = null;
                _response.Errors = new List<string>() { ex.Message, innerException.ToString() };
                _response.StatusCode = ex.StatusCode;
                _response.Result = null;
                _response.Token = null;

                return StatusCode((int)ex.StatusCode, _response);
                
            }

        }


        [HttpDelete("DeleteUpdate/{updateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> DeleteUpdate(int updateId, [FromBody] UpdateUpdateDTO updateDTO)
        {

            Update updateToDelete = _updateRepository.GetUpdate(updateId);
            int applicationId = updateToDelete.ApplicationId;
            Application application = _applicationsRepo.GetApplication(applicationId);

            ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), application.UserId, _response);
            if (actionResult is not null) return actionResult;

            await _updateRepository.DeleteUpdate(updateToDelete);
            return NoContent();
        }

        private int getUpdateUserId(int updateId)
        {
            Update? update = _updateRepository.GetUpdate(updateId) ?? throw new JobSeaException(System.Net.HttpStatusCode.BadRequest, "Update Id does not match any entity in the database.");
            Application? application = _applicationsRepo.GetApplication(update.ApplicationId);
            return application.UserId;
        }
    }
}
