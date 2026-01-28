using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SeaPublicWebsite.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<CorrelationIdMiddleware> logger;
    private const string CorrelationIdCookieName = "CorrelationId";
    private const string CorrelationIdItemKey = "CorrelationId";

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var correlationId = GetOrCreateCorrelationId(httpContext);
        httpContext.Items[CorrelationIdItemKey] = correlationId;

        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path.ToString();

        logger.LogInformation(
            "Request started: CorrelationId={CorrelationId}, Method={Method}, Path={Path}",
            correlationId, method, path);

        var stopwatch = Stopwatch.StartNew();

        await next(httpContext);

        stopwatch.Stop();

        logger.LogInformation(
            "Request completed: CorrelationId={CorrelationId}, Method={Method}, Path={Path}, StatusCode={StatusCode}, Duration={Duration}ms",
            correlationId, method, path, httpContext.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }

    private string GetOrCreateCorrelationId(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(CorrelationIdCookieName, out var existingCorrelationId) 
            && !string.IsNullOrWhiteSpace(existingCorrelationId))
        {
            return existingCorrelationId;
        }

        var newCorrelationId = Guid.NewGuid().ToString();

        httpContext.Response.Cookies.Append(
            CorrelationIdCookieName,
            newCorrelationId,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

        return newCorrelationId;
    }
}
