using AutoMapper;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobSeaAPI.Controllers
{
    [ApiController]
    [Route("jobSea/user")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _dbUser;
        private readonly ILoggerCustom _logger;
        protected APIResponse _response;

        public UserController(IMapper mapper, IUserRepository dbUser, ILoggerCustom logger)
        {
            _mapper = mapper;
            _dbUser = dbUser;
            _logger = logger;
            _response = new();
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
    }
}
