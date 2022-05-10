using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.FileRepositories;
using SeaPublicWebsite.ExternalServices.OpenEpc;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UserDataStore, UserDataStore>();
            services.AddMemoryCache();

            ConfigureFileRepository(services);
            ConfigureEpcApi(services);
            ConfigureGovUkNotify(services);
            
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ErrorHandlingFilter>();
            });
        }

        private void ConfigureFileRepository(IServiceCollection services)
        {
            if (!Config.IsLocal())
            {
                VcapAwsS3Bucket fileStorageBucketConfiguration = Global.VcapServices.AwsS3Bucket.First(b => b.Name.EndsWith("-filestorage"));

                services.AddSingleton<IFileRepository>(s => new AwsFileRepository(fileStorageBucketConfiguration));
            }
            else
            {
                services.AddSingleton<IFileRepository>(s => new SystemFileRepository());
            }

        }

        private void ConfigureEpcApi(IServiceCollection services)
        {
            services.AddScoped<IEpcApi, OpenEpcApi>();
            services.Configure<OpenEpcConfiguration>(
                Configuration.GetSection(OpenEpcConfiguration.ConfigSection));
            // TODO: When the EPB API is ready, uncomment this and remove the above:
            // services.AddScoped<IEpcApi, EPBEPCApi>();
            // services.Configure<EpbEpcConfiguration>(
            //     Configuration.GetSection(EpbEpcConfiguration.ConfigSection));
        }

        private void ConfigureGovUkNotify(IServiceCollection services)
        {
            services.AddScoped<IEmailSender, GovUkNotifyApi>();
            services.Configure<GovUkNotifyConfiguration>(
                Configuration.GetSection(GovUkNotifyConfiguration.ConfigSection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

        private static void ConfigureHttpBasicAuth(IApplicationBuilder app)
        {
            if (!Config.IsProduction())
            {
                // Add HTTP Basic Authentication in our non-production environments to make sure people don't accidentally stumble across the site
                app.UseMiddleware<BasicAuthMiddleware>();
            }
        }
    }
}