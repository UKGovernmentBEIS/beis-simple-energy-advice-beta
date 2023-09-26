using System;
using System.Security.Cryptography;
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
        CheckXContentTypeOptions(context);
        CheckContentSecurityPolicy(context);
        CheckReferrerPolicy(context);

        return next(context);
    }

    private static void CheckXContentTypeOptions(HttpContext context)
    {
        if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        }
    }

    private static void CheckContentSecurityPolicy(HttpContext context)
    {
        if (context.Response.Headers.ContainsKey("Content-Security-Policy")) { return; }
        
        var nonce = GenerateNonce();
        context.Items.Add("ScriptNonce", nonce);
            
        context.Response.Headers.Add(
            "Content-Security-Policy",
            "default-src 'self'; " +
            $"script-src 'self' https://*.googletagmanager.com 'nonce-{nonce}'; " +
            "connect-src 'self' https://*.googletagmanager.com; " +
            "img-src 'self' https://*.googletagmanager.com");
    }

    private static void CheckReferrerPolicy(HttpContext context)
    {
        if (!context.Response.Headers.ContainsKey("Referrer-Policy"))
        {
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");

        }
    }

    private static string GenerateNonce()
    {
        var rng = RandomNumberGenerator.Create();
        var nonceBytes = new byte[32];
        rng.GetBytes(nonceBytes);
        var nonce = Convert.ToBase64String(nonceBytes);
        return nonce;
    }
}