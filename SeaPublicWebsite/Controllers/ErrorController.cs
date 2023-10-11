using System;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Controllers;

[Route("error")]
public class ErrorController: Controller
{
    [HttpGet]
    [HttpPost]
    public IActionResult HandleException()
    {
        return View("ServiceIssue");
    }
    
    [HttpGet("{code:int}")]
    public IActionResult HandleErrorsWithStatusCode(int code)
    {
        return code switch
        {
            404 => View("PageNotFound"),
            403 => View("Forbidden"), // TODO: BEISSEA-85: This is a WIP and should be removed
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}