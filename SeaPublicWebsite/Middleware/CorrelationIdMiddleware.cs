using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SeaPublicWebsite.Services.Cookies;

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

    public async Task Invoke(HttpContext httpContext, CookieService cookieService)
    {
        var correlationId = GetOrCreateCorrelationId(httpContext, cookieService);
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

    private static string GetOrCreateCorrelationId(HttpContext httpContext, CookieService cookieService)
    {
        if (cookieService.TryGetCookie<string>(httpContext.Request, CorrelationIdCookieName, out var existingCorrelationId) 
            && !string.IsNullOrWhiteSpace(existingCorrelationId))
        {
            return existingCorrelationId;
        }

        var newCorrelationId = Guid.NewGuid().ToString();

        cookieService.SetSessionCookie(httpContext.Response, CorrelationIdCookieName, newCorrelationId);

        return newCorrelationId;
    }
}
