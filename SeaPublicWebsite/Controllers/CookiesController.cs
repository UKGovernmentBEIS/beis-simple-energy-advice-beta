using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.Cookie;

namespace SeaPublicWebsite.Controllers;

public class CookiesController: Controller
{

    [HttpGet("/cookies")]
    public IActionResult CookieSettingsGet()
    {
        throw new Exception();
    }

    [HttpPost("/cookies")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieSettingsPost()
    {
        throw new Exception();
    }

    [HttpPost("/cookie-consent")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieConsent(CookieConsent cookieConsent)
    {
        var cookiesAccepted = cookieConsent.Consent == "accept";
        if (cookiesAccepted)
        {
            Console.WriteLine("True");
        }
        else
        {
            Console.WriteLine("False");
        }

        return Redirect(cookieConsent.ReturnUrl);
    }

    [HttpGet("/cookie-details")]
    public IActionResult CookieDetails()
    {
        throw new Exception();
    }

}