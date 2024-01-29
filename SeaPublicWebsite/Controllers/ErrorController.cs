using System;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.Error;

namespace SeaPublicWebsite.Controllers;

[Route("error")]
public class ErrorController: Controller
{
    [HttpGet]
    [HttpPost]
    public IActionResult HandleException()
    {
        var model = new ErrorViewModel { ShowAnswersNotSavedMessage = true };
        return View("ServiceIssue", model);
    }

    [HttpGet("{code:int}")]
    public IActionResult HandleErrorsWithStatusCode(int code)
    {
        return code switch
        {
            404 => View("PageNotFound"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpGet("service-issue")]
    public IActionResult ServiceIssue()
    {
        var model = new ErrorViewModel { ShowAnswersNotSavedMessage = false };
        return View("ServiceIssue", model);
    }
}