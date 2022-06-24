using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Services;

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

            if (viewModel.NewOrReturningUser is NewOrReturningUser.ReturningUser)
            {
                if (!await propertyDataStore.IsReferenceValidAsync(viewModel.Reference))
                {
                    ModelState.AddModelError(nameof(NewOrReturningUserViewModel.Reference), "Check you have typed the reference correctly. Reference must be 8 characters.");
                    return NewOrReturningUser_Get();
                }

                return await ReturningUser_Get(viewModel.Reference);
            }

            string reference = await propertyDataStore.CreateNewPropertyDataAsync();

            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public async Task<IActionResult> OwnershipStatus_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OwnershipStatus_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            propertyData.OwnershipStatus = viewModel.OwnershipStatus;
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OwnershipStatus, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("country/{reference}")]
        public async Task<IActionResult> Country_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> Country_Post(CountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Country_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.Country = viewModel.Country;
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Country, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("service-unsuitable/{reference}")]
        public async Task<IActionResult> ServiceUnsuitable(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ServiceUnsuitable, propertyData);
            var viewModel = new ServiceUnsuitableViewModel
            {
                Reference = propertyData.Reference,
                Country = propertyData.Country,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("ServiceUnsuitable", viewModel);
        }

        [HttpGet("find-epc/{reference}")]
        public async Task<IActionResult> FindEpc_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FindEpc, propertyData);

            var viewModel = new FindEpcViewModel
            {
                Reference = propertyData.Reference,
                FindEpc = propertyData.FindEpc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
            };
            return View("FindEpc", viewModel);
        }

        [HttpPost("find-epc/{reference}")]
        public async Task<IActionResult> FindEpc_Post(FindEpcViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FindEpc_Get(viewModel.Reference);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.Epc = null;
            propertyData.PropertyType = null;
            propertyData.YearBuilt = null;
            propertyData.FindEpc = viewModel.FindEpc;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs =
                questionFlowService.ForwardLinkArguments(QuestionFlowPage.FindEpc, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("postcode/{reference}")]
        public async Task<IActionResult> AskForPostcode_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> AskForPostcode_Post(AskForPostcodeViewModel viewModel)
        {
            if (viewModel.Postcode is not null && !PostcodesIoApi.IsValidPostcode(viewModel.Postcode))
            {
                ModelState.AddModelError(nameof(AskForPostcodeViewModel.Postcode), "Please enter a valid postcode e.g. AB12 3CD");
            }
            
            if (!ModelState.IsValid)
            {
                return await AskForPostcode_Get(viewModel.Reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.Postcode = viewModel.Postcode;
            propertyData.HouseNameOrNumber = viewModel.HouseNameOrNumber;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AskForPostcode, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("address/{reference}")]
        public async Task<ViewResult> ConfirmAddress_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            List<EpcInformation> epcInformationList = await epcApi.GetEpcsInformationForPostcodeAndBuildingNameOrNumber(propertyData.Postcode, propertyData.HouseNameOrNumber);
            var houseNameOrNumber = propertyData.HouseNameOrNumber;
            var filteredEpcInformationList = epcInformationList.Where(e =>
                e.Address1.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase) ||
                e.Address2.Contains(houseNameOrNumber, StringComparison.OrdinalIgnoreCase)).ToList();

            epcInformationList = filteredEpcInformationList.Any() ? filteredEpcInformationList : epcInformationList;
            
            //return only EPCs that contain all the data we want to assume
            var filteredEpcList = epcInformationList.Select(async e  =>
                await epcApi.GetEpcForId(e.EpcId));
            epcInformationList = filteredEpcList
                .Select(e => e.Result)
                .Where(e => e?.ConstructionAgeBand != null
                            && ((e.PropertyType == PropertyType.House && e.HouseType != null)
                            || (e.PropertyType == PropertyType.Bungalow && e.BungalowType != null)
                            || (e.PropertyType == PropertyType.ApartmentFlatOrMaisonette && e.FlatType != null)))
                .Select(e => new EpcInformation(
                    e.EpcId,
                    e.Address1,
                    e.Address2,
                    e.Postcode
                )).ToList();
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ConfirmAddress, propertyData);
            var viewModel = new ConfirmAddressViewModel
            {
                Reference = reference,
                EpcInformationList = epcInformationList,
                SelectedEpcId = epcInformationList.Count == 1 ? epcInformationList[0].EpcId : null,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("ConfirmAddress", viewModel);
        
        }

        [HttpPost("address/{reference}")]
        public async Task<IActionResult> ConfirmAddress_Post(ConfirmAddressViewModel viewModel, int epcCount)
        {
            if (epcCount != 1)
            {
                ModelState.Remove("SingleAddressConfirmed");
            }
            
            if (!ModelState.IsValid)
            {
                return await ConfirmAddress_Get(viewModel.Reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            if (viewModel.SelectedEpcId != "unlisted" && viewModel.SingleAddressConfirmed != SingleAddressConfirmed.No)
            {
                propertyData.Epc = await epcApi.GetEpcForId(viewModel.SelectedEpcId);
            }
            else
            {
                propertyData.Epc = null;
            }
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.ConfirmAddress, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("confirm-epc-details/{reference}")]
        public async Task<IActionResult> ConfirmEpcDetails_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ConfirmEpcDetails, propertyData);

            var viewModel = new ConfirmEpcDetailsViewModel
            {
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
            };
            return View("ConfirmEpcDetails", viewModel);
        }

        [HttpPost("confirm-epc-details/{reference}")]
        public async Task<IActionResult> ConfirmEpcDetails_Post(ConfirmEpcDetailsViewModel viewModel, PropertyType propertyType, HomeAge constructionAgeBand, HouseType houseType, BungalowType bungalowType, FlatType flatType)
        {
            if (!ModelState.IsValid)
            {
                return await ConfirmEpcDetails_Get(viewModel.Reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            if (viewModel.EpcDetailsConfirmed == EpcDetailsConfirmed.Yes)
            {
                propertyData.PropertyType = propertyType;
                propertyData.HouseType = houseType;
                propertyData.BungalowType = bungalowType;
                propertyData.FlatType = flatType;
                propertyData.YearBuilt = constructionAgeBand switch
                {
                    HomeAge.Pre1900 => YearBuilt.Pre1930,
                    HomeAge.From1900To1929 => YearBuilt.Pre1930,
                    HomeAge.From1930To1949 => YearBuilt.From1930To1966,
                    HomeAge.From1950To1966 => YearBuilt.From1930To1966,
                    HomeAge.From1967To1975 => YearBuilt.From1967To1982,
                    HomeAge.From1976To1982 => YearBuilt.From1967To1982,
                    HomeAge.From1983To1990 => YearBuilt.From1983To1995,
                    HomeAge.From1991To1995 => YearBuilt.From1983To1995,
                    HomeAge.From1996To2002 => YearBuilt.From1996To2011,
                    HomeAge.From2003To2006 => YearBuilt.From1996To2011,
                    HomeAge.From2007To2011 => YearBuilt.From1996To2011,
                    HomeAge.From2012ToPresent => YearBuilt.From2012ToPresent,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                propertyData.Epc = null;
                propertyData.PropertyType = null;
                propertyData.YearBuilt = null;
            }
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.ConfirmEpcDetails, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("property-type/{reference}")]
        public async Task<IActionResult> PropertyType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> PropertyType_Post(PropertyTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await PropertyType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.PropertyType = viewModel.PropertyType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.PropertyType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("house-type/{reference}")]
        public async Task<IActionResult> HouseType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> HouseType_Post(HouseTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HouseType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.HouseType = viewModel.HouseType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HouseType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public async Task<IActionResult> BungalowType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await BungalowType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            propertyData.BungalowType = viewModel.BungalowType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.BungalowType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("flat-type/{reference}")]
        public async Task<IActionResult> FlatType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> FlatType_Post(FlatTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FlatType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.FlatType = viewModel.FlatType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FlatType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("home-age/{reference}")]
        public async Task<IActionResult> HomeAge_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HomeAge, propertyData, entryPoint);
            var skipArgs = questionFlowService.SkipLinkArguments(QuestionFlowPage.HomeAge, propertyData, entryPoint);
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = propertyData.PropertyType,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
            };

            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age/{reference}")]
        public async Task<IActionResult> HomeAge_Post(HomeAgeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HomeAge_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            propertyData.YearBuilt = viewModel.YearBuilt;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HomeAge, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("wall-construction/{reference}")]
        public async Task<IActionResult> WallConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> WallConstruction_Post(WallConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await WallConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.WallConstruction = viewModel.WallConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.WallConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public async Task<IActionResult> CavityWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> CavityWallsInsulated_Post(CavityWallsInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await CavityWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.CavityWallsInsulated = viewModel.CavityWallsInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.CavityWallsInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public async Task<IActionResult> SolidWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> SolidWallsInsulated_Post(SolidWallsInsulatedViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {
                return await SolidWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            propertyData.SolidWallsInsulated = viewModel.SolidWallsInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.SolidWallsInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("floor-construction/{reference}")]
        public async Task<IActionResult> FloorConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> FloorConstruction_Post(FloorConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FloorConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.FloorConstruction = viewModel.FloorConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public async Task<IActionResult> FloorInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> FloorInsulated_Post(FloorInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FloorInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.FloorInsulated = viewModel.FloorInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs =
                questionFlowService.ForwardLinkArguments(QuestionFlowPage.FloorInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-construction/{reference}")]
        public async Task<IActionResult> RoofConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofConstruction, propertyData, entryPoint);
            var viewModel = new RoofConstructionViewModel
            {
                PropertyType = propertyData.PropertyType,
                FlatType = propertyData.FlatType,
                RoofConstruction = propertyData.RoofConstruction,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction/{reference}")]
        public async Task<IActionResult> RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await RoofConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.RoofConstruction = viewModel.RoofConstruction;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofConstruction, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("loft-space/{reference}")]
        public async Task<IActionResult> LoftSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.LoftSpace, propertyData, entryPoint);
            var viewModel = new LoftSpaceViewModel
            {
                LoftSpace = propertyData.LoftSpace,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("LoftSpace", viewModel);
        }

        [HttpPost("loft-space/{reference}")]
        public async Task<IActionResult> LoftSpace_Post(LoftSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoftSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.LoftSpace = viewModel.LoftSpace;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.LoftSpace, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("loft-access/{reference}")]
        public async Task<IActionResult> LoftAccess_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.LoftAccess, propertyData, entryPoint);
            var viewModel = new LoftAccessViewModel
            {
                LoftAccess = propertyData.LoftAccess,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("LoftAccess", viewModel);
        }
        
        [HttpPost("loft-access/{reference}")]
        public async Task<IActionResult> LoftAccess_Post(LoftAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoftAccess_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.LoftAccess = viewModel.LoftAccess;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.LoftAccess, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("roof-insulated/{reference}")]
        public async Task<IActionResult> RoofInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofInsulated, propertyData, entryPoint);
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = propertyData.RoofInsulated,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated/{reference}")]
        public async Task<IActionResult> RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await RoofInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.RoofInsulated = viewModel.RoofInsulated;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.RoofInsulated, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("glazing-type/{reference}")]
        public async Task<IActionResult> GlazingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.GlazingType, propertyData, entryPoint);
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = propertyData.GlazingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type/{reference}")]
        public async Task<IActionResult> GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await GlazingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.GlazingType = viewModel.GlazingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.GlazingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("outdoor-space/{reference}")]
        public async Task<IActionResult> OutdoorSpace_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OutdoorSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.HasOutdoorSpace = viewModel.HasOutdoorSpace;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OutdoorSpace, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-type/{reference}")]
        public async Task<IActionResult> HeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.HeatingType = viewModel.HeatingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("other-heating-type/{reference}")]
        public async Task<IActionResult> OtherHeatingType_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> OtherHeatingType_Post(OtherHeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OtherHeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.OtherHeatingType = viewModel.OtherHeatingType;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.OtherHeatingType, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public async Task<IActionResult> HotWaterCylinder_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HotWaterCylinder, propertyData, entryPoint);
            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = propertyData.HasHotWaterCylinder,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder/{reference}")]
        public async Task<IActionResult> HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HotWaterCylinder_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.HasHotWaterCylinder = viewModel.HasHotWaterCylinder;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HotWaterCylinder, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("number-of-occupants/{reference}")]
        public async Task<IActionResult> NumberOfOccupants_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

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
        public async Task<IActionResult> NumberOfOccupants_Post(NumberOfOccupantsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await NumberOfOccupants_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.NumberOfOccupants = viewModel.NumberOfOccupants;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.NumberOfOccupants, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("heating-pattern/{reference}")]
        public async Task<IActionResult> HeatingPattern_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> HeatingPattern_Post(HeatingPatternViewModel viewModel)
        {
            if (viewModel.HeatingPattern != HeatingPattern.Other)
            {
                ModelState.Remove("HoursOfHeatingMorning");
                ModelState.Remove("HoursOfHeatingEvening");
            }
            
            if (!ModelState.IsValid)
            {
                return await HeatingPattern_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

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
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.HeatingPattern, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("temperature/{reference}")]
        public async Task<IActionResult> Temperature_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
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
        public async Task<IActionResult> Temperature_Post(TemperatureViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Temperature_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            propertyData.Temperature = viewModel.Temperature;
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.Temperature, propertyData, viewModel.EntryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("answer-summary/{reference}")]
        public async Task<IActionResult> AnswerSummary_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AnswerSummary, propertyData);
            var viewModel = new AnswerSummaryViewModel
            {
                PropertyData = propertyData,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            return View("AnswerSummary", viewModel);
        }

        [HttpPost("answer-summary/{reference}")]
        public async Task<IActionResult> AnswerSummary_PostAsync(string reference)
        {
            if (!ModelState.IsValid)
            {
                return await AnswerSummary_Get(reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var recommendationsForPropertyAsync = await recommendationService.GetRecommendationsForPropertyAsync(propertyData);
            propertyData.PropertyRecommendations = recommendationsForPropertyAsync.Select(r => 
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
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AnswerSummary, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("returning-user/{reference}")]
        public async Task<IActionResult> ReturningUser_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var recommendations = propertyData.PropertyRecommendations;
            if (!recommendations.Any())
            {
                return RedirectToAction(nameof(NoRecommendations_Get), "EnergyEfficiency", new { reference });
            }

            var firstNotActionedRecommendation = recommendations.Find(recommendation => recommendation.RecommendationAction is null);
            if (firstNotActionedRecommendation is not null)
            {
                return RedirectToAction(nameof(Recommendation_Get), "EnergyEfficiency",
                    new { id = (int)firstNotActionedRecommendation.Key, reference = propertyData.Reference });
            }
            
            return RedirectToAction("YourSavedRecommendations_Get", new {reference = propertyData.Reference});
        }

        [HttpGet("no-recommendations/{reference}")]
        public async Task<IActionResult> NoRecommendations_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NoRecommendations, propertyData);
            var viewModel = new NoRecommendationsViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            return View("NoRecommendations", viewModel);
        }

        [HttpGet("your-recommendations/{reference}")]
        public async Task<IActionResult> YourRecommendations_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.YourRecommendations, propertyData);
            var viewModel = new YourRecommendationsViewModel
            {
                Reference = reference,
                NumberOfPropertyRecommendations = propertyData.PropertyRecommendations.Count,
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
                return await YourRecommendations_Get(viewModel.Reference);
            }

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
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
                            ModelState.AddModelError(nameof(viewModel.EmailAddress), "Unable to send email due to unexpected error. Uncheck this box and make a note of your reference code before you continue.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return await YourRecommendations_Get(viewModel.Reference);
                }
            }
            
            return RedirectToAction("Recommendation_Get", new { id = (int) propertyData.PropertyRecommendations[0].Key, reference = viewModel.Reference });
        }

        [HttpGet("your-recommendations/{id}/{reference}")]
        public async Task<IActionResult> Recommendation_Get(int id, string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
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
        public async Task<IActionResult> Recommendation_Post(RecommendationViewModel viewModel, string command, int id)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.PropertyData.Reference);
            viewModel.PropertyData = propertyData;
            viewModel.PropertyRecommendation =
                propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey)id);
            
            if (!ModelState.IsValid)
            {
                return View("recommendations/" + Enum.GetName(viewModel.PropertyRecommendation.Key), viewModel);
            }

            propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey) id).RecommendationAction =
                viewModel.RecommendationAction;
            await propertyDataStore.SavePropertyDataAsync(propertyData);

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
        public async Task<IActionResult> YourSavedRecommendations_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var viewModel = new YourSavedRecommendationsViewModel
            {
                PropertyData = propertyData
            };
            return View("YourSavedRecommendations", viewModel);
        }

        [HttpGet("recommendation/add-to-plan/{id}/{reference}")]
        public async Task<IActionResult> AddToPlan_Get(int id, string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var recommendationToUpdate = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey) id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.SaveToActionPlan;
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = propertyData.Reference });
        }

        [HttpGet("recommendation/remove-from-plan/{id}/{reference}")]
        public async Task<IActionResult> RemoveFromPlan_Get(int id, string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var recommendationToUpdate = propertyData.PropertyRecommendations.First(r => r.Key == (RecommendationKey)id);
            recommendationToUpdate.RecommendationAction = RecommendationAction.Discard;
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
            return RedirectToAction("YourSavedRecommendations_Get", new { reference = propertyData.Reference });
        }
    }
}
