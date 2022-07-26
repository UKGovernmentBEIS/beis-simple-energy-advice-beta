using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeaPublicWebsite.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate next;
    
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");

        return next(context);
    }
}