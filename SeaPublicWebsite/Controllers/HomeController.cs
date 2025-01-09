using System;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Controllers;

public class HomeController : Controller
{
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