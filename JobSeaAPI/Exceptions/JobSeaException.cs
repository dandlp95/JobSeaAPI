using System.Net;

namespace JobSeaAPI.Exceptions
{
    public class JobSeaException : Exception
    {
        protected readonly HttpStatusCode _statusCode;
        public HttpStatusCode StatusCode { get { return _statusCode; } }
        public JobSeaException(HttpStatusCode statusCode, string message) : base(message)
        {
            _statusCode = statusCode;
        }
    }
}