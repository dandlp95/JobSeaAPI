using AutoMapper;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
        protected APIResponse _response;

        public UserController(IMapper mapper, IUserRepository dbUser, ILoggerCustom logger, IConfiguration configuration, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dbUser = dbUser;
            _logger = logger;
            _response = new();
            _configuration = configuration;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> users = await _dbUser.GetAllAsync();
                _response.Result = _mapper.Map<List<UserDTO>>(users);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "error");
                _response.Result = ex;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpGet("GetUserById/{id}")]
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
                User fetchedUser = await _dbUser.GetAsync(u => u.UserId == id);
                if (fetchedUser == null)
                {
                    return NotFound(_response);
                }
                UserDTO userDTO = _mapper.Map<UserDTO>(fetchedUser);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddUser([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                User user = _mapper.Map<User>(userCreateDTO);
                await _dbUser.CreateAsync(user);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.Token = _tokenService.GetToken(user.Username);

                return CreatedAtRoute(nameof(GetUserById), new { id = user.UserId }, _response);
            }
            catch (JobSeaException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginUser userInfo)
        {

            var userInfo2 = userInfo;
            UserDTO? authenticatedUser = await _dbUser.Authenticate(userInfo.Username, userInfo.password);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }
            string userToken = _tokenService.GetToken(userInfo.Username);
            return Ok(userToken);
        }
    }
}
