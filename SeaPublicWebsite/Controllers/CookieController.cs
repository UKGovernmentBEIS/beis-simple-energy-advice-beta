﻿using System;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.Cookies;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;

namespace SeaPublicWebsite.Controllers;

public class CookieController: Controller
{
    private readonly CookieService cookieService;
    private readonly GlobalConstants globalConstants;

    public CookieController(CookieService cookieService, GlobalConstants globalConstants)
    {
        this.cookieService = cookieService;
        this.globalConstants = globalConstants;
    }

    [HttpGet("/cookies")]
    public IActionResult CookieSettings_Get(bool changesHaveBeenSaved = false)
    {
        cookieService.TryGetCookie<CookieSettings>(Request, cookieService.Configuration.CookieSettingsCookieName, out var cookie);
        
        var viewModel = new CookieSettingsViewModel
        {
            GoogleAnalytics = cookie?.GoogleAnalytics is true,
            ChangesHaveBeenSaved = changesHaveBeenSaved,
            ServiceName = globalConstants.Configuration.ServiceName
        };
        return View("CookieSettings", viewModel);
    }

    [HttpPost("/cookies")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieSettings_Post(CookieSettingsViewModel viewModel)
    {
        var cookieSettings = new CookieSettings
        {
            Version = cookieService.Configuration.CurrentCookieMessageVersion,
            GoogleAnalytics = viewModel.GoogleAnalytics,
        };
        cookieService.SetCookiesSettings(Response, cookieSettings);
        return CookieSettings_Get(changesHaveBeenSaved: true);
    }

    [HttpPost("/cookie-consent")]
    [ValidateAntiForgeryToken]
    public IActionResult CookieConsent(CookieConsent cookieConsent)
    {
        if (cookieConsent.Consent == "hide")
        {
            TempData["BannerState"] = BannerState.Hide;
            return Redirect(cookieConsent.ReturnUrl);
        }
        var cookiesAccepted = cookieConsent.Consent == "accept";
        var cookieSettings = new CookieSettings
        {
            Version = cookieService.Configuration.CurrentCookieMessageVersion,
            GoogleAnalytics = cookiesAccepted
        };
        cookieService.SetCookiesSettings(Response, cookieSettings);
        TempData["BannerState"] = cookiesAccepted
            ? BannerState.ShowAccepted
            : BannerState.ShowRejected;
        return Redirect(cookieConsent.ReturnUrl);
    }

    [HttpGet("/cookie-details")]
    public IActionResult CookieDetails()
    {
        var viewModel = new CookieDetailsViewModel
        {
            ServiceName = globalConstants.Configuration.ServiceName
        };
        return View("CookieDetails", viewModel);
    }

}