using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SeaPublicWebsite.Middleware;

namespace SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;

public class PdfGenerationService
{
    private readonly BasicAuthMiddlewareConfiguration basicAuthConfiguration;

    public PdfGenerationService(IOptions<BasicAuthMiddlewareConfiguration> basicAuthOptions)
    {
        basicAuthConfiguration = basicAuthOptions.Value;
    }

    // This function runs a headless chrome browser without a sandbox (--no-sandbox).
    // This means whatever we run in that browser has access to the server's kernel
    // There is a ticket to revisit this when we move to BEIS Digital's platform:
    // https://softwiretech.atlassian.net/browse/BEISSEA-73
    public async Task<Stream> GeneratePdf(string path)
    {
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            Args = new [] { "--no-sandbox" }
        };
        
        var browser = await Puppeteer.LaunchAsync(launchOptions);
        var page = await browser.NewPageAsync();
        await page.AuthenticateAsync(new Credentials
            { Username = basicAuthConfiguration.Username, Password = basicAuthConfiguration.Password });
        await page.SetCookieAsync(new CookieParam { 
            Name = "service_language", 
            Value = $"c={CultureInfo.CurrentCulture.Name}|uic={CultureInfo.CurrentUICulture.Name}",
            Domain = "localhost"
        });
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
        // If the port the application runs on is ever changed this will need to be updated
        return "http://localhost:80";
    }
}
