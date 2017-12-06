using System.Net.Http;
using System.Net;
using NetlifySharp.Models;

namespace NetlifySharp.Operations
{
    public class ErrorResponseException : HttpRequestException
    {
        public Error Error { get; }

        public ErrorResponseException(Error error, HttpStatusCode statusCode)
            : base($"Unexpected status code of {(int)statusCode} ({statusCode})"
                  + (error == null || error.Message == null ? string.Empty : $": {error.Message}"))
        {
            Error = error;
        }
    }
}