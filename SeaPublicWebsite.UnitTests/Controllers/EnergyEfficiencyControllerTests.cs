using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SeaPublicWebsite;
using SeaPublicWebsite.BusinessLogic;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.BusinessLogic.Services;
using SeaPublicWebsite.Config;
using SeaPublicWebsite.Controllers;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.GoogleAnalytics;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;
using SeaPublicWebsite.Services.EnergyEfficiency;
using SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;

namespace Tests.Controllers;

[TestFixture]
public class EnergyEfficiencyControllerTests
{
    private const string Reference = "ABCDEFGH";

    [Test]
    public async Task FindEpcGet_WhenEpcApiIsUnavailable_RedirectsToPropertyType()
    {
        var dependencies = BuildDependencies();
        dependencies.EpcApiMock.Setup(api => api.IsApiAvailable()).ReturnsAsync(false);
        var controller = BuildController(dependencies);

        var result = await controller.FindEpc_Get(Reference);

        var redirect = result as RedirectToActionResult;
        Assert.That(redirect, Is.Not.Null);
        Assert.That(redirect.ActionName, Is.EqualTo("PropertyType_Get"));
        Assert.That(redirect.RouteValues["reference"], Is.EqualTo(Reference));
    }

    [Test]
    public async Task FindEpcGet_WhenEpcApiIsAvailable_ReturnsFindEpcView()
    {
        var dependencies = BuildDependencies();
        dependencies.EpcApiMock.Setup(api => api.IsApiAvailable()).ReturnsAsync(true);
        dependencies.PropertyDataStoreMock
            .Setup(store => store.LoadPropertyDataAsync(Reference))
            .ReturnsAsync(new PropertyData
            {
                Reference = Reference,
                SearchForEpc = SearchForEpc.Yes
            });

        var controller = BuildController(dependencies);

        var result = await controller.FindEpc_Get(Reference);

        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(viewResult.ViewName, Is.EqualTo("FindEpc"));
    }

    [Test]
    public async Task ConfirmAddressGet_WhenEpcSearchThrowsUnavailable_RedirectsToEpcServiceError()
    {
        var dependencies = BuildDependencies();
        dependencies.EpcApiMock
            .Setup(api => api.GetEpcsInformationForPostcodeAndBuildingNameOrNumber("AB12CD", "12"))
            .ThrowsAsync(new EpcApiUnavailableException("Down", new System.Exception("network")));

        var controller = BuildController(dependencies);

        var result = await controller.ConfirmAddress_Get(Reference, "AB12CD", "12");

        var redirect = result as RedirectToActionResult;
        Assert.That(redirect, Is.Not.Null);
        Assert.That(redirect.ActionName, Is.EqualTo("EpcServiceError_Get"));
        Assert.That(redirect.RouteValues["reference"], Is.EqualTo(Reference));
    }

    [Test]
    public async Task ConfirmAddressPost_WhenEpcFetchThrowsUnavailable_RedirectsToEpcServiceError()
    {
        var dependencies = BuildDependencies();
        dependencies.PropertyDataStoreMock
            .Setup(store => store.LoadPropertyDataAsync(Reference))
            .ReturnsAsync(new PropertyData { Reference = Reference });
        dependencies.EpcApiMock
            .Setup(api => api.GetEpcForId("epc-1"))
            .ThrowsAsync(new EpcApiUnavailableException("Down", new System.Exception("network")));

        var controller = BuildController(dependencies);

        var result = await controller.ConfirmAddress_Post(new ConfirmAddressViewModel
        {
            Reference = Reference,
            SelectedEpcId = "epc-1",
            Number = "12",
            Postcode = "AB12CD"
        });

        var redirect = result as RedirectToActionResult;
        Assert.That(redirect, Is.Not.Null);
        Assert.That(redirect.ActionName, Is.EqualTo("EpcServiceError_Get"));
        Assert.That(redirect.RouteValues["reference"], Is.EqualTo(Reference));
    }

    [Test]
    public async Task ConfirmSingleAddressPost_WhenEpcFetchThrowsUnavailable_RedirectsToEpcServiceError()
    {
        var dependencies = BuildDependencies();
        dependencies.PropertyDataStoreMock
            .Setup(store => store.LoadPropertyDataAsync(Reference))
            .ReturnsAsync(new PropertyData { Reference = Reference });
        dependencies.EpcApiMock
            .Setup(api => api.GetEpcForId("epc-1"))
            .ThrowsAsync(new EpcApiUnavailableException("Down", new System.Exception("network")));

        var controller = BuildController(dependencies);

        var result = await controller.ConfirmSingleAddress_Post(new ConfirmSingleAddressViewModel
        {
            Reference = Reference,
            EpcId = "epc-1",
            EpcAddressConfirmed = EpcAddressConfirmed.Yes,
            Number = "12",
            Postcode = "AB12CD"
        });

        var redirect = result as RedirectToActionResult;
        Assert.That(redirect, Is.Not.Null);
        Assert.That(redirect.ActionName, Is.EqualTo("EpcServiceError_Get"));
        Assert.That(redirect.RouteValues["reference"], Is.EqualTo(Reference));
    }

