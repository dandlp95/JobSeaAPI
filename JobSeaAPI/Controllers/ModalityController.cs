using AutoMapper;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSeaAPI.Controllers
{
    [Route("jobSea")]
    [ApiController]
    public class ModalityController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IModalityRepository _modalitiesRepo;
        private readonly IExceptionHandler _exceptionHandler;

        public ModalityController(IModalityRepository modalitiesRepo, IExceptionHandler exceptionHandler)
        {
            _response = new();
            _modalitiesRepo = modalitiesRepo;
            _exceptionHandler = exceptionHandler;
        }
        [HttpGet("modalities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetModalities()
        {
            try
            {
                List<Modality> modalities = _modalitiesRepo.GetModalities();
                _response.Result = modalities;
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

    }
}
