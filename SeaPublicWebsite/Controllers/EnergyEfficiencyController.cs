using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Notify.Exceptions;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.Data.DataStores;
using SeaPublicWebsite.Data.EnergyEfficiency;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;
using SeaPublicWebsite.Data.EnergyEfficiency.Recommendations;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Services;
using PropertyRecommendation = SeaPublicWebsite.Data.PropertyRecommendation;

namespace SeaPublicWebsite.Controllers
{
    [Route("energy-efficiency")]
    public class EnergyEfficiencyController : Controller
    {
        private readonly PropertyDataStore propertyDataStore;
        private readonly IQuestionFlowService questionFlowService;
        private readonly IEpcApi epcApi;
        private readonly IEmailSender emailApi;
        private readonly RecommendationService recommendationService;

        public EnergyEfficiencyController(
            PropertyDataStore propertyDataStore,
            IQuestionFlowService questionFlowService, 
            IEpcApi epcApi,
            IEmailSender emailApi, 
            RecommendationService recommendationService)
        {
            this.propertyDataStore = propertyDataStore;
            this.propertyDataStore = propertyDataStore;
            this.questionFlowService = questionFlowService;
            this.emailApi = emailApi;
            this.epcApi = epcApi;
            this.recommendationService = recommendationService;
        }
        
        
        [HttpGet("")]
        public IActionResult Index()
        {
           return View("Index");
        }

        
        [HttpGet("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Get()
        {
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NewOrReturningUser, new PropertyData());
            var viewModel = new NewOrReturningUserViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            return View("NewOrReturningUser", viewModel);
        }

        [HttpPost("new-or-returning-user")]
        public async Task<IActionResult> NewOrReturningUser_Post(NewOrReturningUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return NewOrReturningUser_Get();
            }

            if (viewModel.NewOrReturningUser == NewOrReturningUser.ReturningUser)
            {
                if (!propertyDataStore.IsReferenceValid(viewModel.Reference))
                {
                    ModelState.AddModelError(nameof(NewOrReturningUserViewModel.Reference), "Check you have typed the reference correctly. Reference must be 8 characters.");
                    return NewOrReturningUser_Get();
                }
                
                return RedirectToAction("YourSavedRecommendations_Get", "EnergyEfficiency", new { reference = viewModel.Reference });
            }

            string reference = propertyDataStore.GenerateNewReferenceAndSaveEmptyPropertyData();

            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs =
                questionFlowService.BackLinkArguments(QuestionFlowPage.OwnershipStatus, propertyData, entryPoint);
            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = propertyData.OwnershipStatus,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);
            
