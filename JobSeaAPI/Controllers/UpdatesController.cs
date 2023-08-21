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
    [Route("jobSea")]
    [ApiController]
    public class UpdatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerCustom _logger;
        private readonly ITokenService _tokenService;
        protected APIResponse _response;
        private readonly IUpdateRepository _updateRepository;
        private readonly IExceptionHandler _exceptionHandler;

        public UpdatesController(IMapper mapper, ILoggerCustom logger, ITokenService tokenService, IUpdateRepository updateRepository,
            IExceptionHandler exceptionHandler)
        {
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _response = new();
            _updateRepository = updateRepository;
            _exceptionHandler = exceptionHandler;
        }
        [HttpPost("users/{userId}/applications/{applicationId}/updates")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> CreateUpdate([FromBody] UpdateCreateDTO updateDTO, int applicationId, int userId)
        {
            try
            {
                ActionResult actionResult = _tokenService.tokenValidationResponseAction(User.FindFirst("userId"), userId, _response);
                if (actionResult is not null) return actionResult;

                
                Update newUpdate = await _updateRepository.CreateUpdate(updateDTO, applicationId);
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

        [HttpDelete("users/{userId}/applications/{applicationId}/updates/{updateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<APIResponse>> DeleteUpdate(int updateId, int applicationId, int userId)
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
