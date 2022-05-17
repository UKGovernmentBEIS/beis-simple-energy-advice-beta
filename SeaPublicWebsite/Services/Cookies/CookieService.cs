﻿using System;
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
    
    public bool CookieSettingsAreUpToDate(HttpRequest request)
    {
        return TryGetCookie<CookieSettings>(request, Configuration.CookieSettingsCookieName, out var cookie) && 
               cookie.Version == Configuration.CurrentCookieMessageVersion;
    }

    public bool HasAcceptedGoogleAnalytics(HttpRequest request)
    {
        return TryGetCookie<CookieSettings>(request, Configuration.CookieSettingsCookieName, out var cookie) && cookie.GoogleAnalytics;
    }

    public void SetCookie<T>(HttpResponse response, string cookieName, T cookie)
    {
        var cookieString = JsonConvert.SerializeObject(cookie);
        response.Cookies.Append(
            cookieName,
            cookieString,
            new CookieOptions {Secure = true, SameSite = SameSiteMode.Lax, MaxAge = TimeSpan.FromDays(Configuration.DefaultDaysUntilExpiry)});
    }
}