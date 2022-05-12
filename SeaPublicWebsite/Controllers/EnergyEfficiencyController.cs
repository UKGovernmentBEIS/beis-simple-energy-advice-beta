using System;
using System.Linq;
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
        private readonly IQuestionFlowService questionFlowService;

        public EnergyEfficiencyController(UserDataStore userDataStore, IQuestionFlowService questionFlowService)
        {
            this.userDataStore = userDataStore;
            this.questionFlowService = questionFlowService;
        }
        
        
        [HttpGet("")]
        public IActionResult Index()
        {
           return View("Index");
        }

        
        [HttpGet("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Get()
        {
            var viewModel = new NewOrReturningUserViewModel
            {
                BackLink = questionFlowService.BackLink(QuestionFlowPage.NewOrReturningUser, new UserDataModel())
            };
            return View("NewOrReturningUser", viewModel);
        }

        [HttpPost("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Post(NewOrReturningUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return NewOrReturningUser_Get();
            }

            if (viewModel.NewOrReturningUser == NewOrReturningUser.ReturningUser)
            {
                if (!userDataStore.IsReferenceValid(viewModel.Reference))
                {
                    ModelState.AddModelError(nameof(NewOrReturningUserViewModel.Reference), "Check you have typed the reference correctly. Reference must be 8 characters.");
                    return NewOrReturningUser_Get();
                }
                
                return RedirectToAction("YourSavedRecommendations_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }

            string reference = userDataStore.GenerateNewReferenceAndSaveEmptyUserData();
            
            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = userDataModel.OwnershipStatus,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.OwnershipStatus, userDataModel, entryPoint)
            };

            return View("OwnershipStatus", viewModel);
        }

        [HttpPost("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return OwnershipStatus_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.OwnershipStatus = viewModel.OwnershipStatus;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.OwnershipStatus, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("country/{reference}")]
        public IActionResult Country_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new CountryViewModel
            {
                Country = userDataModel.Country,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.Country, userDataModel, entryPoint)
            };

            return View("Country", viewModel);
        }

        [HttpPost("country/{reference}")]
        public IActionResult Country_Post(CountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Country_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Country = viewModel.Country;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.Country, userDataModel, viewModel.EntryPoint));
        }


        [HttpGet("service-unsuitable/{reference}")]
        public IActionResult ServiceUnsuitable(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var viewModel = new ServiceUnsuitableViewModel
            {
                Reference = userDataModel.Reference,
                Country = userDataModel.Country,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.ServiceUnsuitable, userDataModel)
            };
            
            return View("ServiceUnsuitable", viewModel);
        }

        [HttpGet("postcode/{reference}")]
        public IActionResult AskForPostcode_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new AskForPostcodeViewModel
            {
                Postcode = userDataModel.Postcode,
                HouseNameOrNumber = userDataModel.HouseNameOrNumber,
                Reference = userDataModel.Reference,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.AskForPostcode, userDataModel)
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
                return AskForPostcode_Get(viewModel.Reference);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Postcode = viewModel.Postcode;
            userDataModel.HouseNameOrNumber = viewModel.HouseNameOrNumber;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.AskForPostcode, userDataModel));
        }

        
        [HttpGet("address/{reference}")]
        public IActionResult ConfirmAddress_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var epcList = OpenEpcApi.GetEpcsForPostcode(userDataModel.Postcode);
            var houseNameOrNumber = userDataModel.HouseNameOrNumber;
            
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
                BackLink = questionFlowService.BackLink(QuestionFlowPage.ConfirmAddress, userDataModel)
            };

            return View("ConfirmAddress", viewModel);
        }

        [HttpPost("address/{reference}")]
        public IActionResult ConfirmAddress_Post(ConfirmAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return ConfirmAddress_Get(viewModel.Reference);
            }
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            var epc = OpenEpcApi.GetEpcsForPostcode(userDataModel.Postcode).FirstOrDefault(e => e.EpcId == viewModel.SelectedEpcId);
            userDataModel.Epc = epc;

            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.ConfirmAddress, userDataModel));
        }


        [HttpGet("property-type/{reference}")]
        public IActionResult PropertyType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.PropertyType, userDataModel, entryPoint)
            };

            return View("PropertyType", viewModel);
        }

        [HttpPost("property-type/{reference}")]
        public IActionResult PropertyType_Post(PropertyTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PropertyType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.PropertyType = viewModel.PropertyType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.PropertyType, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("house-type/{reference}")]
        public IActionResult HouseType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HouseTypeViewModel
            {
                HouseType = userDataModel.HouseType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.HouseType, userDataModel, entryPoint)
            };

            return View("HouseType", viewModel);
        }

        [HttpPost("house-type/{reference}")]
        public IActionResult HouseType_Post(HouseTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HouseType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HouseType = viewModel.HouseType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.HouseType, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public IActionResult BungalowType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = userDataModel.BungalowType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.BungalowType, userDataModel, entryPoint)
            };

            return View("BungalowType", viewModel);
        }

        [HttpPost("bungalow-type/{reference}")]
        public IActionResult BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BungalowType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.BungalowType = viewModel.BungalowType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.BungalowType, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("flat-type/{reference}")]
        public IActionResult FlatType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FlatTypeViewModel
            {
                FlatType = userDataModel.FlatType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.FlatType, userDataModel, entryPoint)
            };

            return View("FlatType", viewModel);
        }

        [HttpPost("flat-type/{reference}")]
        public IActionResult FlatType_Post(FlatTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return FlatType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FlatType = viewModel.FlatType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.FlatType, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("home-age/{reference}")]
        public IActionResult HomeAge_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.HomeAge, userDataModel, entryPoint)
            };

            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age/{reference}")]
        public IActionResult HomeAge_Post(HomeAgeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HomeAge_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.YearBuilt = viewModel.YearBuilt;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.HomeAge, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("wall-construction/{reference}")]
        public IActionResult WallConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new WallConstructionViewModel
            {
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.WallConstruction, userDataModel, entryPoint)
            };

            return View("WallConstruction", viewModel);
        }

        [HttpPost("wall-construction/{reference}")]
        public IActionResult WallConstruction_Post(WallConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return WallConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.WallConstruction = viewModel.WallConstruction;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.WallConstruction, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = userDataModel.CavityWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.CavityWallsInsulated, userDataModel, entryPoint)
            };

            return View("CavityWallsInsulated", viewModel);
        }

        [HttpPost("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Post(CavityWallsInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CavityWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.CavityWallsInsulated = viewModel.CavityWallsInsulated;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.CavityWallsInsulated, userDataModel, viewModel.EntryPoint));
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new SolidWallsInsulatedViewModel
            {
                SolidWallsInsulated = userDataModel.SolidWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.SolidWallsInsulated, userDataModel, entryPoint)
            };

            return View("SolidWallsInsulated", viewModel);
        }

        [HttpPost("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Post(SolidWallsInsulatedViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {
                return SolidWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.SolidWallsInsulated = viewModel.SolidWallsInsulated;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.SolidWallsInsulated, userDataModel, viewModel.EntryPoint));
        }


        [HttpGet("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = userDataModel.FloorConstruction,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.FloorConstruction, userDataModel, entryPoint)
            };

            return View("FloorConstruction", viewModel);
        }

        [HttpPost("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Post(FloorConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return FloorConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorConstruction = viewModel.FloorConstruction;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.FloorConstruction, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = userDataModel.FloorInsulated,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.FloorInsulated, userDataModel, entryPoint)
            };

            return View("FloorInsulated", viewModel);
        }

        [HttpPost("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Post(FloorInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return FloorInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorInsulated = viewModel.FloorInsulated;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.FloorInsulated, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofConstructionViewModel
            {
                PropertyType = userDataModel.PropertyType,
                FlatType = userDataModel.FlatType,
                RoofConstruction = userDataModel.RoofConstruction,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.RoofConstruction, userDataModel, entryPoint)
            };

            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RoofConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofConstruction = viewModel.RoofConstruction;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.RoofConstruction, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new AccessibleLoftSpaceViewModel
            {
                AccessibleLoftSpace = userDataModel.AccessibleLoftSpace,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.AccessibleLoftSpace, userDataModel, entryPoint)
            };

            return View("AccessibleLoftSpace", viewModel);
        }

        [HttpPost("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Post(AccessibleLoftSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return AccessibleLoftSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.AccessibleLoftSpace = viewModel.AccessibleLoftSpace;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.AccessibleLoftSpace, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = userDataModel.RoofInsulated,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                YearBuilt = userDataModel.YearBuilt,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.RoofInsulated, userDataModel, entryPoint)
            };

            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RoofInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofInsulated = viewModel.RoofInsulated;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.RoofInsulated, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("glazing-type/{reference}")]
        public IActionResult GlazingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = userDataModel.GlazingType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.GlazingType, userDataModel, entryPoint)
            };

            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type/{reference}")]
        public IActionResult GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GlazingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.GlazingType = viewModel.GlazingType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.GlazingType, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = userDataModel.HasOutdoorSpace,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.OutdoorSpace, userDataModel, entryPoint)
            };

            return View("OutdoorSpace", viewModel);
        }

        [HttpPost("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return OutdoorSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.OutdoorSpace, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("heating-type/{reference}")]
        public IActionResult HeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = userDataModel.HeatingType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.HeatingType, userDataModel, entryPoint)
            };

            return View("HeatingType", viewModel);
        }

        [HttpPost("heating-type/{reference}")]
        public IActionResult HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingType = viewModel.HeatingType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.HeatingType, userDataModel, viewModel.EntryPoint));
        }

        [HttpGet("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new OtherHeatingTypeViewModel
            {
                OtherHeatingType = userDataModel.OtherHeatingType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.OtherHeatingType, userDataModel, entryPoint)
            };

            return View("OtherHeatingType", viewModel);
        }

        [HttpPost("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Post(OtherHeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return OtherHeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.OtherHeatingType = viewModel.OtherHeatingType;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.OtherHeatingType, userDataModel, viewModel.EntryPoint));
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = userDataModel.HasHotWaterCylinder,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.HotWaterCylinder, userDataModel, entryPoint)
            };

            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HotWaterCylinder_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.HotWaterCylinder, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = userDataModel.NumberOfOccupants,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.NumberOfOccupants, userDataModel, entryPoint)
            };

            return View("NumberOfOccupants", viewModel);
        }

        [HttpPost("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Post(NumberOfOccupantsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return NumberOfOccupants_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.NumberOfOccupants = viewModel.NumberOfOccupants;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.NumberOfOccupants, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = userDataModel.HeatingPattern,
                HoursOfHeating = userDataModel.HoursOfHeating,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.HeatingPattern, userDataModel, entryPoint)
            };

            return View("HeatingPattern", viewModel);
        }

        [HttpPost("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Post(HeatingPatternViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HeatingPattern_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingPattern = viewModel.HeatingPattern;
            userDataModel.HoursOfHeating = viewModel.HeatingPattern == HeatingPattern.Other ? viewModel.HoursOfHeating : null;

            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.HeatingPattern, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("temperature/{reference}")]
        public IActionResult Temperature_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var viewModel = new TemperatureViewModel
            {
                Temperature = userDataModel.Temperature,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.Temperature, userDataModel, entryPoint)
            };

            return View("Temperature", viewModel);
        }

        [HttpPost("temperature/{reference}")]
        public IActionResult Temperature_Post(TemperatureViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Temperature_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Temperature = viewModel.Temperature;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.Temperature, userDataModel, viewModel.EntryPoint));
        }
        
        [HttpGet("email-address/{reference}")]
        public IActionResult EmailAddress_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new EmailAddressViewModel
            {
                HasEmailAddress = userDataModel.HasEmailAddress,
                EmailAddress = userDataModel.EmailAddress,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.EmailAddress, userDataModel, entryPoint)
            };

            return View("EmailAddress", viewModel);
        }

        [HttpPost("email-address/{reference}")]
        public IActionResult EmailAddress_Post(EmailAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return EmailAddress_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasEmailAddress = viewModel.HasEmailAddress;
            userDataModel.EmailAddress = viewModel.HasEmailAddress == HasEmailAddress.Yes ? viewModel.EmailAddress : null;
            userDataStore.SaveUserData(userDataModel);

            return Redirect(questionFlowService.ForwardLink(QuestionFlowPage.EmailAddress, userDataModel, viewModel.EntryPoint));
        }

        
        [HttpGet("answer-summary/{reference}")]
        public IActionResult AnswerSummary(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var viewModel = new AnswerSummaryViewModel
            {
                UserDataModel = userDataModel,
                BackLink = questionFlowService.BackLink(QuestionFlowPage.AnswerSummary, userDataModel)
            };
            
            return View("AnswerSummary", viewModel);
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
                    EmailAddress = userDataModel.EmailAddress,
                    BackLink = questionFlowService.BackLink(QuestionFlowPage.YourRecommendations, userDataModel)
                }
;            return View("YourRecommendations", viewModel);
        }

        [HttpPost("your-recommendations/{reference}")]
        public IActionResult YourRecommendations_Post(YourRecommendationsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return YourRecommendations_Get(viewModel.Reference);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.HasEmailAddress = viewModel.HasEmailAddress;
            userDataModel.EmailAddress = viewModel.HasEmailAddress == HasEmailAddress.Yes ? viewModel.EmailAddress : null;
            userDataStore.SaveUserData(userDataModel);
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