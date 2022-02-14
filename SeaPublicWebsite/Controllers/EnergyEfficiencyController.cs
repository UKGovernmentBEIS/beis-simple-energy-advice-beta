using GovUkDesignSystem;
using GovUkDesignSystem.Parsers;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

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

        [HttpGet("service-unsuitable")]
        public IActionResult ServiceUnsuitable()
        {
            return View("ServiceUnsuitable");
        }

        [HttpGet("your-property")]
        public IActionResult YourPropertyCover()
        {
            return View("YourPropertyCover");
        }

        [HttpGet("answer-summary")]
        public IActionResult AnswerSummary()
        {
            return View("AnswerSummary");
        }

        [HttpGet("ownership-status")]
        public IActionResult OwnershipStatus_Get()
        {
            var viewModel = new OwnershipStatusViewModel();

            return View("OwnershipStatus", viewModel);
        }

        [HttpPost("ownership-status")]
        public IActionResult OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("OwnershipStatus", viewModel);
            }

            if (viewModel.Answer == OwnershipStatus.PrivateTenancy)
            {
                return RedirectToAction("ServiceUnsuitable");
            }

            return RedirectToAction("Country_Get");
        }

        [HttpGet("country")]
        public IActionResult Country_Get()
        {
            var viewModel = new CountryViewModel();

            return View("Country", viewModel);
        }

        [HttpPost("country")]
        public IActionResult Country_Post(CountryViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("Country", viewModel);
            }

            if (viewModel.Answer != Country.England && viewModel.Answer != Country.Wales)
            {
                return RedirectToAction("ServiceUnsuitable");
            }
            return RedirectToAction("UserGoal_Get");
        }

        [HttpGet("user-goal")]
        public IActionResult UserGoal_Get()
        {
            var viewModel = new UserGoalViewModel();

            return View("UserGoal", viewModel);
        }

        [HttpPost("user-goal")]
        public IActionResult UserGoal_Post(UserGoalViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("UserGoal", viewModel);
            }


            return RedirectToAction("UserInterests_Get");
        }

        [HttpGet("user-interests")]
        public IActionResult UserInterests_Get()
        {
            var viewModel = new UserInterestsViewModel();

            return View("UserInterests", viewModel);
        }

        [HttpPost("user-interests")]
        public IActionResult UserInterests_Post(UserInterestsViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("UserInterests", viewModel);
            }

            return RedirectToAction("YourPropertyCover");
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
            viewModel.ParseAndValidateParameters(Request, m => m.HouseNameOrNumber);

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

            var address = GetAddressApi.getAddress(viewModel.Postcode, viewModel.HouseNameOrNumber);


            return RedirectToAction("ConfirmAddress_Get", address);
        }

        [HttpGet("address")]
        public IActionResult ConfirmAddress_Get(Address address)
        {
            var viewModel = new ConfirmAddressViewModel(address);

            return View("ConfirmAddress", viewModel);
        }

        [HttpGet("home-type")]
        public IActionResult HomeType_Get()
        {
            var viewModel = new HomeTypeViewModel();

            return View("HomeType", viewModel);
        }

        [HttpPost("home-type")]
        public IActionResult HomeType_Post(HomeTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("HomeType", viewModel);
            }

            switch (viewModel.Answer)
            {
                case HomeType.House:
                    return RedirectToAction("HouseType_Get");
                case HomeType.Bungalow:
                    return RedirectToAction("BungalowType_Get");
                case HomeType.FlatDuplexOrMaisonette:
                    return RedirectToAction("FlatDuplexOrMaisonetteType_Get");
                case HomeType.ParkHomeOrMobileHome:
                    return RedirectToAction("ParkHomeOrMobileHomeType_Get");
                default:
                    return RedirectToAction("HomeAge_Get");
            }
        }

        [HttpGet("house-type")]
        public IActionResult HouseType_Get()
        {
            var viewModel = new HouseTypeViewModel();

            return View("HouseType", viewModel);
        }

        [HttpPost("house-type")]
        public IActionResult HouseType_Post(HouseTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("HouseType", viewModel);
            }

            return RedirectToAction("HomeAge_Get");
        }

        [HttpGet("bungalow-type")]
        public IActionResult BungalowType_Get()
        {
            var viewModel = new BungalowTypeViewModel();

            return View("BungalowType", viewModel);
        }

        [HttpPost("bungalow-type")]
        public IActionResult BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("BungalowType", viewModel);
            }

            return RedirectToAction("HomeAge_Get");
        }

        [HttpGet("flat-type")]
        public IActionResult FlatType_Get()
        {
            var viewModel = new FlatTypeViewModel();

            return View("FlatType", viewModel);
        }

        [HttpPost("flat-type")]
        public IActionResult FlatType_Post(FlatTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("FlatType", viewModel);
            }

            return RedirectToAction("HomeAge_Get");
        }

        [HttpGet("home-age")]
        public IActionResult HomeAge_Get()
        {
            var viewModel = new HomeAgeViewModel();

            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age")]
        public IActionResult HomeAge_Post(HomeAgeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.HomeAge);

            if (viewModel.HasAnyErrors())
            {
                return View("HomeAge", viewModel);
            }

            return RedirectToAction("WallType_Get");
        }

        [HttpGet("wall-type")]
        public IActionResult WallType_Get()
        {
            var viewModel = new WallTypeViewModel();

            return View("WallType", viewModel);
        }

        [HttpPost("wall-type")]
        public IActionResult WallType_Post(WallTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("WallType", viewModel);
            }

            return RedirectToAction("RoofConstruction_Get");
        }

        [HttpGet("roof-construction")]
        public IActionResult RoofConstruction_Get()
        {
            var viewModel = new RoofConstructionViewModel();

            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction")]
        public IActionResult RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("RoofConstruction", viewModel);
            }

            return RedirectToAction("RoofInsulated_Get");
        }

        [HttpGet("roof-insulated")]
        public IActionResult RoofInsulated_Get()
        {
            var viewModel = new RoofInsulatedViewModel();

            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated")]
        public IActionResult RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("RoofInsulated", viewModel);
            }

            return RedirectToAction("OutdoorSpace_Get");
        }

        [HttpGet("outdoor-space")]
        public IActionResult OutdoorSpace_Get()
        {
            var viewModel = new OutdoorSpaceViewModel();

            return View("OutdoorSpace", viewModel);
        }

        [HttpPost("outdoor-space")]
        public IActionResult OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("OutdoorSpace", viewModel);
            }

            return RedirectToAction("GlazingType_Get");
        }

        [HttpGet("glazing-type")]
        public IActionResult GlazingType_Get()
        {
            var viewModel = new GlazingTypeViewModel();

            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type")]
        public IActionResult GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("GlazingType", viewModel);
            }

            return RedirectToAction("HeatingType_Get");
        }

        [HttpGet("heating-type")]
        public IActionResult HeatingType_Get()
        {
            var viewModel = new HeatingTypeViewModel();

            return View("HeatingType", viewModel);
        }

        [HttpPost("heating-type")]
        public IActionResult HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("HeatingType", viewModel);
            }

            return RedirectToAction("HotWaterCylinder_Get");
        }

        [HttpGet("hot-water-cylinder")]
        public IActionResult HotWaterCylinder_Get()
        {
            var viewModel = new HotWaterCylinderViewModel();

            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder")]
        public IActionResult HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("HotWaterCylinder", viewModel);
            }

            return RedirectToAction("HeatingPattern_Get");
        }

        [HttpGet("heating-pattern")]
        public IActionResult HeatingPattern_Get()
        {
            var viewModel = new HeatingPatternViewModel();

            return View("HeatingPattern", viewModel);
        }

        [HttpPost("heating-pattern")]
        public IActionResult HeatingPattern_Post(HeatingPatternViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Answer);

            if (viewModel.HasAnyErrors())
            {
                return View("HeatingPattern", viewModel);
            }

            return RedirectToAction("Temperature_Get");
        }

        [HttpGet("temperature")]
        public IActionResult Temperature_Get()
        {
            var viewModel = new TemperatureViewModel();

            return View("Temperature", viewModel);
        }

        [HttpPost("temperature")]
        public IActionResult Temperature_Post(TemperatureViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.Temperature);

            if (viewModel.HasAnyErrors())
            {
                return View("Temperature", viewModel);
            }

            return RedirectToAction("AnswerSummary");
        }
    }
}