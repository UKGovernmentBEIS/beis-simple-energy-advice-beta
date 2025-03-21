using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SeaPublicWebsite.BusinessLogic.Services.Password;

namespace SeaPublicWebsite.Services.EnergyEfficiency.PdfGeneration;

public class PdfGenerationService(AuthService authService, PasswordService passwordService)
{
    /// <summary>
    ///     Warning: When using this method, ensure you only supply paths to trusted content as it creates a non-sandboxed
    ///     browser.
    /// </summary>
    public async Task<Stream> GeneratePdf(string path)
    {
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            Args =
            [
                /*
                //'--no-sandbox' is not considered good practice as what is running in this browser can have root access to the container
                // This is only used here as ECS Tasks on Fargate do not currently support sandboxing
                // See GitHub issue: https://github.com/aws/containers-roadmap/issues/2102
                // At present, we aren't concerned about this as we only serve our own content on this browser process,
                // and we don't render unvalidated user input
                // If you are using this method for any future reason, ensure you are only serving trusted content
                */
                "--no-sandbox"
            ]
        };
        var browser = await Puppeteer.LaunchAsync(launchOptions);
        var page = await browser.NewPageAsync();

        if (authService.AuthIsEnabled())
        {
            await page.SetCookieAsync(new CookieParam
            {
                Name = AuthService.AuthCookieName,
                Value = passwordService.GetConfiguredPasswordHash(),
                Domain = "localhost"
            });
        }

        await page.SetCookieAsync(new CookieParam
        {
            Name = "service_language",
            Value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureInfo.CurrentCulture)),
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
        return "http://localhost:8080";
    }
}