using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace SeaPublicWebsite.PdfGeneration.Services;

public class PdfGenerationService
{
    public async Task<Stream> GeneratePdf(string path, string username, string password)
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();
        await page.AuthenticateAsync(new Credentials { Username = username, Password = password });
        await page.GoToAsync($"https://localhost:5001/{path}");
        var pdfStream = await page.PdfStreamAsync(
            new PdfOptions
            {
                MarginOptions = new MarginOptions { Top = "1cm", Left = "1cm", Right = "1cm", Bottom = "1cm" },
                PrintBackground = true
            }
        );
        await browser.CloseAsync();
        return pdfStream;
    }
}
