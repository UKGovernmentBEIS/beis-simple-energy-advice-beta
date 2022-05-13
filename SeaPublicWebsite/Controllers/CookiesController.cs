using System;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult CookieConsent()
    {
        throw new Exception();
    }

    [HttpGet("/cookie-details")]
    public IActionResult CookieDetails()
    {
        throw new Exception();
    }

}