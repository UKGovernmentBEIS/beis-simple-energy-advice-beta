using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SeaPublicWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
#if DEBUG
            return Redirect("energy-efficiency/new-or-returning-user");
#endif
#pragma warning disable CS0162 // Unreachable code detected
            return Redirect("https://www.gov.uk/improve-energy-efficiency");
#pragma warning restore CS0162
        }
    }
}