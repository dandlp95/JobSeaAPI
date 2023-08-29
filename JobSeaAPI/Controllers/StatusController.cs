using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSeaAPI.Controllers
{
    public class StatusController:ControllerBase
    {
        private readonly IStatusRepository _statusRepository;
        protected APIResponse _response;
        private readonly IExceptionHandler _exceptionHandler;
        public StatusController( IStatusRepository statusRepository, IExceptionHandler exceptionHandler)
        {
            _response = new();
            _statusRepository = statusRepository;
            _exceptionHandler = exceptionHandler;
        }

        [HttpGet("statusOptions")]
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
