using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.Models.Feedback;

namespace SeaPublicWebsite.Controllers;

[Route("")]
public class FeedbackController : Controller
{
    private readonly IEmailSender emailSender;

    public FeedbackController(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }
    
    [HttpGet("feedback")]
    public IActionResult FeedbackForm_Get()
    {
        var viewModel = new FeedbackFormViewModel();
        ViewBag.OnFeedbackPage = true;
        return View("FeedbackForm", viewModel);
    }

    [HttpPost("feedback")]
    public IActionResult FeedbackForm_Post(FeedbackFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return FeedbackForm_Get();
        }
        
        emailSender.SendFeedbackFormResponseEmail(
            viewModel.WhatUserWasDoing, 
            viewModel.WhatUserToldUs);
        return RedirectToAction(nameof(FeedbackThankYou_Get), "Feedback");
    }
    
    [HttpGet("feedback-survey")]
    public IActionResult FeedbackSurvey_Get()
    {
        var viewModel = new FeedbackSurveyViewModel();
        ViewBag.OnFeedbackPage = true;
        return View("FeedbackSurvey", viewModel);
    }

    [HttpPost("feedback-survey")]
    public IActionResult FeedbackSurvey_Post(FeedbackSurveyViewModel viewModel)
    {
        if (!viewModel.VisitReasonList.Any())
        {
            ModelState.AddModelError(nameof(viewModel.VisitReasonList), "Select why you came to the site today");
        }
        if (!viewModel.HowInformationHelpedList.Any())
        {
            ModelState.AddModelError(nameof(viewModel.HowInformationHelpedList), "Select how has the information helped you today");
        }
        if (!viewModel.WhatPlannedToDoList.Any())
        {
            ModelState.AddModelError(nameof(viewModel.WhatPlannedToDoList), "Select what you plan to do with the information");
        }
        if (!ModelState.IsValid)
        {
            return FeedbackSurvey_Get();
        }
        
        emailSender.SendFeedbackSurveyResponseEmail(viewModel);
        return RedirectToAction(nameof(FeedbackThankYou_Get), "Feedback");
    }

    [HttpGet("thank-you")]
    public IActionResult FeedbackThankYou_Get()
    {
        return View("FeedbackThankYou");
    }
}