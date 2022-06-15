using System;
using System.Net;

namespace SeaPublicWebsite.ErrorHandling
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode;

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}