using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.Feedback;

namespace SeaPublicWebsite.Controllers;

[Route("feedback")]
public class FeedbackController : Controller
{
    [HttpGet("send-feedback")]
    public IActionResult SendFeedback_Get()
    {
        var viewModel = new FeedbackFormViewModel();
        return View("FeedbackForm", viewModel);
    }

    [HttpPost("send-feedback")]
    public IActionResult SendFeedback_Post(FeedbackFormViewModel viewModel)
    {
        return View("FeedbackForm", viewModel);
    }
}