    private static EnergyEfficiencyController BuildController(TestDependencies dependencies)
    {
        var propertyDataUpdater = new PropertyDataUpdater(dependencies.QuestionFlowServiceMock.Object,
            dependencies.EpcApiMock.Object);
        var answerService = new AnswerService(dependencies.PropertyDataStoreMock.Object, propertyDataUpdater);

        var controller = new EnergyEfficiencyController(
            dependencies.PropertyDataStoreMock.Object,
            dependencies.QuestionFlowServiceMock.Object,
            dependencies.PropertyDataService,
            dependencies.EpcApiMock.Object,
            dependencies.EmailSenderMock.Object,
            dependencies.CookieService,
            dependencies.GoogleAnalyticsService,
            dependencies.PdfGenerationService,
            dependencies.PostcodesIoApi,
            answerService,
            dependencies.FullHostnameService,
            Options.Create(new ServiceHealthConfig()),
            dependencies.LocalizerMock.Object
        );

        controller.Url = dependencies.UrlHelperMock.Object;
        return controller;
    }

    private static TestDependencies BuildDependencies()
    {
        var propertyDataStoreMock = new Mock<IPropertyDataStore>();
        var questionFlowServiceMock = new Mock<IQuestionFlowService>();
        questionFlowServiceMock.Setup(flow => flow.PreviousStep(
                It.IsAny<QuestionFlowStep>(),
                It.IsAny<PropertyData>(),
                It.IsAny<QuestionFlowStep?>()))
            .Returns(QuestionFlowStep.Start);

        var recommendationServiceMock = new Mock<IRecommendationService>();
        var propertyDataService =
            new PropertyDataService(propertyDataStoreMock.Object, recommendationServiceMock.Object);

        var cookieService = new CookieService(
            Options.Create(new CookieServiceConfiguration
            {
                CookieSettingsCookieName = "cookie-settings",
                CurrentCookieMessageVersion = 1,
                DefaultDaysUntilExpiry = 365
            }),
            Mock.Of<ILogger<CookieService>>());

        var googleAnalyticsService = new GoogleAnalyticsService(
            Options.Create(new GoogleAnalyticsConfiguration
            {
                BaseUrl = "https://example.com",
                ApiSecret = "secret",
                MeasurementId = "measurement",
                CookieName = "_ga"
            }),
            cookieService,
            Mock.Of<ILogger<GoogleAnalyticsService>>());

        var fullHostnameService = new FullHostnameService(
            Options.Create(new FullHostnameConfiguration { BaseUrl = "https://example.com" }));

        var urlHelperMock = new Mock<IUrlHelper>();
        urlHelperMock.Setup(url => url.Action(It.IsAny<UrlActionContext>())).Returns("/back");

        var authService = new AuthService(Mock.Of<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>());
        var passwordService = new SeaPublicWebsite.BusinessLogic.Services.Password.PasswordService(
            Options.Create(new SeaPublicWebsite.BusinessLogic.Services.Password.PasswordConfiguration()));
        var pdfGenerationService = new PdfGenerationService(authService, passwordService);
        var postcodesIoApi = new PostcodesIoApi(Mock.Of<ILogger<PostcodesIoApi>>());

        return new TestDependencies
        {
            PropertyDataStoreMock = propertyDataStoreMock,
            QuestionFlowServiceMock = questionFlowServiceMock,
            EpcApiMock = new Mock<IEpcApi>(),
            EmailSenderMock = new Mock<IEmailSender>(),
            LocalizerMock = new Mock<IStringLocalizer<SharedResources>>(),
            UrlHelperMock = urlHelperMock,
            PropertyDataService = propertyDataService,
            CookieService = cookieService,
            GoogleAnalyticsService = googleAnalyticsService,
            FullHostnameService = fullHostnameService,
            PdfGenerationService = pdfGenerationService,
            PostcodesIoApi = postcodesIoApi
        };
    }

    private class TestDependencies
    {
        public Mock<IPropertyDataStore> PropertyDataStoreMock { get; init; }
        public Mock<IQuestionFlowService> QuestionFlowServiceMock { get; init; }
        public Mock<IEpcApi> EpcApiMock { get; init; }
        public Mock<IEmailSender> EmailSenderMock { get; init; }
        public Mock<IStringLocalizer<SharedResources>> LocalizerMock { get; init; }
        public Mock<IUrlHelper> UrlHelperMock { get; init; }
        public PropertyDataService PropertyDataService { get; init; }
        public CookieService CookieService { get; init; }
        public GoogleAnalyticsService GoogleAnalyticsService { get; init; }
        public FullHostnameService FullHostnameService { get; init; }
        public PdfGenerationService PdfGenerationService { get; init; }
        public PostcodesIoApi PostcodesIoApi { get; init; }
    }
}
