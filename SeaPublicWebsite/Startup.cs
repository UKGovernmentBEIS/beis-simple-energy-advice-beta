using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.BusinessLogic;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;
using SeaPublicWebsite.BusinessLogic.Services;
using SeaPublicWebsite.BusinessLogic.Services.Password;
using SeaPublicWebsite.Config;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.GoogleAnalytics;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Middleware;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;
using SeaPublicWebsite.Services.EnergyEfficiency;
using SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;

namespace SeaPublicWebsite;

public class Startup
{
    private readonly AuthService authService;
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment webHostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        this.configuration = configuration;
        this.webHostEnvironment = webHostEnvironment;

        authService = new AuthService(this.webHostEnvironment);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<AnswerService>();
        services.AddScoped<IPropertyDataStore, PropertyDataStore>();
        services.AddScoped<PropertyDataUpdater>();
        services.AddScoped<IQuestionFlowService, QuestionFlowService>();
        services.AddScoped<PostcodesIoApi>();
        services.AddMemoryCache();
        services.AddSingleton<StaticAssetsVersioningService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IDataAccessProvider, DataAccessProvider>();
        services.AddScoped<EmergencyMaintenanceService>();
        services.AddDataProtection().PersistKeysToDbContext<SeaDbContext>();

        ConfigureServiceHealth(services);
        ConfigureEpcApi(services);
        ConfigureBreApi(services);
        ConfigureGovUkNotify(services);
        ConfigureCookieService(services);
        ConfigureDatabaseContext(services);
        ConfigureGoogleAnalyticsService(services);
        ConfigurePdfGeneration(services);
        ConfigureFullHostnameService(services);
        ConfigurePropertyDataService(services);
        ConfigurePassword(services);

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ErrorHandlingFilter>();
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            options.ModelMetadataDetailsProviders.Add(new GovUkDataBindingErrorTextProvider());
        });
    }

    private void ConfigureServiceHealth(IServiceCollection services)
    {
        services.Configure<ServiceHealthConfig>(
            configuration.GetSection(ServiceHealthConfig.ConfigSection));
    }

    private void ConfigureGoogleAnalyticsService(IServiceCollection services)
    {
        services.Configure<GoogleAnalyticsConfiguration>(
            configuration.GetSection(GoogleAnalyticsConfiguration.ConfigSection));
        services.AddScoped<GoogleAnalyticsService, GoogleAnalyticsService>();
    }

    private void ConfigureDatabaseContext(IServiceCollection services)
    {
        var databaseConnectionString = configuration.GetConnectionString("PostgreSQLConnection");

        services.AddDbContext<SeaDbContext>(opt =>
            opt.UseNpgsql(databaseConnectionString));
    }

    private void ConfigureCookieService(IServiceCollection services)
    {
        services.Configure<CookieServiceConfiguration>(
            configuration.GetSection(CookieServiceConfiguration.ConfigSection));
        // Change the default antiforgery cookie name so it doesn't include Asp.Net for security reasons
        services.AddAntiforgery(options => options.Cookie.Name = "Antiforgery");
        services.AddScoped<CookieService, CookieService>();
    }

    private void ConfigureEpcApi(IServiceCollection services)
    {
        services.Configure<EpbEpcConfiguration>(
            configuration.GetSection(EpbEpcConfiguration.ConfigSection));
        services.AddScoped<IEpcApi, EpbEpcApi>();
    }

    private void ConfigureBreApi(IServiceCollection services)
    {
        services.Configure<BreConfiguration>(
            configuration.GetSection(BreConfiguration.ConfigSection));
        services.AddScoped<BreApi>();
    }

    private void ConfigureGovUkNotify(IServiceCollection services)
    {
        services.AddScoped<IEmailSender, GovUkNotifyApi>();
        services.Configure<GovUkNotifyConfiguration>(
            configuration.GetSection(GovUkNotifyConfiguration.ConfigSection));
    }

    private void ConfigurePdfGeneration(IServiceCollection services)
    {
        services.AddScoped<PdfGenerationService>();
    }

    private void ConfigurePropertyDataService(IServiceCollection services)
    {
        services.AddScoped<PropertyDataService>();
    }

    private void ConfigureFullHostnameService(IServiceCollection services)
    {
        services.AddScoped<FullHostnameService>();
        services.Configure<FullHostnameConfiguration>(
            configuration.GetSection(FullHostnameConfiguration.ConfigSection));
    }

    private void ConfigurePassword(IServiceCollection services)
    {
        services.Configure<PasswordConfiguration>(
            configuration.GetSection(PasswordConfiguration.ConfigSection));
        services.AddScoped<PasswordService>();
        services.AddScoped<AuthService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!webHostEnvironment.IsDevelopment())
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandlingPath = "/error"
            });
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            // app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/error/{0}");

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        if (authService.AuthIsEnabled())
        {
            ConfigureAuth(app);
        }

        ConfigureEmergencyMaintenance(app);

        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void ConfigureAuth(IApplicationBuilder app)
    {
        // Add password authentication in our non-local-development and non-production environments
        // to make sure people don't accidentally stumble across the site
        app.UseMiddleware<AuthMiddleware>();
    }

    private void ConfigureEmergencyMaintenance(IApplicationBuilder app)
    {
        // Add emergency maintenance middleware to return 503 Service Unavailable if required
        app.UseMiddleware<EmergencyMaintenanceMiddleware>();
    }
}