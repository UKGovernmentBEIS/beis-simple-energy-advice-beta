using System;

namespace SeaPublicWebsite.ErrorHandling
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}