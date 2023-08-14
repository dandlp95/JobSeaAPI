using AutoMapper;
using Azure;
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
        [HttpPost("AddUpdate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> CreateUpdate([FromBody] UpdateCreateDTO updateDTO)
        {
            Claim userIdClaim = User.FindFirst("userId");
            Application application = _applicationsRepo.GetApplication(updateDTO.ApplicationId);
            if(application is null)
            {
                _response.Result = null;
                _response.Errors = new List<string>() { "Application Id doesn't match any current applications." };
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            int userId = _tokenService.ValidateUserIdToken(userIdClaim, application.UserId);

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

            Update newUpdate = await _updateRepository.CreateUpdate(updateDTO);
            UpdateDTO newUpdateDTO = _mapper.Map<UpdateDTO>(newUpdate);
            _response.Result = newUpdateDTO;
            _response.Errors = new List<string>();
            _response.StatusCode = System.Net.HttpStatusCode.Created;
            _response.IsSuccess = true;

            return StatusCode(StatusCodes.Status201Created, _response);
        }

        [HttpPut("UpdateUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> UpdateUpdate([FromBody] UpdateUpdateDTO updateDTO)
        {
            //Claim userIdClaim = User.FindFirst("userId");
            //Application application = _applicationsRepo.GetApplication(updateDTO.ApplicationId);
            //if (application is null)
            //{
            //    _response.Result = null;
            //    _response.Errors = new List<string>() { "Application Id doesn't match any current applications." };
            //    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    _response.IsSuccess = false;
            //    return BadRequest(_response);
            //}
            //int userId = _tokenService.ValidateUserIdToken(userIdClaim, application.UserId);

            //if (userId == 0)
            //{
            //    _response.Result = null;
            //    _response.Errors = new List<string>() { "User not found." };
            //    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            //    _response.IsSuccess = false;
            //    return NotFound(_response);
            //}
            //else if (userId == -1)
            //{
            //    return Forbid();
            //}
            

            throw new NotImplementedException();

        }


        [HttpDelete("DeleteApplication/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy ="User")]
        public async Task<ActionResult<APIResponse>> DeleteUpdate(int updateId)
        {
            Claim userIdClaim = User.FindFirst("userId");
            Update updateToDelete = _updateRepository.GetUpdate(updateId);
            int applicationId = updateToDelete.ApplicationId;

            Application application = _applicationsRepo.GetApplication(applicationId);
            int userId = _tokenService.ValidateUserIdToken(userIdClaim, application.UserId);

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
            await _updateRepository.DeleteUpdate(updateToDelete);
           
        }

    }
    // TO DO
    // There is lots of repetitive code. Particularly having to do with auth, create a function to take care of it.
}
