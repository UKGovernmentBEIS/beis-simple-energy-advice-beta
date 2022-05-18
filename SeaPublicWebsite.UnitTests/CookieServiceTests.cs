using System;
using NUnit.Framework;
using FluentAssertions;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.Models.Cookies;
using SeaPublicWebsite.Services.Cookies;

namespace Tests;

[TestFixture]
public class CookieServiceTests
{
    private CookieService CookieService;
    private string Key;
    private int LatestVersion;
    
    [DatapointSource] 
    private CookieSettings[] CookieSettings;

    public CookieServiceTests()
    {
        var config = new CookieServiceConfiguration
        {
            CookieSettingsCookieName = "cookie_settings",
            CurrentCookieMessageVersion = 3,
            DefaultDaysUntilExpiry = 365
        };
        var options = Options.Create(config);
        CookieService = new CookieService(options);

        Key = CookieService.Configuration.CookieSettingsCookieName;
        LatestVersion = CookieService.Configuration.CurrentCookieMessageVersion;
        CookieSettings = SetUpCookieSettings();
    }

    [SetUp]
    public void Setup()
    {
    }
    
    [Theory]
    public void CanSetResponseCookie(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var response = context.Response;
        
        // Act
        CookieService.SetCookie(response, Key, value);
        
        // Assert
        AssertResponseContainsCookie(response, Key, value);
    }

    [Theory]
    public void CanGetRequestCookieSettings(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Act
        var success = CookieService.TryGetCookie<CookieSettings>(request, Key, out var cookie);
        
        // Assert
        success.Should().Be(true);
        cookie.Should().BeEquivalentTo(value);
    }
    
    [Test]
    public void ShouldReturnFalseIfCantGetRequestCookie()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        
        // Act
        var success = CookieService.TryGetCookie<CookieSettings>(request, Key, out _);
        
        // Assert
        success.Should().Be(false);
    }
    
    [Theory]
    public void CanCheckIfCookieSettingsVersionMatches(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Act
        var success = CookieService.CookieSettingsAreUpToDate(request);
        
        // Assert
        success.Should().Be(value.Version == LatestVersion);
    }
    
    [Theory]
    public void CanCheckIfGoogleAnalyticsAreAccepted(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Act
        var success = CookieService.HasAcceptedGoogleAnalytics(request);
        
        // Assert
        success.Should().Be(value.GoogleAnalytics);
    }
    
    [Test]
    public void HidesBannerIfOnCookiePage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        var response = context.Response;
        request.Path = "/cookies";

        // Act
        var bannerState = CookieService.GetAndUpdateBannerState(request, response);
        
        // Assert
        bannerState.Should().Be(BannerState.Hide);
    }
    
    [Theory]
    public void ShowsBannerIfSettingsAreOutdatedOrMissing(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        var response = context.Response;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Precondition
        Assume.That(!CookieService.CookieSettingsAreUpToDate(request));

        // Act
        var bannerState = CookieService.GetAndUpdateBannerState(request, response);
        
        // Assert
        bannerState.Should().Be(BannerState.ShowBanner);
    }
    
    [Theory]
    public void HidesBannerIfCookiesWereSetAndConfirmationWasShown(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        var response = context.Response;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Precondition

        // Act
        var bannerState = CookieService.GetAndUpdateBannerState(request, response);
        
        // Assert
        bannerState.Should().Be(BannerState.Hide);
    }
    
    [Theory]
    public void ShowsConfirmationBannerAndUpdatesRequestCookieIfItWasNotShownAlready(CookieSettings value)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        var response = context.Response;
        request.Headers.Cookie = $"{Key}={ConvertObjectToHttpHeaderSrting(value)}";
        
        // Precondition
        Assume.That(CookieService.CookieSettingsAreUpToDate(request));
        Assume.That(!value.ConfirmationShown);

        // Act
        var bannerState = CookieService.GetAndUpdateBannerState(request, response);
        
        // Assert
        var expectedBannerState = value.GoogleAnalytics ? BannerState.ShowAccepted : BannerState.ShowRejected;
        bannerState.Should().Be(expectedBannerState);
        value.ConfirmationShown = true;
        AssertResponseContainsCookie(response, Key, value);
    }

    private void AssertResponseContainsCookie(HttpResponse response, string key, object value)
    {
        response.Headers.SetCookie.ToString().Should().Contain($"{key}={ConvertObjectToHttpHeaderSrting(value)}");
    }

    private string ConvertObjectToHttpHeaderSrting(object o)
    {
        return Uri.EscapeDataString(JsonConvert.SerializeObject(o));
    }

    private CookieSettings[] SetUpCookieSettings()
    {
        var acceptedLatestCookies = new CookieSettings
        {
            Version = LatestVersion,
            ConfirmationShown = true,
            GoogleAnalytics = true
        };
        
        var outdatedVersion = new CookieSettings
        {
            Version = LatestVersion - 1,
            ConfirmationShown = true,
            GoogleAnalytics = true
        };
        
        var rejectedAnalyticsConfirmationShown = new CookieSettings
        {
            Version = LatestVersion,
            ConfirmationShown = true,
            GoogleAnalytics = false
        };
        
        var rejectedAnalyticsConfirmationNotShown = new CookieSettings
        {
            Version = LatestVersion,
            ConfirmationShown = false,
            GoogleAnalytics = false
        };

        var missingCookie = new CookieSettings();
        
        return new[]
        {
            acceptedLatestCookies,
            outdatedVersion,
            rejectedAnalyticsConfirmationShown,
            rejectedAnalyticsConfirmationNotShown,
            missingCookie
        };
    }
}