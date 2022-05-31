using System;
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
            return RedirectToAction("Index", "EnergyEfficiency");
        }

        [HttpGet("/testError")]
        public IActionResult testError()
        {
            _logger.Log(LogLevel.Critical, "This is a test critical log");
            _logger.Log(LogLevel.Error, "This is a test error log");

            throw new Exception("This is a test unhandled exception");
        }
    }
}