using System;
using System.Linq;
using GovUkDesignSystem;
using GovUkDesignSystem.Parsers;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using SeaPublicWebsite.Models.EnergyEfficiency.Recommendations;
using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.Controllers
{
    [Route("energy-efficiency")]
    public class EnergyEfficiencyController : Controller
    {
        private readonly UserDataStore userDataStore;

        public EnergyEfficiencyController(UserDataStore userDataStore)
        {
            this.userDataStore = userDataStore;
        }
        
        
        [HttpGet("")]
        public IActionResult Index()
        {
           return View("Index");
        }

        
        [HttpGet("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Get()
        {
            var viewModel = new NewOrReturningUserViewModel();
            return View("NewOrReturningUser", viewModel);
        }

        [HttpPost("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Post(NewOrReturningUserViewModel viewModel)
        {
            viewModel.ParseAndValidateParameters(Request, m => m.NewOrReturningUser);

            if (viewModel.HasAnyErrors())
            {
                return View("NewOrReturningUser", viewModel);
            }

            if (viewModel.NewOrReturningUser == NewOrReturningUser.ReturningUser)
            {
                viewModel.ParseAndValidateParameters(Request, m => m.Reference);

                if (viewModel.HasAnyErrors())
                {
                    return View("NewOrReturningUser", viewModel);
                }

                if (!userDataStore.IsReferenceValid(viewModel.Reference))
                {
                    viewModel.AddErrorFor(m => m.Reference, "We could not find this reference. Are you sure you have copied it correctly?");
                    return View("NewOrReturningUser", viewModel);
                }
                
                return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }

            string reference = userDataStore.GenerateNewReferenceAndSaveEmptyUserData();
            
            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = userDataModel.OwnershipStatus,
                Reference = userDataModel.Reference
            };

            return View("OwnershipStatus", viewModel);
        }

        [HttpPost("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.OwnershipStatus);

            if (viewModel.HasAnyErrors())
            {
                return View("OwnershipStatus", viewModel);
            }

            userDataModel.OwnershipStatus = viewModel.OwnershipStatus;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.OwnershipStatus == OwnershipStatus.PrivateTenancy)
            {
                return RedirectToAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "OwnershipStatus", reference = viewModel.Reference});
            }

            return RedirectToAction("AskForPostcode_Get", "EnergyEfficiency", new {reference = viewModel.Reference});
        }

        
        [HttpGet("country/{reference}")]
        public IActionResult Country_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new CountryViewModel
            {
                Country = userDataModel.Country,
                Reference = userDataModel.Reference
            };

            return View("Country", viewModel);
        }

        [HttpPost("country/{reference}")]
        public IActionResult Country_Post(CountryViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.Country);

            if (viewModel.HasAnyErrors())
            {
                return View("Country", viewModel);
            }

            userDataModel.Country = viewModel.Country;
            userDataStore.SaveUserData(userDataModel);
            
            if (viewModel.Country != Country.England && viewModel.Country != Country.Wales)
            {
                return RedirectToAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "Country", reference = viewModel.Reference});
            }
            
            return RedirectToAction("OwnershipStatus_Get", "EnergyEfficiency", new {reference = viewModel.Reference});
        }


        [HttpGet("service-unsuitable/{from}/{reference}")]
        public IActionResult ServiceUnsuitable(string from, string reference)
        {
            ViewBag.From = from;
            return View("ServiceUnsuitable", reference);
        }

        [HttpGet("postcode/{reference}")]
        public IActionResult AskForPostcode_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new AskForPostcodeViewModel
            {
                Postcode = userDataModel.Address?.postcode,
                Reference = userDataModel.Reference
            };

            return View("AskForPostcode", viewModel);
        }

        [HttpPost("postcode/{reference}")]
        public IActionResult AskForPostcode_Post(AskForPostcodeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.Postcode);
            viewModel.ParseAndValidateParameters(Request, m => m.HouseNameOrNumber);

            if (viewModel.HasAnyErrors())
            {
                return View("AskForPostcode", viewModel);
            }

            if (!PostcodesIoApi.IsValidPostcode(viewModel.Postcode))
            {
                viewModel.AddErrorFor(m => m.Postcode, "Enter a valid UK post code");
                return View("AskForPostcode", viewModel);
            }

            Address address = GetAddressApi.getAddress(viewModel.Postcode, viewModel.HouseNameOrNumber);

            userDataModel.Address = address;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("ConfirmAddress_Get", "EnergyEfficiency", new {reference = viewModel.Reference});
        }

        
        [HttpGet("address/{reference}")]
        public IActionResult ConfirmAddress_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new ConfirmAddressViewModel
            {
                Reference = reference,
                Address = userDataModel.Address
            };

            return View("ConfirmAddress", viewModel);
        }

        
        [HttpGet("property-type/{reference}")]
        public IActionResult PropertyType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                Reference = reference
            };

            return View("PropertyType", viewModel);
        }

        [HttpPost("property-type/{reference}")]
        public IActionResult PropertyType_Post(PropertyTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.PropertyType);

            if (viewModel.HasAnyErrors())
            {
                return View("PropertyType", viewModel);
            }

            userDataModel.PropertyType = viewModel.PropertyType;
            userDataStore.SaveUserData(userDataModel);

            switch (viewModel.PropertyType)
            {
                case PropertyType.House:
                    return RedirectToAction("HouseType_Get", new {reference = viewModel.Reference});
                case PropertyType.Bungalow:
                    return RedirectToAction("BungalowType_Get", new {reference = viewModel.Reference});
                case PropertyType.ApartmentFlatOrMaisonette:
                    return RedirectToAction("FlatType_Get", new {reference = viewModel.Reference});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        
        [HttpGet("house-type/{reference}")]
        public IActionResult HouseType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HouseTypeViewModel
            {
                HouseType = userDataModel.HouseType,
                Reference = userDataModel.Reference
            };

            return View("HouseType", viewModel);
        }

        [HttpPost("house-type/{reference}")]
        public IActionResult HouseType_Post(HouseTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.HouseType);

            if (viewModel.HasAnyErrors())
            {
                return View("HouseType", viewModel);
            }

            userDataModel.HouseType = viewModel.HouseType;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public IActionResult BungalowType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = userDataModel.BungalowType,
                Reference = reference
            };

            return View("BungalowType", viewModel);
        }

        [HttpPost("bungalow-type/{reference}")]
        public IActionResult BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.BungalowType);

            if (viewModel.HasAnyErrors())
            {
                return View("BungalowType", viewModel);
            }

            userDataModel.BungalowType = viewModel.BungalowType;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("flat-type/{reference}")]
        public IActionResult FlatType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FlatTypeViewModel
            {
                FlatType = userDataModel.FlatType,
                Reference = userDataModel.Reference
            };

            return View("FlatType", viewModel);
        }

        [HttpPost("flat-type/{reference}")]
        public IActionResult FlatType_Post(FlatTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.FlatType);

            if (viewModel.HasAnyErrors())
            {
                return View("FlatType", viewModel);
            }

            userDataModel.FlatType = viewModel.FlatType;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("home-age/{reference}")]
        public IActionResult HomeAge_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference
            };

            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age/{reference}")]
        public IActionResult HomeAge_Post(HomeAgeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.YearBuilt);

            if (viewModel.HasAnyErrors())
            {
                return View("HomeAge", viewModel);
            }

            userDataModel.YearBuilt = viewModel.YearBuilt;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("WallType_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("wall-type/{reference}")]
        public IActionResult WallType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new WallTypeViewModel
            {
                WallType = userDataModel.WallType,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference
            };

            return View("WallType", viewModel);
        }

        [HttpPost("wall-type/{reference}")]
        public IActionResult WallType_Post(WallTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.WallType);

            if (viewModel.HasAnyErrors())
            {
                return View("WallType", viewModel);
            }

            userDataModel.WallType = viewModel.WallType;
            userDataStore.SaveUserData(userDataModel);

            if (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette
                && (userDataModel.FlatType == FlatType.GroundFloor || userDataModel.FlatType == FlatType.MiddleFloor))
            {
                return RedirectToAction("GlazingType_Get", new {reference = viewModel.Reference});
            }
            else
            {
                return RedirectToAction("RoofConstruction_Get", new {reference = viewModel.Reference});
            }
        }

        
        [HttpGet("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofConstructionViewModel
            {
                RoofConstruction = userDataModel.RoofConstruction,
                Reference = userDataModel.Reference
            };

            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.RoofConstruction);

            if (viewModel.HasAnyErrors())
            {
                return View("RoofConstruction", viewModel);
            }

            userDataModel.RoofConstruction = viewModel.RoofConstruction;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("RoofInsulated_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = userDataModel.RoofInsulated,
                Reference = userDataModel.Reference
            };

            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.RoofInsulated);

            if (viewModel.HasAnyErrors())
            {
                return View("RoofInsulated", viewModel);
            }

            userDataModel.RoofInsulated = viewModel.RoofInsulated;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("GlazingType_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = userDataModel.HasOutdoorSpace,
                Reference = userDataModel.Reference
            };

            return View("OutdoorSpace", viewModel);
        }

        [HttpPost("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.HasOutdoorSpace);

            if (viewModel.HasAnyErrors())
            {
                return View("OutdoorSpace", viewModel);
            }

            userDataModel.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("HeatingType_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("glazing-type/{reference}")]
        public IActionResult GlazingType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = userDataModel.GlazingType,
                Reference = userDataModel.Reference
            };

            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type/{reference}")]
        public IActionResult GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.GlazingType);

            if (viewModel.HasAnyErrors())
            {
                return View("GlazingType", viewModel);
            }

            userDataModel.GlazingType = viewModel.GlazingType;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("OutdoorSpace_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("heating-type/{reference}")]
        public IActionResult HeatingType_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = userDataModel.HeatingType,
                Reference = userDataModel.Reference
            };

            return View("HeatingType", viewModel);
        }

        [HttpPost("heating-type/{reference}")]
        public IActionResult HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.HeatingType);

            if (viewModel.HasAnyErrors())
            {
                return View("HeatingType", viewModel);
            }

            userDataModel.HeatingType = viewModel.HeatingType;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.HeatingType == HeatingType.GasBoiler ||
                viewModel.HeatingType == HeatingType.OilBoiler ||
                viewModel.HeatingType == HeatingType.LpgBoiler)
            {
                return RedirectToAction("HotWaterCylinder_Get", new {reference = viewModel.Reference});
            }
            else
            {
                return RedirectToAction("NumberOfOccupants_Get", new {reference = viewModel.Reference});
            }
        }

        
        [HttpGet("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = userDataModel.HasHotWaterCylinder,
                Reference = userDataModel.Reference
            };

            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.HasHotWaterCylinder);

            if (viewModel.HasAnyErrors())
            {
                return View("HotWaterCylinder", viewModel);
            }

            userDataModel.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("NumberOfOccupants_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = userDataModel.NumberOfOccupants,
                Reference = userDataModel.Reference
            };

            return View("NumberOfOccupants", viewModel);
        }

        [HttpPost("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Post(NumberOfOccupantsViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.NumberOfOccupants);

            if (viewModel.HasAnyErrors())
            {
                return View("NumberOfOccupants", viewModel);
            }

            userDataModel.NumberOfOccupants = viewModel.NumberOfOccupants;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("HeatingPattern_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = userDataModel.HeatingPattern,
                Reference = userDataModel.Reference
            };

            return View("HeatingPattern", viewModel);
        }

        [HttpPost("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Post(HeatingPatternViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.HeatingPattern);

            if (viewModel.HasAnyErrors())
            {
                return View("HeatingPattern", viewModel);
            }

            userDataModel.HeatingPattern = viewModel.HeatingPattern;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("Temperature_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("temperature/{reference}")]
        public IActionResult Temperature_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new TemperatureViewModel
            {
                Temperature = userDataModel.Temperature,
                Reference = userDataModel.Reference
            };

            return View("Temperature", viewModel);
        }

        [HttpPost("temperature/{reference}")]
        public IActionResult Temperature_Post(TemperatureViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            viewModel.ParseAndValidateParameters(Request, m => m.Temperature);

            if (viewModel.HasAnyErrors())
            {
                return View("Temperature", viewModel);
            };

            userDataModel.Temperature = viewModel.Temperature;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("AnswerSummary", new {reference = viewModel.Reference});
        }
        
        
        [HttpGet("answer-summary/{reference}")]
        public IActionResult AnswerSummary(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            return View("AnswerSummary", userDataModel);
        }

        
        [HttpGet("your-recommendations/{reference}")]
        public IActionResult YourRecommendations_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var recommendationsForUser = RecommendationService.GetRecommendationsForUser(userDataModel);
            userDataModel.UserRecommendations = recommendationsForUser.Select(r => 
                new UserRecommendation()
                {
                    Key = r.Key,
                    Title = r.Title,
                    MinInstallCost = r.MinInstallCost,
                    MaxInstallCost = r.MaxInstallCost,
                    Saving = r.Saving,
                    Summary = r.Summary
                }
            ).ToList();

            var viewModel = new YourRecommendationsViewModel
                {
                    Reference = reference,
                    NumberOfUserRecommendations = recommendationsForUser.Count,
                    FirstReferenceId = (int)recommendationsForUser[0].Key
                }
;            return View("YourRecommendations", viewModel);
        }

        [HttpGet("your-recommendations/{id}/{reference}")]
        public IActionResult Recommendation_Get(int id, string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var viewModel = new RecommendationViewModel
            {
                UserDataModel = userDataModel,
                UserRecommendation = userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey) id),
                RecommendationAction = userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey)id).RecommendationAction
            };

            var recommendationKey = (RecommendationKey) id;

            return View("recommendations/" + Enum.GetName(recommendationKey), viewModel);
        }

        [HttpPost("your-recommendations/{id}/{reference}")]
        public IActionResult Recommendation_Post(RecommendationViewModel viewModel, string command, int id)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.UserDataModel.Reference);
            viewModel.UserDataModel = userDataModel;
            viewModel.UserRecommendation =
                userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey)id);

            viewModel.ParseAndValidateParameters(Request, m => m.RecommendationAction);

            if (viewModel.HasAnyErrors())
            {
                return View("recommendations/" + Enum.GetName(viewModel.UserRecommendation.Key), viewModel);
            };

            userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey) id).RecommendationAction =
                viewModel.RecommendationAction;

            switch(command)
            {
                case "goForwards":
                    return RedirectToAction("Recommendation_Get",
                        new {id = (int) viewModel.NextRecommendationKey(), reference = userDataModel.Reference});
                case "goBackwards":
                    return RedirectToAction("Recommendation_Get",
                        new {id = (int) viewModel.PreviousRecommendationKey(), reference = userDataModel.Reference});
                case "goToActionPlan":
                    return RedirectToAction("YourSavedRecommendations_Get", new {reference = userDataModel.Reference});
            }

            return View("Recommendation", viewModel);
        }

        [HttpGet("your-saved-recommendations/{reference}")]
        public IActionResult YourSavedRecommendations_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new YourSavedRecommendationsViewModel
            {
                UserDataModel = userDataModel
            };
            return View("YourSavedRecommendations", viewModel);
        }

        [HttpGet("recommendation/add-to-plan/{id}/{reference}")]
        public IActionResult AddToPlan_Get(int id, string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var recommendationToUpdate = userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey) id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.SaveToActionPlan;
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = userDataModel.Reference });
        }

        [HttpGet("recommendation/remove-from-plan/{id}/{reference}")]
        public IActionResult RemoveFromPlan_Get(int id, string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var recommendationToUpdate = userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey)id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.Discard;
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = userDataModel.Reference });
        }
    }
}