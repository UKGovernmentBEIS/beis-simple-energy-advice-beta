using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Controllers
{
    public class HealthCheckController : Controller
    {
        [HttpGet("/health-check")]
        [IgnoreAntiforgeryToken] // TODO: BEISSEA-85: This seems like the best option but it hasn't been tested yet
        public IActionResult Index()
        {
            return View();
        }
    }
}