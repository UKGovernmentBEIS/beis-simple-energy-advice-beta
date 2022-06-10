using System;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.ErrorHandling
{

    public abstract class CustomErrorPageException : Exception
    {
        public abstract string ViewName { get; }
        public abstract int StatusCode { get; }
    }


    public class UserReferenceNotFoundException : CustomErrorPageException
    {
        public override string ViewName => "../Error/UserReferenceNotFound";
        public override int StatusCode => 404;
        public string Reference { get; set; }
    }

}
