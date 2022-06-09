using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Notify.Exceptions;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ErrorHandling;
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
        private readonly IQuestionFlowService questionFlowService;
        private readonly IEpcApi epcApi;
        private readonly IEmailSender emailApi;
        private readonly RecommendationService recommendationService;

        public EnergyEfficiencyController(
            UserDataStore userDataStore,
            IQuestionFlowService questionFlowService, 
            IEpcApi epcApi, 
            IEmailSender emailApi, 
            RecommendationService recommendationService)
        {
            this.userDataStore = userDataStore;
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
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NewOrReturningUser, new UserDataModel());
            var viewModel = new NewOrReturningUserViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
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

            if (viewModel.NewOrReturningUser is NewOrReturningUser.ReturningUser)
            {
                if (!userDataStore.IsReferenceValid(viewModel.Reference))
                {
                    ModelState.AddModelError(nameof(NewOrReturningUserViewModel.Reference), "Check you have typed the reference correctly. Reference must be 8 characters.");
                    return NewOrReturningUser_Get();
                }

                return ReturningUser_Get(viewModel.Reference);
            }

            var reference = userDataStore.GenerateNewReferenceAndSaveEmptyUserData();
            
            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public IActionResult OwnershipStatus_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs =
                questionFlowService.BackLinkArguments(QuestionFlowPage.OwnershipStatus, userDataModel, entryPoint);
            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = userDataModel.OwnershipStatus,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.OwnershipStatus = viewModel.OwnershipStatus;
            userDataStore.SaveUserData(userDataModel);
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OwnershipStatus, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("country/{reference}")]
        public IActionResult Country_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Country, userDataModel, entryPoint);
            var viewModel = new CountryViewModel
            {
                Country = userDataModel.Country,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Country = viewModel.Country;
            userDataStore.SaveUserData(userDataModel);
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Country, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("service-unsuitable/{reference}")]
        public IActionResult ServiceUnsuitable(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ServiceUnsuitable, userDataModel);
            var viewModel = new ServiceUnsuitableViewModel
            {
                Reference = userDataModel.Reference,
                Country = userDataModel.Country,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("ServiceUnsuitable", viewModel);
        }

        [HttpGet("postcode/{reference}")]
        public IActionResult AskForPostcode_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AskForPostcode, userDataModel);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.AskForPostcode, userDataModel);
            var viewModel = new AskForPostcodeViewModel
            {
                Postcode = userDataModel.Postcode,
                HouseNameOrNumber = userDataModel.HouseNameOrNumber,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Postcode = viewModel.Postcode;
            userDataModel.HouseNameOrNumber = viewModel.HouseNameOrNumber;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AskForPostcode, userDataModel);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("address/{reference}")]
        public async Task<ViewResult> ConfirmAddress_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var epcList = await epcApi.GetEpcsForPostcode(userDataModel.Postcode);
            var houseNameOrNumber = userDataModel.HouseNameOrNumber;
            
            if (houseNameOrNumber != null)
            {
                var filteredEpcList = epcList.Where(e =>
                    e.Address1.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase) || e.Address2.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase)).ToList();

                epcList = filteredEpcList.Any() ? filteredEpcList : epcList;
            }

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ConfirmAddress, userDataModel);
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
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            var epc = (await epcApi.GetEpcsForPostcode(userDataModel.Postcode)).FirstOrDefault(e => e.EpcId == viewModel.SelectedEpcId);
            userDataModel.Epc = epc;

            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.ConfirmAddress, userDataModel);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("property-type/{reference}")]
        public IActionResult PropertyType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.PropertyType, userDataModel, entryPoint);
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = userDataModel.PropertyType,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.PropertyType = viewModel.PropertyType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.PropertyType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("house-type/{reference}")]
        public IActionResult HouseType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HouseType, userDataModel, entryPoint);
            var viewModel = new HouseTypeViewModel
            {
                HouseType = userDataModel.HouseType,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HouseType = viewModel.HouseType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HouseType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public IActionResult BungalowType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.BungalowType, userDataModel, entryPoint);
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = userDataModel.BungalowType,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.BungalowType = viewModel.BungalowType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.BungalowType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("flat-type/{reference}")]
        public IActionResult FlatType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FlatType, userDataModel, entryPoint);
            var viewModel = new FlatTypeViewModel
            {
                FlatType = userDataModel.FlatType,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FlatType = viewModel.FlatType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FlatType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("home-age/{reference}")]
        public IActionResult HomeAge_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HomeAge, userDataModel, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.HomeAge, userDataModel, entryPoint);
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = userDataModel.PropertyType,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.YearBuilt = viewModel.YearBuilt;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HomeAge, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("wall-construction/{reference}")]
        public IActionResult WallConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.WallConstruction, userDataModel, entryPoint);
            var viewModel = new WallConstructionViewModel
            {
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.WallConstruction = viewModel.WallConstruction;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.WallConstruction, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public IActionResult CavityWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.CavityWallsInsulated, userDataModel, entryPoint);
            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = userDataModel.CavityWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.CavityWallsInsulated = viewModel.CavityWallsInsulated;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.CavityWallsInsulated, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public IActionResult SolidWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.SolidWallsInsulated, userDataModel, entryPoint);
            var viewModel = new SolidWallsInsulatedViewModel
            {
                SolidWallsInsulated = userDataModel.SolidWallsInsulated,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            
            userDataModel.SolidWallsInsulated = viewModel.SolidWallsInsulated;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.SolidWallsInsulated, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("floor-construction/{reference}")]
        public IActionResult FloorConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorConstruction, userDataModel, entryPoint);
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = userDataModel.FloorConstruction,
                WallConstruction = userDataModel.WallConstruction,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorConstruction = viewModel.FloorConstruction;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorConstruction, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public IActionResult FloorInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorInsulated, userDataModel, entryPoint);
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = userDataModel.FloorInsulated,
                YearBuilt = userDataModel.YearBuilt,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.FloorInsulated = viewModel.FloorInsulated;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs =
                questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorInsulated, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-construction/{reference}")]
        public IActionResult RoofConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofConstruction, userDataModel, entryPoint);
            var viewModel = new RoofConstructionViewModel
            {
                PropertyType = userDataModel.PropertyType,
                FlatType = userDataModel.FlatType,
                RoofConstruction = userDataModel.RoofConstruction,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofConstruction = viewModel.RoofConstruction;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofConstruction, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("accessible-loft-space/{reference}")]
        public IActionResult AccessibleLoftSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AccessibleLoftSpace, userDataModel, entryPoint);
            var viewModel = new AccessibleLoftSpaceViewModel
            {
                AccessibleLoftSpace = userDataModel.AccessibleLoftSpace,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.AccessibleLoftSpace = viewModel.AccessibleLoftSpace;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AccessibleLoftSpace, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-insulated/{reference}")]
        public IActionResult RoofInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofInsulated, userDataModel, entryPoint);
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = userDataModel.RoofInsulated,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                YearBuilt = userDataModel.YearBuilt,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.RoofInsulated = viewModel.RoofInsulated;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofInsulated, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("glazing-type/{reference}")]
        public IActionResult GlazingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.GlazingType, userDataModel, entryPoint);
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = userDataModel.GlazingType,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.GlazingType = viewModel.GlazingType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.GlazingType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("outdoor-space/{reference}")]
        public IActionResult OutdoorSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.OutdoorSpace, userDataModel, entryPoint);
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = userDataModel.HasOutdoorSpace,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OutdoorSpace, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-type/{reference}")]
        public IActionResult HeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HeatingType, userDataModel, entryPoint);
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = userDataModel.HeatingType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingType = viewModel.HeatingType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("other-heating-type/{reference}")]
        public IActionResult OtherHeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.OtherHeatingType, userDataModel, entryPoint);
            var viewModel = new OtherHeatingTypeViewModel
            {
                OtherHeatingType = userDataModel.OtherHeatingType,
                Reference = userDataModel.Reference,
                EntryPoint = entryPoint,
                Epc = userDataModel.Epc,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.OtherHeatingType = viewModel.OtherHeatingType;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OtherHeatingType, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public IActionResult HotWaterCylinder_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HotWaterCylinder, userDataModel, entryPoint);
            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = userDataModel.HasHotWaterCylinder,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HotWaterCylinder, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public IActionResult NumberOfOccupants_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NumberOfOccupants, userDataModel, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.NumberOfOccupants, userDataModel, entryPoint);
            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = userDataModel.NumberOfOccupants,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.NumberOfOccupants = viewModel.NumberOfOccupants;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.NumberOfOccupants, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public IActionResult HeatingPattern_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HeatingPattern, userDataModel, entryPoint);
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = userDataModel.HeatingPattern,
                HoursOfHeatingMorning = userDataModel.HoursOfHeatingMorning,
                HoursOfHeatingEvening = userDataModel.HoursOfHeatingEvening,
                Reference = userDataModel.Reference,
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
            
            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.HeatingPattern = viewModel.HeatingPattern;
            if (viewModel.HeatingPattern is HeatingPattern.Other)
            {
                userDataModel.HoursOfHeatingMorning = viewModel.HoursOfHeatingMorning;
                userDataModel.HoursOfHeatingEvening = viewModel.HoursOfHeatingEvening;
            }
            else
            {
                userDataModel.HoursOfHeatingMorning = null;
                userDataModel.HoursOfHeatingEvening = null;
            }

            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingPattern, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("temperature/{reference}")]
        public IActionResult Temperature_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Temperature, userDataModel, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.Temperature, userDataModel, entryPoint);
            var viewModel = new TemperatureViewModel
            {
                Temperature = userDataModel.Temperature,
                Reference = userDataModel.Reference,
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

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);

            userDataModel.Temperature = viewModel.Temperature;
            UserDataHelper.ResetUnusedFields(userDataModel);
            userDataStore.SaveUserData(userDataModel);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Temperature, userDataModel, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("answer-summary/{reference}")]
        public IActionResult AnswerSummary_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AnswerSummary, userDataModel);
            var viewModel = new AnswerSummaryViewModel
            {
                UserDataModel = userDataModel,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("AnswerSummary", viewModel);
        }

        [HttpPost("answer-summary/{reference}")]
        public async Task<IActionResult> AnswerSummary_PostAsync(string reference)
        {
            if (!ModelState.IsValid)
            {
                return AnswerSummary_Get(reference);
            }
            
            var userDataModel = userDataStore.LoadUserData(reference);
            var recommendationsForUser = await recommendationService.GetRecommendationsForUserAsync(userDataModel);
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
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AnswerSummary, userDataModel);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("returning-user/{reference}")]
        public IActionResult ReturningUser_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var recommendations = userDataModel.UserRecommendations;
            if (!recommendations.Any())
            {
                return RedirectToAction(nameof(NoRecommendations_Get), "EnergyEfficiency", new { reference });
            }

            var firstNotActionedRecommendation = recommendations.Find(recommendation => recommendation.RecommendationAction is null);
            if (firstNotActionedRecommendation is not null)
            {
                return RedirectToAction(nameof(Recommendation_Get), "EnergyEfficiency",
                    new { id = (int)firstNotActionedRecommendation.Key, reference = userDataModel.Reference });
            }
            
            return RedirectToAction("YourSavedRecommendations_Get", new {reference = userDataModel.Reference});
        }

        [HttpGet("no-recommendations/{reference}")]
        public IActionResult NoRecommendations_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NoRecommendations, userDataModel);
            var viewModel = new NoRecommendationsViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            return View("NoRecommendations", viewModel);
        }

        [HttpGet("your-recommendations/{reference}")]
        public IActionResult YourRecommendations_Get(string reference)
        {
            var userDataModel = userDataStore.LoadUserData(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.YourRecommendations, userDataModel);
            var viewModel = new YourRecommendationsViewModel
            {
                Reference = reference,
                NumberOfUserRecommendations = userDataModel.UserRecommendations.Count,
                HasEmailAddress = false,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            return View("YourRecommendations", viewModel);
        }

        [HttpPost("your-recommendations/{reference}")]
        public IActionResult YourRecommendations_Post(YourRecommendationsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return YourRecommendations_Get(viewModel.Reference);
            }

            var userDataModel = userDataStore.LoadUserData(viewModel.Reference);
            if (viewModel.HasEmailAddress)
            {
                try
                {
                    emailApi.SendReferenceNumberEmail(viewModel.EmailAddress, userDataModel.Reference);
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
                    return YourRecommendations_Get(viewModel.Reference);
                }
            }
            
            return RedirectToAction("Recommendation_Get", new { id = (int) userDataModel.UserRecommendations[0].Key, reference = viewModel.Reference });
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