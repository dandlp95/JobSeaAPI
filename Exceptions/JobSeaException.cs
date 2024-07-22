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
        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(base.Message))
                {
                    return $"[{(int)_statusCode}] {base.Message}";
                }
                else
                {
                    return $"[{(int)_statusCode}] {DefaultMessage()}";
                }
            }
        }

        private string DefaultMessage()
        {
            switch (_statusCode)
            {
                case HttpStatusCode.NotFound:
                    return "Resource not found.";
                case HttpStatusCode.Unauthorized:
                    return "You are not authorized to access this resource.";
                case HttpStatusCode.BadRequest:
                    return "Invalid request.";
                case HttpStatusCode.Conflict:
                    return "Request could not be processed because of a conflict in the request.";
                case HttpStatusCode.InternalServerError:
                    return "An internal server error occurred.";
                default:
                    return "An error occurred.";
            }
        }
    }
}