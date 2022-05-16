using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.Models.Cookies;

namespace SeaPublicWebsite.Services.Cookies;

public class CookieService
{
    public readonly CookieServiceConfiguration Configuration;

    public CookieService(IOptions<CookieServiceConfiguration> options)
    {
        Configuration = options.Value;
    }

    public bool TryGetCookie<T>(HttpRequest request, string cookieName, out T cookie)
    {
        if (request.Cookies.TryGetValue(cookieName, out var cookieString))
        {
            try
            {
                cookie = JsonConvert.DeserializeObject<T>(cookieString);
                return true;
            }
            catch (JsonException)
            {
                // In case of failure, return false as if there was no cookie
            }
        }

        cookie = default;
        return false;
    }
    public bool HasSeenCookieLatestMessage(HttpRequest request)
    {
        if (TryGetCookie<CookieSettings>(request, Configuration.CookieSettingsCookieName, out var cookie))
        {
            return cookie.Version == Configuration.CurrentCookieMessageVersion;
        }
        return false;
    }

    public bool HasAcceptedGoogleAnalytics(HttpRequest request)
    {
        return TryGetCookie<CookieSettings>(request, Configuration.CookieSettingsCookieName, out var cookie) && cookie.GoogleAnalytics;
    }

    public void SetCookie<T>(HttpResponse response, string cookieName, T cookie, int daysUntilExpiry = 365)
    {
        var cookieString = JsonConvert.SerializeObject(cookie);
        response.Cookies.Append(
            cookieName,
            cookieString,
            new CookieOptions {Secure = true, SameSite = SameSiteMode.Lax, MaxAge = TimeSpan.FromDays(daysUntilExpiry)});
    }
}