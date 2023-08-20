using Azure;
using JobSeaAPI.Exceptions;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSeaAPI.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        public ExceptionHandler() { }
        public ActionResult returnExceptionResponse(JobSeaException ex, APIResponse response)
        {
            Exception? innerException = ex.InnerException;

            response.Result = null;
            response.Errors = new List<string>() { ex.Message, innerException.ToString() };
            response.StatusCode = ex.StatusCode;
            response.Result = null;
            response.Token = null;

            return new ObjectResult(response)
            {
                StatusCode = (int)ex.StatusCode
            };
        }
    }
}
