using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobSeaAPI.Controllers
{
    [ApiController]
    [Route("jobSea/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _dbUser;
        private readonly ILoggerCustom _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        protected APIResponse _response;
        private IExceptionHandler _exceptionHandler;

        public UserController(IMapper mapper, IUserRepository dbUser, ILoggerCustom logger, IConfiguration configuration, ITokenService tokenService, 
               IHttpContextAccessor httpContextAccessor, ApplicationDbContext db, IExceptionHandler exceptionHandler)
        {
            _db = db;
            _mapper = mapper;
            _dbUser = dbUser;
            _logger = logger;
            _response = new();
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _exceptionHandler = exceptionHandler;
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> users = _dbUser.GetAllUsers();
                _response.Result = _mapper.Map<List<UserDTO>>(users);
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpGet("users/{id}")]
        [HttpGet("id", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUserById(int id)
        {
            try
            {
                if (id < 1)
                {
                    _logger.Log("Id is invalid", "error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                User fetchedUser = _dbUser.GetUser(id);
                if (fetchedUser == null)
                {
                    return NotFound(_response);
                }
                UserDTO userDTO = _mapper.Map<UserDTO>(fetchedUser);
                return Ok(userDTO);
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

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddUser([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                User? user = _mapper.Map<User>(userCreateDTO);
                await _dbUser.CreateUser(user);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                UserDTO userInfoResponse = _mapper.Map<UserDTO>(user);
                _response.Result = userInfoResponse;
                _response.Token = _tokenService.GetToken(user.Username, user.UserId);

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


        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<APIResponse> Login([FromBody] LoginUser userInfo)
        {
            try
            {
                UserDTO? authenticatedUser = _dbUser.Authenticate(userInfo.Username, userInfo.password);
                if (authenticatedUser is null)
                {
                    return Unauthorized();
                }
                string userToken = _tokenService.GetToken(authenticatedUser.Username, authenticatedUser.UserId);
                _response.IsSuccess = true;
                _response.Token = userToken;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Errors = null;
                _response.Result = authenticatedUser;

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

        [HttpDelete("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteUser(int userId)
        {
            try
            {
                await _dbUser.DeleteUser(userId);
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

        private bool IsDuplicateEntryError(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx &&
                (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        }
    }
}
