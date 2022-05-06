using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.FileRepositories;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Helpers.UserFlow;

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
            services.AddScoped<IPageLinker, PageLinker>();

            ConfigureFileRepository(services);
            
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
            if (!string.IsNullOrWhiteSpace(Global.BasicAuthUsername)
                && !string.IsNullOrWhiteSpace(Global.BasicAuthPassword))
            {
                // Add HTTP Basic Authentication in our non-production environments to make sure people don't accidentally stumble across the site
                // The site will still also be secured by the usual login/cookie auth - this is just an extra layer to make the site not publicly accessible
                app.UseMiddleware<BasicAuthMiddleware>();
            }
        }
    }
}