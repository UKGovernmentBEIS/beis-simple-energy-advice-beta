using GovUkDesignSystem;
using GovUkDesignSystem.Parsers;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency;

namespace SeaPublicWebsite.Controllers
{
    [Route("energy-efficiency")]
    public class EnergyEfficiencyController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }
        
        [HttpGet("postcode")]
        public IActionResult AskForPostcode_Get()
        {
            var viewModel = new AskForPostcodeViewModel();
            
            return View("AskForPostcode", viewModel);
        }
        
        [HttpPost("postcode")]
        public IActionResult AskForPostcode_Post(AskForPostcodeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Postcode);

            if (viewModel.HasAnyErrors())
            {
                return View("AskForPostcode", viewModel);
            }

            if (!PostcodesIoApi.IsValidPostcode(viewModel.Postcode))
            {
                viewModel.AddErrorFor(m => m.Postcode, "Enter a valid UK post code");
            }

            if (viewModel.HasAnyErrors())
            {
                return View("AskForPostcode", viewModel);
            }

            return Json(new {Success = "Yes!"});
        }
    }
}