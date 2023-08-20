using JobSeaAPI.Exceptions;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSeaAPI.Services
{
    public interface IExceptionHandler
    {
        ActionResult returnExceptionResponse(JobSeaException ex, APIResponse response);
        ActionResult returnExceptionResponse(Exception ex, APIResponse response);
    }
}
