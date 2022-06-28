using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeaPublicWebsite.Models.Cookies;
using SeaPublicWebsite.Services.Cookies;

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
            return RedirectToAction(nameof(EnergyEfficiencyController.Index), "EnergyEfficiency");
        }
    }
}