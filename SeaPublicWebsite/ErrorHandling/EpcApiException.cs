using System;

namespace SeaPublicWebsite.ErrorHandling
{
    public class EpcApiException: Exception
    {
        public EpcApiException(string message) : base(message)
        {
        }
    }
}