            propertyData.OwnershipStatus = viewModel.OwnershipStatus;
            propertyDataStore.SavePropertyData(propertyData);
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OwnershipStatus, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("country/{reference}")]
        public IActionResult Country_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            // var propertyData = propertyDataStore.LoadPropertyData(reference);
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Country, propertyData, entryPoint);
            var viewModel = new CountryViewModel
            {
                Country = propertyData.Country,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.Country = viewModel.Country;
            propertyDataStore.SavePropertyData(propertyData);
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Country, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("service-unsuitable/{reference}")]
        public IActionResult ServiceUnsuitable(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ServiceUnsuitable, propertyData);
            var viewModel = new ServiceUnsuitableViewModel
            {
                Reference = propertyData.Reference,
                Country = propertyData.Country,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("ServiceUnsuitable", viewModel);
        }

        [HttpGet("postcode/{reference}")]
        public IActionResult AskForPostcode_Get(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AskForPostcode, propertyData);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.AskForPostcode, propertyData);
            var viewModel = new AskForPostcodeViewModel
            {
                Postcode = propertyData.Postcode,
                HouseNameOrNumber = propertyData.HouseNameOrNumber,
                Reference = propertyData.Reference,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
            };

            return View("AskForPostcode", viewModel);
        }

        [HttpPost("postcode/{reference}")]
        public IActionResult AskForPostcode_Post(AskForPostcodeViewModel viewModel)
        {
            if (viewModel.Postcode is not null && !PostcodesIoApi.IsValidPostcode(viewModel.Postcode))
            {
                ModelState.AddModelError(nameof(AskForPostcodeViewModel.Postcode), "Enter a valid UK post code");
            }
            
            if (!ModelState.IsValid)
            {
                return AskForPostcode_Get(viewModel.Reference);
            }
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.Postcode = viewModel.Postcode;
            propertyData.HouseNameOrNumber = viewModel.HouseNameOrNumber;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AskForPostcode, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("address/{reference}")]
        public async Task<ViewResult> ConfirmAddress_Get(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            var epcList = await epcApi.GetEpcsForPostcode(propertyData.Postcode);
            var houseNameOrNumber = propertyData.HouseNameOrNumber;
            
            if (houseNameOrNumber != null)
            {
                var filteredEpcList = epcList.Where(e =>
                    e.Address1.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase) || e.Address2.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase)).ToList();

                epcList = filteredEpcList.Any() ? filteredEpcList : epcList;
            }

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ConfirmAddress, propertyData);
            var viewModel = new ConfirmAddressViewModel
            {
                Reference = reference,
                EPCList = epcList,
                SelectedEpcId = epcList.Count == 1 ? epcList[0].EpcId : null,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("ConfirmAddress", viewModel);
        }

        [HttpPost("address/{reference}")]
        public async Task<IActionResult> ConfirmAddress_Post(ConfirmAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await ConfirmAddress_Get(viewModel.Reference);
            }
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);
            
            var epc = (await epcApi.GetEpcsForPostcode(propertyData.Postcode)).FirstOrDefault(e => e.EpcId == viewModel.SelectedEpcId);
            propertyData.Epc = epc;

            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.ConfirmAddress, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("property-type/{reference}")]
        public IActionResult PropertyType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.PropertyType, propertyData, entryPoint);
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = propertyData.PropertyType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.PropertyType = viewModel.PropertyType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.PropertyType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("house-type/{reference}")]
        public IActionResult HouseType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HouseType, propertyData, entryPoint);
            var viewModel = new HouseTypeViewModel
            {
                HouseType = propertyData.HouseType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.HouseType = viewModel.HouseType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HouseType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public IActionResult BungalowType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.BungalowType, propertyData, entryPoint);
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = propertyData.BungalowType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);
            
            propertyData.BungalowType = viewModel.BungalowType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.BungalowType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("flat-type/{reference}")]
        public IActionResult FlatType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FlatType, propertyData, entryPoint);
            var viewModel = new FlatTypeViewModel
            {
                FlatType = propertyData.FlatType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.FlatType = viewModel.FlatType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FlatType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("home-age/{reference}")]
        public IActionResult HomeAge_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HomeAge, propertyData, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.HomeAge, propertyData, entryPoint);
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = propertyData.PropertyType,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);
            
            propertyData.YearBuilt = viewModel.YearBuilt;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HomeAge, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("wall-construction/{reference}")]
        public IActionResult WallConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.WallConstruction, propertyData, entryPoint);
            var viewModel = new WallConstructionViewModel
            {
                WallConstruction = propertyData.WallConstruction,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.WallConstruction = viewModel.WallConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.WallConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.CavityWallsInsulated, propertyData, entryPoint);
            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = propertyData.CavityWallsInsulated,
                WallConstruction = propertyData.WallConstruction,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.CavityWallsInsulated = viewModel.CavityWallsInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.CavityWallsInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.SolidWallsInsulated, propertyData, entryPoint);
            var viewModel = new SolidWallsInsulatedViewModel
            {
                SolidWallsInsulated = propertyData.SolidWallsInsulated,
                WallConstruction = propertyData.WallConstruction,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);
            
            propertyData.SolidWallsInsulated = viewModel.SolidWallsInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.SolidWallsInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorConstruction, propertyData, entryPoint);
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = propertyData.FloorConstruction,
                WallConstruction = propertyData.WallConstruction,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.FloorConstruction = viewModel.FloorConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorInsulated, propertyData, entryPoint);
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = propertyData.FloorInsulated,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.FloorInsulated = viewModel.FloorInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs =
                questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofConstruction, propertyData, entryPoint);
            var viewModel = new RoofConstructionViewModel
            {
                PropertyType = propertyData.PropertyType,
                FlatType = propertyData.FlatType,
                RoofConstruction = propertyData.RoofConstruction,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.RoofConstruction = viewModel.RoofConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AccessibleLoftSpace, propertyData, entryPoint);
            var viewModel = new AccessibleLoftSpaceViewModel
            {
                AccessibleLoftSpace = propertyData.AccessibleLoftSpace,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.AccessibleLoftSpace = viewModel.AccessibleLoftSpace;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AccessibleLoftSpace, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofInsulated, propertyData, entryPoint);
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = propertyData.RoofInsulated,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                YearBuilt = propertyData.YearBuilt,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.RoofInsulated = viewModel.RoofInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("glazing-type/{reference}")]
        public IActionResult GlazingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.GlazingType, propertyData, entryPoint);
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = propertyData.GlazingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.GlazingType = viewModel.GlazingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.GlazingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.OutdoorSpace, propertyData, entryPoint);
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = propertyData.HasOutdoorSpace,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OutdoorSpace, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-type/{reference}")]
        public IActionResult HeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HeatingType, propertyData, entryPoint);
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = propertyData.HeatingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.HeatingType = viewModel.HeatingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.OtherHeatingType, propertyData, entryPoint);
            var viewModel = new OtherHeatingTypeViewModel
            {
                OtherHeatingType = propertyData.OtherHeatingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.OtherHeatingType = viewModel.OtherHeatingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OtherHeatingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HotWaterCylinder, propertyData, entryPoint);
            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = propertyData.HasHotWaterCylinder,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HotWaterCylinder, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NumberOfOccupants, propertyData, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.NumberOfOccupants, propertyData, entryPoint);
            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = propertyData.NumberOfOccupants,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.NumberOfOccupants = viewModel.NumberOfOccupants;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.NumberOfOccupants, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HeatingPattern, propertyData, entryPoint);
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = propertyData.HeatingPattern,
                HoursOfHeatingMorning = propertyData.HoursOfHeatingMorning,
                HoursOfHeatingEvening = propertyData.HoursOfHeatingEvening,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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
            
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.HeatingPattern = viewModel.HeatingPattern;
            if (viewModel.HeatingPattern is HeatingPattern.Other)
            {
                propertyData.HoursOfHeatingMorning = viewModel.HoursOfHeatingMorning;
                propertyData.HoursOfHeatingEvening = viewModel.HoursOfHeatingEvening;
            }
            else
            {
                propertyData.HoursOfHeatingMorning = null;
                propertyData.HoursOfHeatingEvening = null;
            }

            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingPattern, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("temperature/{reference}")]
        public IActionResult Temperature_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Temperature, propertyData, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.Temperature, propertyData, entryPoint);
            var viewModel = new TemperatureViewModel
            {
                Temperature = propertyData.Temperature,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
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

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyData.Temperature = viewModel.Temperature;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            propertyDataStore.SavePropertyData(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Temperature, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("answer-summary/{reference}")]
        public IActionResult AnswerSummary(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AnswerSummary, propertyData);
            var viewModel = new AnswerSummaryViewModel
            {
                PropertyData = propertyData,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("AnswerSummary", viewModel);
        }

        
        [HttpGet("your-recommendations/{reference}")]
        public async Task<IActionResult> YourRecommendations_GetAsync(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            var recommendationsForProperty = await recommendationService.GetRecommendationsForPropertyAsync(propertyData);
            propertyData.PropertyRecommendations = recommendationsForProperty.Select(r => 
                new PropertyRecommendation()
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
            propertyDataStore.SavePropertyData(propertyData);

            int firstReferenceId = recommendationsForProperty.Count == 0 ? -1 : (int) recommendationsForProperty[0].Key;
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.YourRecommendations, propertyData);
            var viewModel = new YourRecommendationsViewModel
            {
                Reference = reference,
                NumberOfPropertyRecommendations = recommendationsForProperty.Count,
                FirstReferenceId = firstReferenceId,
                HasEmailAddress = false,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            return View("YourRecommendations", viewModel);
        }

        [HttpPost("your-recommendations/{reference}")]
        public async Task<IActionResult> YourRecommendations_Post(YourRecommendationsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await YourRecommendations_GetAsync(viewModel.Reference);
            }

            var propertyData = propertyDataStore.LoadPropertyData(viewModel.Reference);

            propertyDataStore.SavePropertyData(propertyData);
            
            if (viewModel.HasEmailAddress)
            {
                try
                {
                    emailApi.SendReferenceNumberEmail(viewModel.EmailAddress, propertyData.Reference);
                }
                catch (EmailSenderException e)
                {
                    switch (e.Type)
                    {
                        case EmailSenderExceptionType.InvalidEmailAddress:
                            ModelState.AddModelError(nameof(viewModel.EmailAddress), "Enter a valid email address");
                            break;
                        case EmailSenderExceptionType.Other:
                            ModelState.AddModelError(nameof(viewModel.EmailAddress), "Unable to send email due to unexpected error. Uncheck this box and make a note of your reference number before you continue.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return await YourRecommendations_GetAsync(viewModel.Reference);
                }
            }
            
            return RedirectToAction("Recommendation_Get", new { id = viewModel.FirstReferenceId, reference = viewModel.Reference });
        }

        [HttpGet("your-recommendations/{id}/{reference}")]
        public IActionResult Recommendation_Get(int id, string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            var viewModel = new RecommendationViewModel
            {
                PropertyData = propertyData,
                PropertyRecommendation = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey) id),
                RecommendationAction = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey)id).RecommendationAction
            };

            var recommendationKey = (RecommendationKey) id;

            return View("recommendations/" + Enum.GetName(recommendationKey), viewModel);
        }

        [HttpPost("your-recommendations/{id}/{reference}")]
        public IActionResult Recommendation_Post(RecommendationViewModel viewModel, string command, int id)
        {
            var propertyData = propertyDataStore.LoadPropertyData(viewModel.PropertyData.Reference);
            viewModel.PropertyData = propertyData;
            viewModel.PropertyRecommendation =
                propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey)id);
            
            if (!ModelState.IsValid)
            {
                return View("recommendations/" + Enum.GetName(viewModel.PropertyRecommendation.Key), viewModel);
            }

            propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey) id).RecommendationAction =
                viewModel.RecommendationAction;
            propertyDataStore.SavePropertyData(propertyData);

            switch(command)
            {
                case "goForwards":
                    return RedirectToAction("Recommendation_Get",
                        new {id = (int) viewModel.NextRecommendationKey(), reference = propertyData.Reference});
                case "goBackwards":
                    return RedirectToAction("Recommendation_Get",
                        new {id = (int) viewModel.PreviousRecommendationKey(), reference = propertyData.Reference});
                case "goToActionPlan":
                    return RedirectToAction("YourSavedRecommendations_Get", new {reference = propertyData.Reference});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("your-saved-recommendations/{reference}")]
        public IActionResult YourSavedRecommendations_Get(string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);
            
            var viewModel = new YourSavedRecommendationsViewModel
            {
                PropertyData = propertyData
            };
            return View("YourSavedRecommendations", viewModel);
        }

        [HttpGet("recommendation/add-to-plan/{id}/{reference}")]
        public IActionResult AddToPlan_Get(int id, string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var recommendationToUpdate = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey) id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.SaveToActionPlan;
            propertyDataStore.SavePropertyData(propertyData);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = propertyData.Reference });
        }

        [HttpGet("recommendation/remove-from-plan/{id}/{reference}")]
        public IActionResult RemoveFromPlan_Get(int id, string reference)
        {
            var propertyData = propertyDataStore.LoadPropertyData(reference);

            var recommendationToUpdate = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey)id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.Discard;
            propertyDataStore.SavePropertyData(propertyData);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = propertyData.Reference });
        }
    }
}