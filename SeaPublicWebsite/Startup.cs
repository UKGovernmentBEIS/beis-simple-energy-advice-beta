using System;
using System.Text.RegularExpressions;
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
using SeaPublicWebsite.Data;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.GoogleAnalytics;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Middleware;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;
using SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;
using Serilog;

namespace SeaPublicWebsite
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AnswerService>();
            services.AddScoped<PropertyDataStore>();
            services.AddScoped<PropertyDataUpdater>();
            services.AddScoped<IQuestionFlowService, QuestionFlowService>();
            services.AddScoped<PostcodesIoApi>();
            services.AddMemoryCache();
            services.AddSingleton<StaticAssetsVersioningService>();
            services.AddScoped<RecommendationService>();
            services.AddScoped<IDataAccessProvider, DataAccessProvider>();
            services.AddDataProtection().PersistKeysToDbContext<SeaDbContext>();

            ConfigureEpcApi(services);
            ConfigureBreApi(services);
            ConfigureGovUkNotify(services);
            ConfigureCookieService(services);
            ConfigureDatabaseContext(services);
            ConfigureGoogleAnalyticsService(services);
            ConfigurePdfGeneration(services);
            ConfigureFullHostnameService(services);

            services.Configure<BasicAuthMiddlewareConfiguration>(
                configuration.GetSection(BasicAuthMiddlewareConfiguration.ConfigSection));

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ErrorHandlingFilter>();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.ModelMetadataDetailsProviders.Add(new GovUkDataBindingErrorTextProvider());
            });
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

        private void ConfigureFullHostnameService(IServiceCollection services)
        {
            services.AddScoped<FullHostnameService>();
            services.Configure<FullHostnameConfiguration>(configuration.GetSection(FullHostnameConfiguration.ConfigSection));
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

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            ConfigureHttpBasicAuth(app);

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureHttpBasicAuth(IApplicationBuilder app)
        {
            // Add HTTP Basic Authentication in our non-production environments to make sure people don't accidentally stumble across the site
            // and on production only for pdf generation endpoints
            app.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}