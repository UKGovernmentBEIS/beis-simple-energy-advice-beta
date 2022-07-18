using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Data;

namespace SeaPublicWebsite.DeleteOldData
{
    class Program
    {
        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            ExemplifyScoping(host.Services);

            return host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddScoped<IDataAccessProvider, DataAccessProvider>()
        .AddDbContext<SeaDbContext>(opt =>
        opt.UseNpgsql("UserId=postgres;Password=postgres;Server=localhost;Port=5432;Database=seadev;Integrated Security=true;Include Error Detail=true;Pooling=true")));

        static void ExemplifyScoping(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            services.GetRequiredService<IDataAccessProvider>().DeleteOldPropertyData();
        }
    }
}