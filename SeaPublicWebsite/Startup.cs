using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Controllers;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.Bre;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.FileRepositories;
using SeaPublicWebsite.ExternalServices.OpenEpc;
using SeaPublicWebsite.Middleware;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;

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
            services.AddScoped<UserDataStore, UserDataStore>();
            services.AddScoped<IQuestionFlowService, QuestionFlowService>();
            services.AddMemoryCache();
            services.AddSingleton<StaticAssetsVersioningService>();

            ConfigureFileRepository(services);
            ConfigureEpcApi(services);
            ConfigureBreApi(services);
            ConfigureGovUkNotify(services);
            ConfigureCookieService(services);

            if (!webHostEnvironment.IsProduction())
            {
                services.Configure<BasicAuthMiddlewareConfiguration>(
                    configuration.GetSection(BasicAuthMiddlewareConfiguration.ConfigSection));
            }
            
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ErrorHandlingFilter>();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddScoped<RecommendationService>();
            
            services.AddDbContext<SeaDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection")));
        }
        
        private void ConfigureCookieService(IServiceCollection services)
        {
            services.Configure<CookieServiceConfiguration>(
                configuration.GetSection(CookieServiceConfiguration.ConfigSection));
            services.AddScoped<CookieService, CookieService>();
        }

        private void ConfigureFileRepository(IServiceCollection services)
        {
            if (!webHostEnvironment.IsDevelopment())
            {
                var vcapServiceConfig = VcapServiceFactory.GetVcapServices(configuration);
                VcapAwsS3Bucket fileStorageBucketConfiguration = vcapServiceConfig.AwsS3Bucket.First(b => b.Name.EndsWith("-filestorage"));

                services.AddSingleton<IFileRepository>(s => new AwsFileRepository(fileStorageBucketConfiguration));
            }
            else
            {
                services.AddSingleton<IFileRepository>(s => new SystemFileRepository());
            }
        }

        private void ConfigureEpcApi(IServiceCollection services)
        {
            services.Configure<OpenEpcConfiguration>(
                configuration.GetSection(OpenEpcConfiguration.ConfigSection));
            services.AddScoped<IEpcApi, OpenEpcApi>();
            // TODO: When the EPB API is ready, uncomment this and remove the above:
            // services.Configure<EpbEpcConfiguration>(
            //     configuration.GetSection(EpbEpcConfiguration.ConfigSection));
            // services.AddScoped<IEpcApi, EpbEpcApi>();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!webHostEnvironment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            ConfigureHttpBasicAuth(app);
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureHttpBasicAuth(IApplicationBuilder app)
        {
            if (!webHostEnvironment.IsProduction())
            {
                // Add HTTP Basic Authentication in our non-production environments to make sure people don't accidentally stumble across the site
                app.UseMiddleware<BasicAuthMiddleware>();
            }
        }
    }
}