using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.Models.Feedback;

namespace SeaPublicWebsite.Controllers;

[Route("feedback")]
public class FeedbackController : Controller
{
    private readonly IEmailSender emailSender;

    public FeedbackController(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }
    
    [HttpGet("")]
    public IActionResult FeedbackForm_Get()
    {
        var viewModel = new FeedbackFormViewModel();
        return View("FeedbackForm", viewModel);
    }

    [HttpPost("")]
    public IActionResult FeedbackForm_Post(FeedbackFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("FeedbackForm", viewModel);
        }
        
        emailSender.SendFeedbackFormResponseEmail(
            viewModel.WhatUserWasDoing, 
            viewModel.WhatUserToldUs);
        return RedirectToAction(nameof(FeedbackThankYou_Get), "Feedback");
    }

    [HttpGet("thank-you")]
    public IActionResult FeedbackThankYou_Get()
    {
        return View("FeedbackThankYou");
    }
}