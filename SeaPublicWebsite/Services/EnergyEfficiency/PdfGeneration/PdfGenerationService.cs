using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SeaPublicWebsite.Middleware;

namespace SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;

public class PdfGenerationService
{
    private readonly BasicAuthMiddlewareConfiguration basicAuthConfiguration;
    private readonly IServer server;

    public PdfGenerationService(
        IOptions<BasicAuthMiddlewareConfiguration> basicAuthOptions,
        IServer server)
    {
        basicAuthConfiguration = basicAuthOptions.Value;
        this.server = server;
    }

    // This function runs a headless chrome browser without a sandbox (--no-sandbox).
    // This means whatever we run in that browser has access to the server's kernel
    // There is a ticket to revisit this when we move to BEIS Digital's platform:
    // https://softwiretech.atlassian.net/browse/BEISSEA-73
    public async Task<Stream> GeneratePdf(string path)
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            IgnoreDefaultArgs = true,
            IgnoredDefaultArgs = new[] { "--disable-extensions" },
            Args = new [] { "--no-sandbox" }
        };
        
        var browser = await Puppeteer.LaunchAsync(launchOptions);
        var page = await browser.NewPageAsync();
        await page.AuthenticateAsync(new Credentials
            { Username = basicAuthConfiguration.Username, Password = basicAuthConfiguration.Password });
        await page.GoToAsync($"{GetLocalAddress()}/{path}");
        var pdfStream = await page.PdfStreamAsync(
            new PdfOptions
            {
                MarginOptions = new MarginOptions { Top = "1cm", Left = "1cm", Right = "1cm", Bottom = "1cm" },
                PrintBackground = true,
                Scale = new decimal(0.8)
            }
        );
        await browser.CloseAsync();
        return pdfStream;
    }

    private string GetLocalAddress()
    {
        return server.Features.Get<IServerAddressesFeature>().Addresses.First();
    }
}
