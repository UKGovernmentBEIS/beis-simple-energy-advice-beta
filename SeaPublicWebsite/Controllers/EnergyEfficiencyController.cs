using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Helpers;
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
        private readonly IEpcApi epcApi;
        private readonly IEmailSender emailApi;

        public EnergyEfficiencyController(UserDataStore userDataStore, IEpcApi epcApi, IEmailSender emailApi)
        {
            this.userDataStore = userDataStore;
            this.emailApi = emailApi;
            this.epcApi = epcApi;
        }
        
        
        [HttpGet("")]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                ServiceName = Constants.SERVICE_NAME
            };
           return View("Index", viewModel);
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
            if (!ModelState.IsValid)
            {
                return View("NewOrReturningUser", viewModel);
            }

            if (viewModel.NewOrReturningUser == NewOrReturningUser.ReturningUser)
            {
                if (!userDataStore.IsReferenceValid(viewModel.Reference))
                {
                    ModelState.AddModelError(nameof(NewOrReturningUserViewModel.Reference), "Check you have typed the reference correctly. Reference must be 8 characters.");
                    return View("NewOrReturningUser", viewModel);
                }
                
                return RedirectToAction("YourSavedRecommendations_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }

            string reference = userDataStore.GenerateNewReferenceAndSaveEmptyUserData();
            
            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = userDataModel.OwnershipStatus,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("OwnershipStatus", viewModel);
        }

        [HttpPost("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("OwnershipStatus", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.OwnershipStatus = viewModel.OwnershipStatus;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.OwnershipStatus == OwnershipStatus.PrivateTenancy)
            {
                return RedirectToAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "OwnershipStatus", reference = viewModel.Reference});
            }

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("AskForPostcode_Get", "EnergyEfficiency", new {reference = viewModel.Reference});
        }

        
        [HttpGet("country/{reference}")]
        public IActionResult Country_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new CountryViewModel
            {
                Country = userDataModel.Country,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("Country", viewModel);
        }

        [HttpPost("country/{reference}")]
        public IActionResult Country_Post(CountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Country", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Country = viewModel.Country;
            userDataStore.SaveUserData(userDataModel);
            
            if (viewModel.Country != Country.England && viewModel.Country != Country.Wales)
            {
                return RedirectToAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "Country", reference = viewModel.Reference});
            }

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("OwnershipStatus_Get", "EnergyEfficiency", new {reference = viewModel.Reference});
        }


        [HttpGet("service-unsuitable/{from}/{reference}")]
        public IActionResult ServiceUnsuitable(string from, string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            ViewBag.From = from;
            ViewBag.Country = userDataModel.Country;
            
            return View("ServiceUnsuitable", reference);
        }

        [HttpGet("postcode/{reference}")]
        public IActionResult AskForPostcode_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new AskForPostcodeViewModel
            {
                Postcode = userDataModel.Postcode,
                Reference = userDataModel.Reference
            };

            return View("AskForPostcode", viewModel);
        }

        [HttpPost("postcode/{reference}")]
        public IActionResult AskForPostcode_Post(AskForPostcodeViewModel viewModel)
        {
            if (!PostcodesIoApi.IsValidPostcode(viewModel.Postcode))
            {
                ModelState.AddModelError(nameof(AskForPostcodeViewModel.Postcode), "Enter a valid UK post code");
            }
            
            if (!ModelState.IsValid)
            {
                return View("AskForPostcode", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Postcode = viewModel.Postcode;
            userDataStore.SaveUserData(userDataModel);

            return RedirectToAction("ConfirmAddress_Get", "EnergyEfficiency", new {reference = viewModel.Reference, houseNameOrNumber = viewModel.HouseNameOrNumber});
        }

        
        [HttpGet("address/{reference}")]
        public async Task<IActionResult> ConfirmAddress_Get(string reference, string houseNameOrNumber)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var epcList = await epcApi.GetEpcsForPostcode(userDataModel.Postcode);

            if (houseNameOrNumber != null)
            {
                var filteredEpcList = epcList.Where(e =>
                    e.Address1.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase) || e.Address2.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase)).ToList();

                epcList = filteredEpcList.Any() ? filteredEpcList : epcList;
            }

            var viewModel = new ConfirmAddressViewModel
            {
                Reference = reference,
                EPCList = epcList,
                SelectedEpcId = epcList.Count == 1 ? epcList[0].EpcId : null,
            };

            return View("ConfirmAddress", viewModel);
        }

        [HttpPost("address/{reference}")]
        public async Task<IActionResult> ConfirmAddress_Post(ConfirmAddressViewModel viewModel)
        {
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            var epc = (await epcApi.GetEpcsForPostcode(userDataModel.Postcode)).FirstOrDefault(e => e.EpcId == viewModel.SelectedEpcId);
            userDataModel.Epc = epc;

            userDataStore.SaveUserData(userDataModel);

            return RedirectToAction("PropertyType_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
        }


        [HttpGet("property-type/{reference}")]
        public IActionResult PropertyType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                Reference = reference,
                Change = change
            };

            return View("PropertyType", viewModel);
        }

        [HttpPost("property-type/{reference}")]
        public IActionResult PropertyType_Post(PropertyTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("PropertyType", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.PropertyType = viewModel.PropertyType;
            userDataStore.SaveUserData(userDataModel);

            switch (viewModel.PropertyType)
            {
                case PropertyType.House:
                    return RedirectToAction("HouseType_Get", new {reference = viewModel.Reference, change = viewModel.Change});
                case PropertyType.Bungalow:
                    return RedirectToAction("BungalowType_Get", new {reference = viewModel.Reference, change = viewModel.Change});
                case PropertyType.ApartmentFlatOrMaisonette:
                    return RedirectToAction("FlatType_Get", new {reference = viewModel.Reference, change = viewModel.Change});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("house-type/{reference}")]
        public IActionResult HouseType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HouseTypeViewModel
            {
                HouseType = userDataModel.HouseType,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("HouseType", viewModel);
        }

        [HttpPost("house-type/{reference}")]
        public IActionResult HouseType_Post(HouseTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("HouseType", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HouseType = viewModel.HouseType;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public IActionResult BungalowType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = userDataModel.BungalowType,
                Reference = reference,
                Change = change
            };

            return View("BungalowType", viewModel);
        }

        [HttpPost("bungalow-type/{reference}")]
        public IActionResult BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("BungalowType", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.BungalowType = viewModel.BungalowType;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("flat-type/{reference}")]
        public IActionResult FlatType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FlatTypeViewModel
            {
                FlatType = userDataModel.FlatType,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("FlatType", viewModel);
        }

        [HttpPost("flat-type/{reference}")]
        public IActionResult FlatType_Post(FlatTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("FlatType", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FlatType = viewModel.FlatType;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("HomeAge_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("home-age/{reference}")]
        public IActionResult HomeAge_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age/{reference}")]
        public IActionResult HomeAge_Post(HomeAgeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("HomeAge", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.YearBuilt = viewModel.YearBuilt;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("WallConstruction_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("wall-construction/{reference}")]
        public IActionResult WallConstruction_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new WallConstructionViewModel
            {
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("WallConstruction", viewModel);
        }

        [HttpPost("wall-construction/{reference}")]
        public IActionResult WallConstruction_Post(WallConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("WallConstruction", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.WallConstruction = viewModel.WallConstruction;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference});
            }
            else if (viewModel.WallConstruction == WallConstruction.Cavity ||
                     viewModel.WallConstruction == WallConstruction.Mixed)
            {
                return RedirectToAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else if (viewModel.WallConstruction == WallConstruction.Solid)
            {
                return RedirectToAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else
            {
                // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
                if (userDataModel.PropertyType == PropertyType.House ||
                    userDataModel.PropertyType == PropertyType.Bungalow ||
                    (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.GroundFloor))
                {
                    return RedirectToAction("FloorConstruction_Get", new { reference = viewModel.Reference });
                }
                else if (userDataModel.PropertyType == PropertyType.House ||
                         userDataModel.PropertyType == PropertyType.Bungalow ||
                         (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.TopFloor))
                {
                    return RedirectToAction("RoofConstruction_Get", new { reference = viewModel.Reference });
                }
                else
                {
                    return RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
                }
            }
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = userDataModel.CavityWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("CavityWallsInsulated", viewModel);
        }

        [HttpPost("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Post(CavityWallsInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("CavityWallsInsulated", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.CavityWallsInsulated = viewModel.CavityWallsInsulated;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else if (viewModel.WallConstruction == WallConstruction.Mixed)
            {
                return RedirectToAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else
            {
                // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
                if (userDataModel.PropertyType == PropertyType.House ||
                    userDataModel.PropertyType == PropertyType.Bungalow ||
                    (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.GroundFloor))
                {
                    return RedirectToAction("FloorConstruction_Get", new { reference = viewModel.Reference });
                }
                else if (userDataModel.PropertyType == PropertyType.House ||
                         userDataModel.PropertyType == PropertyType.Bungalow ||
                         (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.TopFloor))
                {
                    return RedirectToAction("RoofConstruction_Get", new { reference = viewModel.Reference });
                }
                else
                {
                    return RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
                }
            }
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new SolidWallsInsulatedViewModel
            {
                SolidWallsInsulated = userDataModel.SolidWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("SolidWallsInsulated", viewModel);
        }

        [HttpPost("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Post(SolidWallsInsulatedViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {
                return View("SolidWallsInsulated", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.SolidWallsInsulated = viewModel.SolidWallsInsulated;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else if (userDataModel.PropertyType == PropertyType.House ||
                     userDataModel.PropertyType == PropertyType.Bungalow ||
                     (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.GroundFloor))
            {
                return RedirectToAction("FloorConstruction_Get", new { reference = viewModel.Reference });
            }
            else if (userDataModel.PropertyType == PropertyType.House ||
                     userDataModel.PropertyType == PropertyType.Bungalow ||
                     (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.TopFloor))
            {
                return RedirectToAction("RoofConstruction_Get", new { reference = viewModel.Reference });
            }
            else
            {
                return RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
            }
        }


        [HttpGet("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = userDataModel.FloorConstruction,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("FloorConstruction", viewModel);
        }

        [HttpPost("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Post(FloorConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("FloorConstruction", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorConstruction = viewModel.FloorConstruction;
            userDataStore.SaveUserData(userDataModel);
            
            if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else if (userDataModel.FloorConstruction == FloorConstruction.SolidConcrete 
                || userDataModel.FloorConstruction == FloorConstruction.SuspendedTimber 
                || userDataModel.FloorConstruction == FloorConstruction.Mix ) 
            {
                return RedirectToAction("FloorInsulated_Get", new { reference = viewModel.Reference });
            }
            else if (userDataModel.PropertyType == PropertyType.House ||
                     userDataModel.PropertyType == PropertyType.Bungalow ||
                     (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.TopFloor))
            {
                return RedirectToAction("RoofConstruction_Get", new { reference = viewModel.Reference });
            }
            else
            {
                return RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
            }
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = userDataModel.FloorInsulated,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("FloorInsulated", viewModel);
        }

        [HttpPost("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Post(FloorInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("FloorInsulated", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorInsulated = viewModel.FloorInsulated;
            userDataStore.SaveUserData(userDataModel);
            
            if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference });
            }
            else if (userDataModel.PropertyType == PropertyType.House ||
                     userDataModel.PropertyType == PropertyType.Bungalow ||
                     (userDataModel.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userDataModel.FlatType == FlatType.TopFloor))
            {
                return RedirectToAction("RoofConstruction_Get", new { reference = viewModel.Reference });
            }
            else
            {
                return RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
            }
        }

        [HttpGet("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofConstructionViewModel
            {
                PropertyType = userDataModel.PropertyType,
                FlatType = userDataModel.FlatType,
                RoofConstruction = userDataModel.RoofConstruction,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("RoofConstruction", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofConstruction = viewModel.RoofConstruction;
            userDataStore.SaveUserData(userDataModel);

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : userDataModel.RoofConstruction == RoofConstruction.Flat 
                    ? RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference }) 
                    :  RedirectToAction("AccessibleLoftSpace_Get", new {reference = viewModel.Reference});
        }

        [HttpGet("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new AccessibleLoftSpaceViewModel
            {
                AccessibleLoftSpace = userDataModel.AccessibleLoftSpace,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("AccessibleLoftSpace", viewModel);
        }

        [HttpPost("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Post(AccessibleLoftSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("AccessibleLoftSpace", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.AccessibleLoftSpace = viewModel.AccessibleLoftSpace;
            userDataStore.SaveUserData(userDataModel);

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : userDataModel.AccessibleLoftSpace == AccessibleLoftSpace.Yes
                    ? RedirectToAction("RoofInsulated_Get", new {reference = viewModel.Reference})
                    : RedirectToAction("GlazingType_Get", new { reference = viewModel.Reference });
        }

        [HttpGet("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = userDataModel.RoofInsulated,
                Reference = userDataModel.Reference,
                Change = change,
                YearBuilt = userDataModel.YearBuilt
            };

            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("RoofInsulated", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofInsulated = viewModel.RoofInsulated;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("GlazingType_Get", new {reference = viewModel.Reference});
        }

        [HttpGet("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = userDataModel.HasOutdoorSpace,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("OutdoorSpace", viewModel);
        }

        [HttpPost("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("OutdoorSpace", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("HeatingType_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("glazing-type/{reference}")]
        public IActionResult GlazingType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = userDataModel.GlazingType,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type/{reference}")]
        public IActionResult GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("GlazingType", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.GlazingType = viewModel.GlazingType;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("OutdoorSpace_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("heating-type/{reference}")]
        public IActionResult HeatingType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = userDataModel.HeatingType,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("HeatingType", viewModel);
        }

        [HttpPost("heating-type/{reference}")]
        public IActionResult HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("HeatingType", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingType = viewModel.HeatingType;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.HeatingType == HeatingType.Other)
            {
                return RedirectToAction("OtherHeatingType_Get",
                    new {reference = viewModel.Reference, change = viewModel.Change});
            }
            else if (viewModel.Change)
            {
                return RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference});
            }
            else if (viewModel.HeatingType == HeatingType.GasBoiler ||
                     viewModel.HeatingType == HeatingType.OilBoiler ||
                     viewModel.HeatingType == HeatingType.LpgBoiler)
            {
                return RedirectToAction("HotWaterCylinder_Get",
                    new {reference = viewModel.Reference, change = viewModel.Change});
            }
            else
            {
                return RedirectToAction("NumberOfOccupants_Get", new {reference = viewModel.Reference});
            }
        }

        [HttpGet("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new OtherHeatingTypeViewModel
            {
                OtherHeatingType = userDataModel.OtherHeatingType,
                Reference = userDataModel.Reference,
                Change = change,
                Epc = userDataModel.Epc
            };

            return View("OtherHeatingType", viewModel);
        }

        [HttpPost("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Post(OtherHeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("OtherHeatingType", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.OtherHeatingType = viewModel.OtherHeatingType;
            userDataStore.SaveUserData(userDataModel);

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference })
                : RedirectToAction("NumberOfOccupants_Get", new { reference = viewModel.Reference });
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = userDataModel.HasHotWaterCylinder,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("HotWaterCylinder", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("NumberOfOccupants_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = userDataModel.NumberOfOccupants,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("NumberOfOccupants", viewModel);
        }

        [HttpPost("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Post(NumberOfOccupantsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("NumberOfOccupants", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.NumberOfOccupants = viewModel.NumberOfOccupants;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("HeatingPattern_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = userDataModel.HeatingPattern,
                HoursOfHeating = userDataModel.HoursOfHeating,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("HeatingPattern", viewModel);
        }

        [HttpPost("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Post(HeatingPatternViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("HeatingPattern", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingPattern = viewModel.HeatingPattern;
            userDataModel.HoursOfHeating = viewModel.HeatingPattern == HeatingPattern.Other ? viewModel.HoursOfHeating : null;

            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("Temperature_Get", new {reference = viewModel.Reference});
        }

        
        [HttpGet("temperature/{reference}")]
        public IActionResult Temperature_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new TemperatureViewModel
            {
                Temperature = userDataModel.Temperature,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("Temperature", viewModel);
        }

        [HttpPost("temperature/{reference}")]
        public IActionResult Temperature_Post(TemperatureViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Temperature", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Temperature = viewModel.Temperature;
            userDataStore.SaveUserData(userDataModel);
            
            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new {reference = viewModel.Reference})
                : RedirectToAction("AnswerSummary", new { reference = viewModel.Reference });
        }


        [HttpGet("email-address/{reference}")]
        public IActionResult EmailAddress_Get(string reference, bool change = false)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new EmailAddressViewModel
            {
                HasEmailAddress = userDataModel.HasEmailAddress,
                EmailAddress = userDataModel.EmailAddress,
                Reference = userDataModel.Reference,
                Change = change
            };

            return View("EmailAddress", viewModel);
        }

        [HttpPost("email-address/{reference}")]
        public IActionResult EmailAddress_Post(EmailAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("EmailAddress", viewModel);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasEmailAddress = viewModel.HasEmailAddress;
            userDataModel.EmailAddress = viewModel.HasEmailAddress == HasEmailAddress.Yes ? viewModel.EmailAddress : null;
            userDataStore.SaveUserData(userDataModel);

            return viewModel.Change
                ? RedirectToAction("AnswerSummary", "EnergyEfficiency", new { reference = viewModel.Reference })
                : RedirectToAction("AnswerSummary", new { reference = viewModel.Reference });
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
                    LifetimeSaving = r.LifetimeSaving,
                    Lifetime = r.Lifetime,
                    Summary = r.Summary
                }
            ).ToList();
            userDataStore.SaveUserData(userDataModel);

            var viewModel = new YourRecommendationsViewModel
                {
                    Reference = reference,
                    NumberOfUserRecommendations = recommendationsForUser.Count,
                    FirstReferenceId = (int)recommendationsForUser[0].Key,
                    HasEmailAddress = userDataModel.HasEmailAddress,
                    EmailAddress = userDataModel.EmailAddress
                }
;            return View("YourRecommendations", viewModel);
        }

        [HttpPost("your-recommendations/{reference}")]
        public IActionResult YourRecommendations_Post(YourRecommendationsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("YourRecommendations", viewModel);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasEmailAddress = viewModel.HasEmailAddress;
            userDataModel.EmailAddress = viewModel.HasEmailAddress == HasEmailAddress.Yes ? viewModel.EmailAddress : null;
            userDataStore.SaveUserData(userDataModel);

            if (viewModel.HasEmailAddress == HasEmailAddress.Yes)
            {
                emailApi.SendReferenceNumberEmail(userDataModel.EmailAddress, userDataModel.Reference);
            }
            
            return RedirectToAction("Recommendation_Get", new { id = viewModel.FirstReferenceId, reference = viewModel.Reference });
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
            
            if (!ModelState.IsValid)
            {
                return View("recommendations/" + Enum.GetName(viewModel.UserRecommendation.Key), viewModel);
            }

            userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey) id).RecommendationAction =
                viewModel.RecommendationAction;
            userDataStore.SaveUserData(userDataModel);

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = userDataModel.Reference });
        }

        [HttpGet("recommendation/remove-from-plan/{id}/{reference}")]
        public IActionResult RemoveFromPlan_Get(int id, string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var recommendationToUpdate = userDataModel.UserRecommendations.First(r => r.Key == (RecommendationKey)id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.Discard;
            userDataStore.SaveUserData(userDataModel);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = userDataModel.Reference });
        }
    }
}