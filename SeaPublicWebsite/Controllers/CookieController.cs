using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.Cookies;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;

namespace SeaPublicWebsite.Controllers;

public class CookieController: Controller
{
    private readonly CookieService cookieService;

    public CookieController(CookieService cookieService)
    {
        this.cookieService = cookieService;
    }

    [HttpGet("/cookies")]
    public IActionResult CookieSettings_Get()
    {
        return View("CookieSettings");
    }

    [HttpPost("/cookies")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieSettings_Post()
    {
        return View("CookieSettings");
    }

    [HttpPost("/cookie-consent")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieConsent(CookieConsent cookieConsent)
    {
        var cookiesAccepted = cookieConsent.Consent == "accept";
        var cookieSettings = new CookieSettings
        {
            Version = cookieService.Configuration.CurrentCookieMessageVersion,
            GoogleAnalytics = cookiesAccepted
        };
        cookieService.SetCookiesSettings(Response, cookieSettings);

        return Redirect(cookieConsent.ReturnUrl);
    }

    [HttpGet("/cookie-details")]
    public IActionResult CookieDetails()
    {
        return View("CookieDetails");
    }

}