using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.Feedback;

namespace SeaPublicWebsite.Controllers;

[Route("feedback")]
public class FeedbackController : Controller
{
    [HttpGet("")]
    public IActionResult FeedbackForm_Get()
    {
        var viewModel = new FeedbackFormViewModel();
        return View("FeedbackForm", viewModel);
    }

    [HttpPost("")]
    public IActionResult FeedbackForm_Post(FeedbackFormViewModel viewModel)
    {
        return RedirectToAction(nameof(FeedbackThankYou_Get), "Feedback");
    }

    [HttpGet("thank-you")]
    public IActionResult FeedbackThankYou_Get()
    {
        return View("FeedbackThankYou");
    }
}