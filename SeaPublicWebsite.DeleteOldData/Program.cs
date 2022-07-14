using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Data;

namespace SeaPublicWebsite.DeleteOldData
{
    public class Program
    {
        static Task Main(string[] args) =>
                CreateHostBuilder(args).Build().RunAsync();

            static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((_, services) =>
                        services.AddHostedService<Worker>()
                            .AddScoped<IDataAccessProvider, DataAccessProvider>());
    }

    public class Worker : BackgroundService
    {
        private readonly IDataAccessProvider dataAccessProvider;

        public Worker(IDataAccessProvider dataAccessProvider) =>
            this.dataAccessProvider = dataAccessProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            dataAccessProvider.DeleteOldPropertyData();
            return Task.CompletedTask;
        }
    }
}