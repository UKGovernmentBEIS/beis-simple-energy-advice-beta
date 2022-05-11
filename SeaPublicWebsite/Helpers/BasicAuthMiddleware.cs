﻿using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeaPublicWebsite.Helpers
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(new PathString("/health-check")))
            {
                await next.Invoke(httpContext);
                return;
            }

            // Add HTTP Basic Authentication in our non-production environments to make sure people don't accidentally stumble across the site
            // The site will still also be secured by the usual login/cookie auth - this is just an extra layer to make the site not publicly accessible
            if (IsAuthorised(httpContext))
            {
                await next.Invoke(httpContext);
            }
            else
            {
                SendUnauthorisedResponse(httpContext);
            }
        }

        private bool IsAuthorised(HttpContext httpContext)
        {
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(httpContext.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];

                if (Config.GetAppSetting("BasicAuthUsername") == username &&
                    Config.GetAppSetting("BasicAuthPassword") == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void SendUnauthorisedResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            AddOrUpdateHeader(httpContext, "WWW-Authenticate", "Basic realm=\"Gender Pay Gap service\"");
        }

        private static void AddOrUpdateHeader(HttpContext httpContext, string headerName, string headerValue)
        {
            if (httpContext.Response.Headers.ContainsKey(headerName))
            {
                httpContext.Response.Headers[headerName] = headerValue;
            }
            else
            {
                httpContext.Response.Headers.Add(headerName, headerValue);
            }
        }


    }
}
