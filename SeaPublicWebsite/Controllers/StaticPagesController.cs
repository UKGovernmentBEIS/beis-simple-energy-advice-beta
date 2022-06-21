using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Controllers;

public class StaticPagesController : Controller
{
    [HttpGet("/accessibility-statement")]
    public IActionResult AccessibilityStatement()
    {
        return View("AccessibilityStatement");
    }
}