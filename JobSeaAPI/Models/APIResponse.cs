﻿using System.Net;
using System.Security.Claims;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<String> Errors { get; set; }
        public object Result { get; set; }
    }
}
