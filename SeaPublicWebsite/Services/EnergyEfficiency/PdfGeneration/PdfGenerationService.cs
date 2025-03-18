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
    public async Task<Stream> GeneratePdf(string path)
    {
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            Args =
            [
                /* Investigated this flag's removal in PC-1760
                // Due to limitations with running chromium sandboxes in Fargate, we are unable to remove this flag
                // See GitHub issue: https://github.com/aws/containers-roadmap/issues/2102
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