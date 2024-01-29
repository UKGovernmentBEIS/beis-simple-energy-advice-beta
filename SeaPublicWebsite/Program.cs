using System;
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
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Data;
using Serilog;
using Serilog.Events;

namespace SeaPublicWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a Serilog bootstrap logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            
            var builder = WebApplication.CreateBuilder(args);
            
            // Hide that we are using Kestrel for security reasons
            builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false);
            
            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);

            // Switch to the full Serilog logger
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName());
            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("cy"),
                new CultureInfo("en-GB")
            };   
            builder.Services.AddControllersWithViews();  
            builder.Services
                .AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services
                .AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            
            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture("en-GB");   
                options.SupportedCultures = supportedCultures;    
                options.SupportedUICultures = supportedCultures; 
            });
            
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
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

            
            // Migrate the database for local dev and for instance 0 on GOV.PaaS.
            // As we use rolling deployments there shouldn't be any chance of multiple instances of this running at the
            // same time anyway, but it's easy to check the instance index for extra safety.
            if (app.Environment.IsDevelopment() || app.Configuration["CF_INSTANCE_INDEX"] == "0")
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SeaDbContext>();
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}