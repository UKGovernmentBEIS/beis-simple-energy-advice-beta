using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SeaPublicWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TopLevelConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IOptions<TopLevelConfiguration> options)
        {
            _logger = logger;
            _configuration = options.Value;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "EnergyEfficiency");
        }

        [HttpGet("/testError")]
        public IActionResult TestError()
        {
            _logger.Log(LogLevel.Critical, "This is a test critical log");
            _logger.Log(LogLevel.Error, "This is a test error log");

            throw new Exception("This is a test unhandled exception");
        }
        
        [HttpGet("/testDbConnection")]
        public IActionResult TestDbConnection()
        {
            _logger.Log(LogLevel.Information, $"Database url = '{_configuration.DATABASE_URL}'");
            return RedirectToAction("Index", "EnergyEfficiency");
        }
    }

    public class TopLevelConfiguration
    {
        public string DATABASE_URL { get; set; }
    }
}