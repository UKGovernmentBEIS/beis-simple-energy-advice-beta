﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Controllers;

[Route("error")]
public class ErrorController: Controller
{
    [HttpGet("{code:int}")]
    public IActionResult Index(int code)
    {
        return code switch
        {
            404 => View("PageNotFound"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}