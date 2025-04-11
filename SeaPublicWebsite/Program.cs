using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SeaPublicWebsite.Data;

namespace SeaPublicWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Hide that we are using Kestrel for security reasons
            builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false);

            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);
            
            List<CultureInfo> supportedCultures =
            [
                new CultureInfo("cy"),
                new CultureInfo("en-GB")
            ];
            builder.Services.AddControllersWithViews();
            builder.Services
                .AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services
                .AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var app = builder.Build();
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider
                    {
                        CookieName = "service_language",
                    },
                }
            };


            var requestProvider = new RouteDataRequestCultureProvider();
            requestLocalizationOptions.RequestCultureProviders.Insert(0, requestProvider);
            app.UseRequestLocalization(requestLocalizationOptions);

            startup.Configure(app, app.Environment);

            // Migrate the database if it's out of date. Ideally we wouldn't do this on app startup for our deployed
            // environments, because we're risking multiple containers attempting to run the migrations concurrently and
            // getting into a mess. However, we very rarely add migrations at this point, so in practice it's easier to
            // risk it and keep an eye on the deployment: we should be doing rolling deployments anyway which makes it
            // very unlikely we run into concurrency issues. If that changes though we should look at moving migrations
            // to a deployment pipeline step, and only doing the following locally (PC-1150).
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SeaDbContext>();
            dbContext.Database.Migrate();

            app.Run();
        }
    }
}