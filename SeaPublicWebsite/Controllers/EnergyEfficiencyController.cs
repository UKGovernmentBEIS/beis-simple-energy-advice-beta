using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.BusinessLogic.Services;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.EmailSending;
using SeaPublicWebsite.ExternalServices.GoogleAnalytics;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Models.Cookies;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Services;
using SeaPublicWebsite.Services.Cookies;

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
        private readonly CookieService cookieService;
        private readonly GoogleAnalyticsService googleAnalyticsService;
        private readonly PostcodesIoApi postcodesIoApi;

        public EnergyEfficiencyController(
            PropertyDataStore propertyDataStore,
            IQuestionFlowService questionFlowService, 
            IEpcApi epcApi,
            IEmailSender emailApi,
            RecommendationService recommendationService,
            CookieService cookieService,
            GoogleAnalyticsService googleAnalyticsService,
            PostcodesIoApi postcodesIoApi)
        {
            this.propertyDataStore = propertyDataStore;
            this.questionFlowService = questionFlowService;
            this.emailApi = emailApi;
            this.epcApi = epcApi;
            this.recommendationService = recommendationService;
            this.cookieService = cookieService;
            this.googleAnalyticsService = googleAnalyticsService;
            this.postcodesIoApi = postcodesIoApi;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            // TODO: seabeta-576 When private beta finishes, this section should be removed.
            if (!cookieService.HasAcceptedGoogleAnalytics(Request))
            {
                return RedirectToAction(nameof(PrivateBeta_Get), "EnergyEfficiency");
            }
            
            return View("Index");
        }
        
        [HttpGet("new-or-returning-user")]
        public IActionResult NewOrReturningUser_Get()
        {
            var backArgs = GetBackArgs(QuestionFlowStep.NewOrReturningUser, null);
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

                return await ReturningUser_Get(viewModel.Reference, fromMagicLink: false);
            }

            var propertyData = await propertyDataStore.CreateNewPropertyDataAsync();

            var nextStep = questionFlowService.NextStep(QuestionFlowStep.NewOrReturningUser, propertyData);
            var forwardArgs = GetActionArgumentsForQuestion(nextStep, propertyData.Reference);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        
        [HttpGet("ownership-status/{reference}")]
        public async Task<IActionResult> OwnershipStatus_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.OwnershipStatus, propertyData);
            var viewModel = new OwnershipStatusViewModel
            {
                OwnershipStatus = propertyData.OwnershipStatus,
                Reference = propertyData.Reference,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("OwnershipStatus", viewModel);
        }

        [HttpPost("ownership-status/{reference}")]
        public async Task<IActionResult> OwnershipStatus_Post(OwnershipStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OwnershipStatus_Get(viewModel.Reference);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.OwnershipStatus = viewModel.OwnershipStatus,
                viewModel.Reference,
                QuestionFlowStep.OwnershipStatus);
        }

        [HttpGet("country/{reference}")]
        public async Task<IActionResult> Country_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.Country, propertyData);
            var viewModel = new CountryViewModel
            {
                Country = propertyData.Country,
                Reference = propertyData.Reference,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("Country", viewModel);
        }

        [HttpPost("country/{reference}")]
        public async Task<IActionResult> Country_Post(CountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Country_Get(viewModel.Reference);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.Country = viewModel.Country,
                viewModel.Reference,
                QuestionFlowStep.Country);
        }


        [HttpGet("service-unsuitable/{reference}")]
        public async Task<IActionResult> ServiceUnsuitable(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.ServiceUnsuitable, propertyData);
            var viewModel = new ServiceUnsuitableViewModel
            {
                Reference = propertyData.Reference,
                Country = propertyData.Country,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("ServiceUnsuitable", viewModel);
        }

        [HttpGet("find-epc/{reference}")]
        public async Task<IActionResult> FindEpc_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.FindEpc, propertyData);

            var viewModel = new FindEpcViewModel
            {
                Reference = propertyData.Reference,
                FindEpc = propertyData.SearchForEpc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("FindEpc", viewModel);
        }

        [HttpPost("find-epc/{reference}")]
        public async Task<IActionResult> FindEpc_Post(FindEpcViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FindEpc_Get(viewModel.Reference);
            }

            return await UpdatePropertyAndRedirect(
                propertyData =>
                {
                    propertyData.SearchForEpc = viewModel.FindEpc;
                    propertyData.EpcDetailsConfirmed = null;
                    propertyData.Epc = null;
                    propertyData.PropertyType = null;
                    propertyData.YearBuilt = null;
                },
                viewModel.Reference,
                QuestionFlowStep.FindEpc,
                viewModel.EntryPoint);
        }

        [HttpGet("postcode/{reference}")]
        public async Task<IActionResult> AskForPostcode_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.AskForPostcode, propertyData);
            var skipDestination = questionFlowService.SkipDestination(QuestionFlowStep.AskForPostcode, propertyData);
            var skipArgs = GetActionArgumentsForQuestion(skipDestination, reference);
            var viewModel = new AskForPostcodeViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
                SkipLink = Url.Action(skipArgs.Action, skipArgs.Controller, skipArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("AskForPostcode", viewModel);
        }

        [HttpPost("postcode/{reference}")]
        public async Task<IActionResult> AskForPostcode_Post(AskForPostcodeViewModel viewModel, bool cancel = false)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            if (cancel)
            {
                return await UpdatePropertyAndRedirect(
                    // Behave as if the user had selected "No" on the page before
                    p => { p.SearchForEpc = SearchForEpc.No; },
                    viewModel.Reference,
                    QuestionFlowStep.AskForPostcode);
            }
            
            if (viewModel.Postcode is not null && !(await postcodesIoApi.IsValidPostcode(viewModel.Postcode)))
            {
                ModelState.AddModelError(nameof(AskForPostcodeViewModel.Postcode), "Enter a valid UK postcode");
            }
            
            if (!ModelState.IsValid)
            {
                return await AskForPostcode_Get(viewModel.Reference);
            }

            var nextStep = questionFlowService.NextStep(QuestionFlowStep.AskForPostcode, propertyData);
            var forwardArgs = GetActionArgumentsForQuestion(nextStep, viewModel.Reference);

            return RedirectToAction(
                forwardArgs.Action,
                forwardArgs.Controller,
                // It would be nice to find a way to get GetActionArgumentsForQuestion() to include the postcode and
                // house number.
                new { reference = viewModel.Reference, postcode = viewModel.Postcode, number = viewModel.HouseNameOrNumber });
        }

        
        [HttpGet("address/{reference}/{postcode}/{number}")]
        public async Task<ViewResult> ConfirmAddress_Get(string reference, string postcode, string number)
        {
            List<EpcSearchResult> epcSearchResults = await epcApi.GetEpcsInformationForPostcodeAndBuildingNameOrNumber(postcode, number);
            
            var filteredEpcSearchResults = epcSearchResults.Where(e =>
                e.Address1.Contains(number, StringComparison.OrdinalIgnoreCase) ||
                e.Address2.Contains(number, StringComparison.OrdinalIgnoreCase)).ToList();

            epcSearchResults = filteredEpcSearchResults.Any() ? filteredEpcSearchResults : epcSearchResults;

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.ConfirmAddress, propertyData);
            var backLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values);

            if (epcSearchResults.Count == 0)
            {
                var viewModel = new NoEpcFoundViewModel
                {
                    Reference = reference,
                    BackLink = backLink
                };
                return View("NoEpcFound", viewModel);
            }
            else if (epcSearchResults.Count == 1)
            {
                var viewModel = new ConfirmSingleAddressViewModel
                {
                    Reference = reference,
                    BackLink = backLink,
                    EpcSearchResult = epcSearchResults[0],
                    Number = number,
                    Postcode = postcode,
                    EpcId = epcSearchResults[0].EpcId
                };
                return View("ConfirmSingleAddress", viewModel);
            }
            else
            {
                var viewModel = new ConfirmAddressViewModel
                {
                    Reference = reference,
                    EpcSearchResults = epcSearchResults,
                    Postcode = postcode,
                    Number = number,
                    BackLink = backLink
                };

                ViewBag.FeedbackUrl = propertyData.ReturningUser
                    ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                    : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
                return View("ConfirmAddress", viewModel);
            }
        }

        [HttpPost("address/{reference}/{postcode}/{number}")]
        public async Task<IActionResult> ConfirmAddress_Post(ConfirmAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await ConfirmAddress_Get(viewModel.Reference, viewModel.Postcode, viewModel.Number);
            }
            
            var epc = viewModel.SelectedEpcId == "unlisted" ? null : await epcApi.GetEpcForId(viewModel.SelectedEpcId);
            
            return await UpdatePropertyAndRedirect(
                p => { p.Epc = epc;},
                viewModel.Reference,
                QuestionFlowStep.ConfirmAddress);
        }
        
        [HttpPost("single-address/{reference}/{postcode}/{number}")]
        public async Task<IActionResult> ConfirmSingleAddress_Post(ConfirmSingleAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await ConfirmAddress_Get(viewModel.Reference, viewModel.Postcode, viewModel.Number);
            }
            
            var epc = viewModel.EpcAddressConfirmed == EpcAddressConfirmed.Yes ? await epcApi.GetEpcForId(viewModel.EpcId) : null;

            return await UpdatePropertyAndRedirect(
                p => { p.Epc = epc; },
                viewModel.Reference,
                QuestionFlowStep.ConfirmAddress);
        }

        [HttpGet("confirm-epc-details/{reference}")]
        public async Task<IActionResult> ConfirmEpcDetails_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.ConfirmEpcDetails, propertyData);

            var viewModel = new ConfirmEpcDetailsViewModel
            {
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("ConfirmEpcDetails", viewModel);
        }

        [HttpPost("confirm-epc-details/{reference}")]
        public async Task<IActionResult> ConfirmEpcDetails_Post(ConfirmEpcDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await ConfirmEpcDetails_Get(viewModel.Reference);
            }

            return await UpdatePropertyAndRedirect(
                propertyData =>
                {
                    propertyData.EpcDetailsConfirmed = viewModel.EpcDetailsConfirmed;
                    Epc epc = propertyData.Epc;
                    if (viewModel.EpcDetailsConfirmed == EpcDetailsConfirmed.Yes)
                    {
                        propertyData.PropertyType = epc.PropertyType;
                        propertyData.HouseType = epc.HouseType;
                        propertyData.BungalowType = epc.BungalowType;
                        propertyData.FlatType = epc.FlatType;
                        propertyData.YearBuilt = epc.ConstructionAgeBand switch
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
                },
                viewModel.Reference,
                QuestionFlowStep.ConfirmEpcDetails);
        }

        [HttpGet("no-epc-found/{reference}")]
        public async Task<IActionResult> NoEpcFound_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.NoEpcFound, propertyData);

            var viewModel = new NoEpcFoundViewModel
            {
                Reference = propertyData.Reference,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values),
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("NoEpcFound", viewModel);
        }

        [HttpPost("no-epc-found/{reference}")]
        public async Task<IActionResult> NoEpcFound_Post(NoEpcFoundViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await NoEpcFound_Get(viewModel.Reference);
            }

            return await UpdatePropertyAndRedirect(p => { }, viewModel.Reference, QuestionFlowStep.NoEpcFound);
        }

        [HttpGet("property-type/{reference}")]
        public async Task<IActionResult> PropertyType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.PropertyType, propertyData, entryPoint);
            var viewModel = new PropertyTypeViewModel
            {
                PropertyType = propertyData.PropertyType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("PropertyType", viewModel);
        }

        [HttpPost("property-type/{reference}")]
        public async Task<IActionResult> PropertyType_Post(PropertyTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await PropertyType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.PropertyType = viewModel.PropertyType,
                viewModel.Reference,
                QuestionFlowStep.PropertyType,
                viewModel.EntryPoint);
        }

        [HttpGet("house-type/{reference}")]
        public async Task<IActionResult> HouseType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.HouseType, propertyData, entryPoint);
            var viewModel = new HouseTypeViewModel
            {
                HouseType = propertyData.HouseType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("HouseType", viewModel);
        }

        [HttpPost("house-type/{reference}")]
        public async Task<IActionResult> HouseType_Post(HouseTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HouseType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.HouseType = viewModel.HouseType,
                viewModel.Reference,
                QuestionFlowStep.HouseType,
                viewModel.EntryPoint);
        }

        
        [HttpGet("bungalow-type/{reference}")]
        public async Task<IActionResult> BungalowType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.BungalowType, propertyData, entryPoint);
            var viewModel = new BungalowTypeViewModel
            {
                BungalowType = propertyData.BungalowType,
                Reference = reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("BungalowType", viewModel);
        }

        [HttpPost("bungalow-type/{reference}")]
        public async Task<IActionResult> BungalowType_Post(BungalowTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await BungalowType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.BungalowType = viewModel.BungalowType,
                viewModel.Reference,
                QuestionFlowStep.BungalowType, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("flat-type/{reference}")]
        public async Task<IActionResult> FlatType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.FlatType, propertyData, entryPoint);
            var viewModel = new FlatTypeViewModel
            {
                FlatType = propertyData.FlatType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("FlatType", viewModel);
        }

        [HttpPost("flat-type/{reference}")]
        public async Task<IActionResult> FlatType_Post(FlatTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FlatType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.FlatType = viewModel.FlatType,
                viewModel.Reference,
                QuestionFlowStep.FlatType, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("home-age/{reference}")]
        public async Task<IActionResult> HomeAge_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.HomeAge, propertyData, entryPoint);
            var viewModel = new HomeAgeViewModel
            {
                PropertyType = propertyData.PropertyType,
                YearBuilt = propertyData.YearBuilt,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("HomeAge", viewModel);
        }

        [HttpPost("home-age/{reference}")]
        public async Task<IActionResult> HomeAge_Post(HomeAgeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HomeAge_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            return await UpdatePropertyAndRedirect(
                p => p.YearBuilt = viewModel.YearBuilt,
                viewModel.Reference,
                QuestionFlowStep.HomeAge, 
                viewModel.EntryPoint);
        }

        [HttpGet("check-your-unchangeable-answers/{reference}")]
        public async Task<IActionResult> CheckYourUnchangeableAnswers_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // If the user comes back to this page and their changes haven't been saved,
            // we need to revert to their old answers.
            if (propertyData.UneditedData is not null)
            {
                propertyData.RevertToUneditedData();
                await propertyDataStore.SavePropertyDataAsync(propertyData);
            }
            
            var backArgs = GetBackArgs(QuestionFlowStep.CheckYourUnchangeableAnswers, propertyData, entryPoint);
            var viewModel = new CheckYourUnchangeableAnswersViewModel()
            {
                Reference = reference,
                PropertyData = propertyData,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("CheckYourUnchangeableAnswers", viewModel);
        }

        [HttpPost("check-your-unchangeable-answers/{reference}")]
        public async Task<IActionResult> CheckYourUnchangeableAnswers_Post(CheckYourUnchangeableAnswersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await CheckYourUnchangeableAnswers_Get(viewModel.Reference, viewModel.EntryPoint);
            }

            return await UpdatePropertyAndRedirect(p => { }, viewModel.Reference, QuestionFlowStep.CheckYourUnchangeableAnswers);
        }


        [HttpGet("wall-construction/{reference}")]
        public async Task<IActionResult> WallConstruction_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.WallConstruction, propertyData, entryPoint);
            var viewModel = new WallConstructionViewModel
            {
                WallConstruction = propertyData.WallConstruction,
                HintSolidWalls = propertyData.HintSolidWalls,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("WallConstruction", viewModel);
        }

        [HttpPost("wall-construction/{reference}")]
        public async Task<IActionResult> WallConstruction_Post(WallConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await WallConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.WallConstruction = viewModel.WallConstruction,
                viewModel.Reference,
                QuestionFlowStep.WallConstruction, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public async Task<IActionResult> CavityWallsInsulated_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.CavityWallsInsulated, propertyData, entryPoint);
            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = propertyData.CavityWallsInsulated,
                YearBuilt = propertyData.YearBuilt,
                HintUninsulatedCavityWalls = propertyData.HintUninsulatedCavityWalls,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("CavityWallsInsulated", viewModel);
        }

        [HttpPost("cavity-walls-insulated/{reference}")]
        public async Task<IActionResult> CavityWallsInsulated_Post(CavityWallsInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await CavityWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.CavityWallsInsulated = viewModel.CavityWallsInsulated,
                viewModel.Reference,
                QuestionFlowStep.CavityWallsInsulated, 
                viewModel.EntryPoint);
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public async Task<IActionResult> SolidWallsInsulated_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.SolidWallsInsulated, propertyData, entryPoint);
            var viewModel = new SolidWallsInsulatedViewModel
            {
                SolidWallsInsulated = propertyData.SolidWallsInsulated,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("SolidWallsInsulated", viewModel);
        }

        [HttpPost("solid-walls-insulated/{reference}")]
        public async Task<IActionResult> SolidWallsInsulated_Post(SolidWallsInsulatedViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {
                return await SolidWallsInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.SolidWallsInsulated = viewModel.SolidWallsInsulated,
                viewModel.Reference,
                QuestionFlowStep.SolidWallsInsulated, 
                viewModel.EntryPoint);
        }


        [HttpGet("floor-construction/{reference}")]
        public async Task<IActionResult> FloorConstruction_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.FloorConstruction, propertyData, entryPoint);
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = propertyData.FloorConstruction,
                HintSuspendedTimber = propertyData.HintSuspendedTimber,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("FloorConstruction", viewModel);
        }

        [HttpPost("floor-construction/{reference}")]
        public async Task<IActionResult> FloorConstruction_Post(FloorConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FloorConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.FloorConstruction = viewModel.FloorConstruction,
                viewModel.Reference,
                QuestionFlowStep.FloorConstruction, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public async Task<IActionResult> FloorInsulated_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.FloorInsulated, propertyData, entryPoint);
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = propertyData.FloorInsulated,
                HintUninsulatedFloor = propertyData.HintUninsulatedFloor,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("FloorInsulated", viewModel);
        }

        [HttpPost("floor-insulated/{reference}")]
        public async Task<IActionResult> FloorInsulated_Post(FloorInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await FloorInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.FloorInsulated = viewModel.FloorInsulated,
                viewModel.Reference,
                QuestionFlowStep.FloorInsulated, 
                viewModel.EntryPoint);
        }

        [HttpGet("roof-construction/{reference}")]
        public async Task<IActionResult> RoofConstruction_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.RoofConstruction, propertyData, entryPoint);
            var viewModel = new RoofConstructionViewModel
            {
                RoofConstruction = propertyData.RoofConstruction,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("RoofConstruction", viewModel);
        }

        [HttpPost("roof-construction/{reference}")]
        public async Task<IActionResult> RoofConstruction_Post(RoofConstructionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await RoofConstruction_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.RoofConstruction = viewModel.RoofConstruction,
                viewModel.Reference,
                QuestionFlowStep.RoofConstruction, 
                viewModel.EntryPoint);
        }

        [HttpGet("loft-space/{reference}")]
        public async Task<IActionResult> LoftSpace_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.LoftSpace, propertyData, entryPoint);
            var viewModel = new LoftSpaceViewModel
            {
                LoftSpace = propertyData.LoftSpace,
                HintHaveLoftAndAccess = propertyData.HintHaveLoftAndAccess,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("LoftSpace", viewModel);
        }

        [HttpPost("loft-space/{reference}")]
        public async Task<IActionResult> LoftSpace_Post(LoftSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoftSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.LoftSpace = viewModel.LoftSpace,
                viewModel.Reference,
                QuestionFlowStep.LoftSpace, 
                viewModel.EntryPoint);
        }

        [HttpGet("loft-access/{reference}")]
        public async Task<IActionResult> LoftAccess_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.LoftAccess, propertyData, entryPoint);
            var viewModel = new LoftAccessViewModel
            {
                LoftAccess = propertyData.LoftAccess,
                HintHaveLoftAndAccess = propertyData.HintHaveLoftAndAccess,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("LoftAccess", viewModel);
        }
        
        [HttpPost("loft-access/{reference}")]
        public async Task<IActionResult> LoftAccess_Post(LoftAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoftAccess_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.LoftAccess = viewModel.LoftAccess,
                viewModel.Reference,
                QuestionFlowStep.LoftAccess, 
                viewModel.EntryPoint);
        }

        [HttpGet("roof-insulated/{reference}")]
        public async Task<IActionResult> RoofInsulated_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.RoofInsulated, propertyData, entryPoint);
            var viewModel = new RoofInsulatedViewModel
            {
                RoofInsulated = propertyData.RoofInsulated,
                HintUninsulatedRoof = propertyData.HintUninsulatedRoof,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("RoofInsulated", viewModel);
        }

        [HttpPost("roof-insulated/{reference}")]
        public async Task<IActionResult> RoofInsulated_Post(RoofInsulatedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await RoofInsulated_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.RoofInsulated = viewModel.RoofInsulated,
                viewModel.Reference,
                QuestionFlowStep.RoofInsulated, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("glazing-type/{reference}")]
        public async Task<IActionResult> GlazingType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.GlazingType, propertyData, entryPoint);
            var viewModel = new GlazingTypeViewModel
            {
                GlazingType = propertyData.GlazingType,
                HintSingleGlazing = propertyData.HintSingleGlazing,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("GlazingType", viewModel);
        }

        [HttpPost("glazing-type/{reference}")]
        public async Task<IActionResult> GlazingType_Post(GlazingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await GlazingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.GlazingType = viewModel.GlazingType,
                viewModel.Reference,
                QuestionFlowStep.GlazingType, 
                viewModel.EntryPoint);
        }

        [HttpGet("outdoor-space/{reference}")]
        public async Task<IActionResult> OutdoorSpace_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.OutdoorSpace, propertyData, entryPoint);
            var viewModel = new OutdoorSpaceViewModel
            {
                HasOutdoorSpace = propertyData.HasOutdoorSpace,
                HintHasOutdoorSpace = propertyData.HintHasOutdoorSpace,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("OutdoorSpace", viewModel);
        }

        [HttpPost("outdoor-space/{reference}")]
        public async Task<IActionResult> OutdoorSpace_Post(OutdoorSpaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OutdoorSpace_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.HasOutdoorSpace = viewModel.HasOutdoorSpace,
                viewModel.Reference,
                QuestionFlowStep.OutdoorSpace, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("heating-type/{reference}")]
        public async Task<IActionResult> HeatingType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.HeatingType, propertyData, entryPoint);
            var viewModel = new HeatingTypeViewModel
            {
                HeatingType = propertyData.HeatingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("HeatingType", viewModel);
        }

        [HttpPost("heating-type/{reference}")]
        public async Task<IActionResult> HeatingType_Post(HeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.HeatingType = viewModel.HeatingType,
                viewModel.Reference,
                QuestionFlowStep.HeatingType, 
                viewModel.EntryPoint);
        }

        [HttpGet("other-heating-type/{reference}")]
        public async Task<IActionResult> OtherHeatingType_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.OtherHeatingType, propertyData, entryPoint);
            var viewModel = new OtherHeatingTypeViewModel
            {
                OtherHeatingType = propertyData.OtherHeatingType,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                Epc = propertyData.Epc,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("OtherHeatingType", viewModel);
        }

        [HttpPost("other-heating-type/{reference}")]
        public async Task<IActionResult> OtherHeatingType_Post(OtherHeatingTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await OtherHeatingType_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.OtherHeatingType = viewModel.OtherHeatingType,
                viewModel.Reference,
                QuestionFlowStep.OtherHeatingType, 
                viewModel.EntryPoint);
        }


        [HttpGet("hot-water-cylinder/{reference}")]
        public async Task<IActionResult> HotWaterCylinder_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.HotWaterCylinder, propertyData, entryPoint);
            var viewModel = new HotWaterCylinderViewModel
            {
                HasHotWaterCylinder = propertyData.HasHotWaterCylinder,
                Reference = propertyData.Reference,
                Epc = propertyData.Epc,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("HotWaterCylinder", viewModel);
        }

        [HttpPost("hot-water-cylinder/{reference}")]
        public async Task<IActionResult> HotWaterCylinder_Post(HotWaterCylinderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await HotWaterCylinder_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.HasHotWaterCylinder = viewModel.HasHotWaterCylinder,
                viewModel.Reference,
                QuestionFlowStep.HotWaterCylinder, 
                viewModel.EntryPoint);
        }
        
        [HttpGet("number-of-occupants/{reference}")]
        public async Task<IActionResult> NumberOfOccupants_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = GetBackArgs(QuestionFlowStep.NumberOfOccupants, propertyData, entryPoint);
            var viewModel = new NumberOfOccupantsViewModel
            {
                NumberOfOccupants = propertyData.NumberOfOccupants,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("NumberOfOccupants", viewModel);
        }

        [HttpPost("number-of-occupants/{reference}")]
        public async Task<IActionResult> NumberOfOccupants_Post(NumberOfOccupantsViewModel viewModel, bool? skip = null)
        {
            if (skip is true)
            {
                return await UpdatePropertyAndRedirect(
                    p => p.NumberOfOccupants = null,
                    viewModel.Reference,
                    QuestionFlowStep.NumberOfOccupants, 
                    viewModel.EntryPoint);
            }
            
            if (!ModelState.IsValid)
            {
                return await NumberOfOccupants_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.NumberOfOccupants = viewModel.NumberOfOccupants,
                viewModel.Reference,
                QuestionFlowStep.NumberOfOccupants, 
                viewModel.EntryPoint);
        }
        
        [HttpGet("heating-pattern/{reference}")]
        public async Task<IActionResult> HeatingPattern_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.HeatingPattern, propertyData, entryPoint);
            var viewModel = new HeatingPatternViewModel
            {
                HeatingPattern = propertyData.HeatingPattern,
                HoursOfHeatingMorning = propertyData.HoursOfHeatingMorning,
                HoursOfHeatingEvening = propertyData.HoursOfHeatingEvening,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
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
            
            return await UpdatePropertyAndRedirect(
                p =>
                {
                    p.HeatingPattern = viewModel.HeatingPattern;
                    p.HoursOfHeatingMorning = viewModel.HoursOfHeatingMorning;
                    p.HoursOfHeatingEvening = viewModel.HoursOfHeatingEvening;
                },
                viewModel.Reference,
                QuestionFlowStep.HeatingPattern, 
                viewModel.EntryPoint);
        }
        
        [HttpGet("temperature/{reference}")]
        public async Task<IActionResult> Temperature_Get(string reference, QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = GetBackArgs(QuestionFlowStep.Temperature, propertyData, entryPoint);
            var viewModel = new TemperatureViewModel
            {
                Temperature = propertyData.Temperature,
                Reference = propertyData.Reference,
                EntryPoint = entryPoint,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("Temperature", viewModel);
        }

        [HttpPost("temperature/{reference}")]
        public async Task<IActionResult> Temperature_Post(TemperatureViewModel viewModel, bool? skip = null)
        {
            if (skip is true)
            {
                return await UpdatePropertyAndRedirect(
                    p => p.Temperature = null,
                    viewModel.Reference,
                    QuestionFlowStep.Temperature, 
                    viewModel.EntryPoint);
            }
            
            if (!ModelState.IsValid)
            {
                return await Temperature_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.Temperature = viewModel.Temperature,
                viewModel.Reference,
                QuestionFlowStep.Temperature, 
                viewModel.EntryPoint);
        }

        [HttpGet("answer-summary/{reference}")]
        public async Task<IActionResult> AnswerSummary_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // If the user comes back to this page and their changes haven't been saved,
            // we need to revert to their old answers.
            if (propertyData.UneditedData is not null)
            {
                propertyData.RevertToUneditedData();
                await propertyDataStore.SavePropertyDataAsync(propertyData);
            }

            var backArgs = GetBackArgs(QuestionFlowStep.AnswerSummary, propertyData);
            var viewModel = new AnswerSummaryViewModel
            {
                PropertyData = propertyData,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
            return View("AnswerSummary", viewModel);
        }

        [HttpPost("answer-summary/{reference}")]
        public async Task<IActionResult> AnswerSummary_Post(string reference)
        {
            if (!ModelState.IsValid)
            {
                return await AnswerSummary_Get(reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            if (propertyData.PropertyRecommendations is null || propertyData.PropertyRecommendations.Count == 0)
            {
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
            }

            propertyData.HasSeenRecommendations = true;
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var nextStep = questionFlowService.NextStep(QuestionFlowStep.AnswerSummary, propertyData);
            var forwardArgs = GetActionArgumentsForQuestion(nextStep, reference);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("returning-user/{reference}")]
        public async Task<IActionResult> ReturningUser_Get(string reference, bool fromMagicLink = true)
        {
            if (cookieService.HasAcceptedGoogleAnalytics(Request))
            {
                await googleAnalyticsService.SendEvent(new GaRequestBody
                {
                    ClientId = googleAnalyticsService.GetClientId(Request),
                    GaEvents = new List<GaEvent>
                    {
                        new()
                        {
                            Name = "user_returned",
                            Parameters = new Dictionary<string, object>
                            {
                                {"value", fromMagicLink ? "link" : "code"}
                            }
                        }
                    }
                });
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // Arriving here means the user used the reference code or the magic link,
            // so we mark the user as a returning user
            propertyData.ReturningUser = true;
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
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
            
            return RedirectToAction("ActionPlan_Get", new {reference = propertyData.Reference});
        }

        [HttpGet("no-recommendations/{reference}")]
        public async Task<IActionResult> NoRecommendations_Get(string reference, string emailAddress = null, bool emailSent = false)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.NoRecommendations, propertyData);
            var viewModel = new NoRecommendationsViewModel
            {
                Reference = propertyData.Reference,
                EmailAddress = emailAddress,
                EmailSent = emailSent,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
            return View("NoRecommendations", viewModel);
        }

        [HttpPost("no-recommendations/{reference}")]
        public async Task<IActionResult> NoRecommendations_Post(NoRecommendationsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await NoRecommendations_Get(viewModel.Reference, emailAddress: viewModel.EmailAddress);
            }

            TrySendReferenceNumberEmail(viewModel.EmailAddress, viewModel.Reference);
            
            if (!ModelState.IsValid)
            {
                return await NoRecommendations_Get(viewModel.Reference, emailAddress: viewModel.EmailAddress);
            }

            return RedirectToAction(nameof(NoRecommendations_Get), "EnergyEfficiency",
                new
                {
                    reference = viewModel.Reference, 
                    emailAddress = viewModel.EmailAddress,
                    emailSent = true
                }, "email-sent");
        }

        [HttpGet("your-recommendations/{reference}")]
        public async Task<IActionResult> YourRecommendations_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = GetBackArgs(QuestionFlowStep.YourRecommendations, propertyData);
            var viewModel = new YourRecommendationsViewModel
            {
                Reference = reference,
                NumberOfPropertyRecommendations = propertyData.PropertyRecommendations.Count,
                HasEmailAddress = false,
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
            return View("YourRecommendations", viewModel);
        }

        [HttpPost("your-recommendations/{reference}")]
        public async Task<IActionResult> YourRecommendations_Post(YourRecommendationsViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.HasEmailAddress)
            {
                TrySendReferenceNumberEmail(viewModel.EmailAddress, viewModel.Reference);
            }
            
            if (!ModelState.IsValid)
            {
                return await YourRecommendations_Get(viewModel.Reference);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            return RedirectToAction("Recommendation_Get", new { id = (int)propertyData.GetFirstRecommendationKey(), reference = viewModel.Reference });
        }

        [HttpGet("your-recommendations/{id}/{reference}")]
        public async Task<IActionResult> Recommendation_Get(int id, string reference, bool fromActionPlan = false)
        {
            var recommendationKey = (RecommendationKey)id;
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var recommendationIndex = propertyData.GetRecommendationIndex(recommendationKey);
            string backLink;

            if (fromActionPlan)
            {
                backLink = Url.Action(nameof(ActionPlan_Get), new { reference });
            }
            else if (recommendationIndex == 0)
            {
                backLink = Url.Action(nameof(YourRecommendations_Get), "EnergyEfficiency", new { reference });
            }
            else
            {
                backLink = Url.Action(nameof(Recommendation_Get), "EnergyEfficiency",
                    new { id = (int)propertyData.GetPreviousRecommendationKey(recommendationKey), reference });
            }
            
            var viewModel = new RecommendationViewModel
            {
                RecommendationIndex = recommendationIndex,
                PropertyRecommendations = propertyData.PropertyRecommendations,
                RecommendationAction = propertyData.PropertyRecommendations[recommendationIndex].RecommendationAction,
                FromActionPlan = fromActionPlan,
                BackLink = backLink
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
            return View("recommendations/" + Enum.GetName(recommendationKey), viewModel);
        }

        [HttpPost("your-recommendations/{id}/{reference}")]
        public async Task<IActionResult> Recommendation_Post(RecommendationViewModel viewModel, string command, int id, string reference)
        {
            var recommendationKey = (RecommendationKey)id;
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            if (command == "goBackwards")
            {
                return RedirectToAction(nameof(Recommendation_Get),
                    new {id = (int)propertyData.GetPreviousRecommendationKey(recommendationKey), reference = propertyData.Reference});
            }
            
            if (!ModelState.IsValid)
            {
                return await Recommendation_Get(id, reference, viewModel.FromActionPlan);
            }
            
            propertyData.PropertyRecommendations.Single(r => r.Key == recommendationKey).RecommendationAction =
                viewModel.RecommendationAction;
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            if (command == "goForwards")
            {
                if (viewModel.FromActionPlan)
                {
                    return RedirectToAction(nameof(ActionPlan_Get), new {reference = propertyData.Reference});
                }
                
                if (propertyData.GetLastRecommendationKey() == recommendationKey)
                {
                    return RedirectToAction(nameof(AlternativeRecommendations_Get), new {reference = propertyData.Reference});
                }

                return RedirectToAction(nameof(Recommendation_Get),
                    new {id = (int)propertyData.GetNextRecommendationKey(recommendationKey), reference = propertyData.Reference});
            }
            
            throw new ArgumentOutOfRangeException();
        }

        [HttpGet("alternative-recommendations/{reference}")]
        public async Task<IActionResult> AlternativeRecommendations_Get(string reference, bool fromActionPlan = false)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var viewModel = new AlternativeRecommendationsViewModel
            {
                Reference = reference,
                FromActionPlan = fromActionPlan,
                ShowAltRadiatorPanels = propertyData.ShowAltRadiatorPanels,
                ShowAltHeatPump =  propertyData.ShowAltHeatPump,
                ShowAltDraughtProofFloors = propertyData.ShowAltDraughtProofFloors,
                ShowAltDraughtProofWindowsAndDoors = propertyData.ShowAltDraughtProofWindowsAndDoors,
                ShowAltDraughtProofLoftAccess = propertyData.ShowAltDraughtProofLoftAccess,
                LastIndex = propertyData.PropertyRecommendations.Count,
                BackLink = fromActionPlan
                    ? Url.Action(nameof(ActionPlan_Get), new { reference })
                    : Url.Action(nameof(Recommendation_Get), new { id = (int)propertyData.GetLastRecommendationKey(), reference })
            };
            
            return View("Recommendations/AlternativeRecommendations", viewModel);
        }
        
        [HttpPost("alternative-recommendations/{reference}")]
        public async Task<IActionResult> AlternativeRecommendations_Post(AlternativeRecommendationsViewModel viewModel, string command)
        {
            if (!ModelState.IsValid)
            {
                return await AlternativeRecommendations_Get(viewModel.Reference, viewModel.FromActionPlan);
            }
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            switch (command)
            {
                case "goBackwards":
                    return RedirectToAction(nameof(Recommendation_Get), 
                        new {id = (int)propertyData.GetLastRecommendationKey(), reference = propertyData.Reference});
                case "goForwards":
                    return RedirectToAction(nameof(ActionPlan_Get), new {reference = propertyData.Reference});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("water-and-energy-efficiency/{reference}")]
        public IActionResult WaterAndElectricityEfficiency_Get(string reference, bool fromNoRecommendations, bool fromActionPlan = false)
        {
            var viewModel = new WaterAndEnergyEfficiencyViewModel
            {
                Reference = reference,
                FromActionPlan = fromActionPlan,
                FromNoRecommendations = fromNoRecommendations,
                BackLink = fromNoRecommendations
                    ? Url.Action(nameof(NoRecommendations_Get), new { reference })
                    : Url.Action(nameof(AlternativeRecommendations_Get), new { reference, fromActionPlan})
                    
            };
            
            return View("Recommendations/WaterAndEnergyEfficiency", viewModel);
        }
        
        [HttpPost("water-and-energy-efficiency/{reference}")]
        public IActionResult WaterAndElectricityEfficiency_Post(WaterAndEnergyEfficiencyViewModel viewModel, string command)
        {
            if (!ModelState.IsValid)
            {
                return WaterAndElectricityEfficiency_Get(viewModel.Reference, viewModel.FromActionPlan);
            }
            
            switch (command)
            {
                case "goBackwards":
                    return RedirectToAction(nameof(AlternativeRecommendations_Get), 
                        new {reference = viewModel.Reference, fromActionPlan = viewModel.FromActionPlan});
                case "goForwards":
                    return viewModel.FromNoRecommendations
                        ? RedirectToAction(nameof(NoRecommendations_Get), new {reference = viewModel.Reference})
                        : RedirectToAction(nameof(ActionPlan_Get), new {reference = viewModel.Reference});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("action-plan/{reference}")]
        public async Task<IActionResult> ActionPlan_Get(string reference, string emailAddress = null, bool emailSent = false)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var viewModel = new ActionPlanViewModel
            {
                BackLink = Url.Action(nameof(AlternativeRecommendations_Get), new { reference }),
                PropertyData = propertyData,
                EmailAddress = emailAddress,
                EmailSent = emailSent
            };

            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
            if (viewModel.GetSavedRecommendations().Any())
            {
                return View("ActionPlan/ActionPlanWithSavedRecommendations", viewModel);
            }

            if (viewModel.GetDecideLaterRecommendations().Any())
            {
                return View("ActionPlan/ActionPlanWithMaybeRecommendations", viewModel);
            }

            return View("ActionPlan/ActionPlanWithDiscardedRecommendations", viewModel);
        }

        [HttpPost("action-plan/{reference}")]
        public async Task<IActionResult> ActionPlan_Post(YourSavedRecommendationsEmailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await ActionPlan_Get(viewModel.Reference, emailAddress: viewModel.EmailAddress);
            }

            TrySendReferenceNumberEmail(viewModel.EmailAddress, viewModel.Reference);
            
            if (!ModelState.IsValid)
            {
                return await ActionPlan_Get(viewModel.Reference, emailAddress: viewModel.EmailAddress);
            }

            return RedirectToAction(nameof(ActionPlan_Get), "EnergyEfficiency",
                new
                {
                    reference = viewModel.Reference, emailAddress = viewModel.EmailAddress,
                    emailSent = true
                }, "email-sent");
        }
        
        // TODO: seabeta-576 When private beta finishes, this section should be removed. (View included)
        [HttpGet("/private-beta")]
        public IActionResult PrivateBeta_Get()
        {
            return View("PrivateBeta", new PrivateBetaViewModel());
        }
        
        // TODO: seabeta-576 When private beta finishes, this section should be removed.
        [HttpPost("/private-beta")]
        public IActionResult PrivateBeta_Post(PrivateBetaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PrivateBeta_Get();
            }

            if (!viewModel.HasAcceptedCookies)
            {
                ModelState.AddModelError(nameof(viewModel.HasAcceptedCookies), 
                    "We need your consent to enable analytics cookies on your device before you can proceed to the service.");
                return PrivateBeta_Get();
            }
            
            var cookieSettings = new CookieSettings
            {
                Version = cookieService.Configuration.CurrentCookieMessageVersion,
                ConfirmationShown = true,
                GoogleAnalytics = true
            };
            cookieService.SetCookie(Response, cookieService.Configuration.CookieSettingsCookieName, cookieSettings);

            return RedirectToAction("Index", "EnergyEfficiency");
        }

        private async Task<RedirectToActionResult> UpdatePropertyAndRedirect(
            Action<PropertyData> update,
            string reference,
            QuestionFlowStep currentPage,
            QuestionFlowStep? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // If entryPoint is set then the user is editing their answers (and if HasSeenRecommendations then they have already generated recommendations that may now need to change), so we need to take a copy of the current answers
            if ((entryPoint is not null || propertyData.HasSeenRecommendations) && propertyData.UneditedData is null)
            {
                propertyData.CreateUneditedData();
            }
            update(propertyData);
            propertyData.ResetUnusedFields();
            var nextStep = questionFlowService.NextStep(currentPage, propertyData, entryPoint);
            
            // If the user is going back to the answer summary page or the check your unchangeable answers page then they finished editing and we
            // can get rid of the old answers
            if ((entryPoint is not null || propertyData.HasSeenRecommendations) &&
                (nextStep == QuestionFlowStep.AnswerSummary ||
                 nextStep == QuestionFlowStep.CheckYourUnchangeableAnswers))
            {
                propertyData.CommitEdits();
            }
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            
            var forwardArgs = GetActionArgumentsForQuestion(nextStep, propertyData.Reference, entryPoint);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        private void TrySendReferenceNumberEmail(string emailAddress, string reference)
        {
            try
            {
                emailApi.SendReferenceNumberEmail(emailAddress, reference);
            }
            catch (EmailSenderException e)
            {
                switch (e.Type)
                {
                    case EmailSenderExceptionType.InvalidEmailAddress:
                        ModelState.AddModelError(nameof(emailAddress), "Enter a valid email address");
                        return;
                    case EmailSenderExceptionType.Other:
                        ModelState.AddModelError(nameof(emailAddress), "Unable to send email due to unexpected error. Please make a note of your reference code.");
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private PathByActionArguments GetBackArgs(
            QuestionFlowStep currentStep,
            PropertyData propertyData,
            QuestionFlowStep? entryPoint = null)
        {
            var backStep = questionFlowService.PreviousStep(currentStep, propertyData, entryPoint);
            return GetActionArgumentsForQuestion(backStep, propertyData?.Reference, entryPoint);
        }

        private PathByActionArguments GetActionArgumentsForQuestion(QuestionFlowStep question, string reference = null, QuestionFlowStep? entryPoint = null)
        {
            return question switch
            {
                QuestionFlowStep.Start => new PathByActionArguments(nameof(Index), "EnergyEfficiency"),
                QuestionFlowStep.NewOrReturningUser => new PathByActionArguments(nameof(NewOrReturningUser_Get), "EnergyEfficiency"),
                QuestionFlowStep.OwnershipStatus => new PathByActionArguments(nameof(OwnershipStatus_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.Country => new PathByActionArguments(nameof(Country_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.FindEpc => new PathByActionArguments(nameof(FindEpc_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.ServiceUnsuitable => new PathByActionArguments(nameof(ServiceUnsuitable), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.AskForPostcode => new PathByActionArguments(nameof(AskForPostcode_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.ConfirmAddress => new PathByActionArguments(nameof(ConfirmAddress_Get), "EnergyEfficiency"),
                QuestionFlowStep.ConfirmEpcDetails => new PathByActionArguments(nameof(ConfirmEpcDetails_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.NoEpcFound => new PathByActionArguments(nameof(NoEpcFound_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.PropertyType => new PathByActionArguments(nameof(PropertyType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.HouseType => new PathByActionArguments(nameof(HouseType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.BungalowType => new PathByActionArguments(nameof(BungalowType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.FlatType => new PathByActionArguments(nameof(FlatType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.HomeAge => new PathByActionArguments(nameof(HomeAge_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.CheckYourUnchangeableAnswers => new PathByActionArguments(nameof(CheckYourUnchangeableAnswers_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.WallConstruction => new PathByActionArguments(nameof(WallConstruction_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.CavityWallsInsulated => new PathByActionArguments(nameof(CavityWallsInsulated_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.SolidWallsInsulated => new PathByActionArguments(nameof(SolidWallsInsulated_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.FloorConstruction => new PathByActionArguments(nameof(FloorConstruction_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.FloorInsulated => new PathByActionArguments(nameof(FloorInsulated_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.RoofConstruction => new PathByActionArguments(nameof(RoofConstruction_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.LoftSpace => new PathByActionArguments(nameof(LoftSpace_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.LoftAccess => new PathByActionArguments(nameof(LoftAccess_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.RoofInsulated => new PathByActionArguments(nameof(RoofInsulated_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.OutdoorSpace => new PathByActionArguments(nameof(OutdoorSpace_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.GlazingType => new PathByActionArguments(nameof(GlazingType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.HeatingType => new PathByActionArguments(nameof(HeatingType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.OtherHeatingType => new PathByActionArguments(nameof(OtherHeatingType_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.HotWaterCylinder => new PathByActionArguments(nameof(HotWaterCylinder_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.NumberOfOccupants => new PathByActionArguments(nameof(NumberOfOccupants_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.HeatingPattern => new PathByActionArguments(nameof(HeatingPattern_Get), "EnergyEfficiency", GetPathValues(reference, entryPoint)),
                QuestionFlowStep.Temperature => new PathByActionArguments(nameof(Temperature_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.AnswerSummary => new PathByActionArguments(nameof(AnswerSummary_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.NoRecommendations => new PathByActionArguments(nameof(NoRecommendations_Get), "EnergyEfficiency", GetPathValues(reference)),
                QuestionFlowStep.YourRecommendations => new PathByActionArguments(nameof(YourRecommendations_Get), "EnergyEfficiency", GetPathValues(reference)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private object GetPathValues(string reference, QuestionFlowStep? entryPoint = null)
        {
            if (reference == null)
            {
                throw new ArgumentException("Reference must be provided");
            }
            
            if (entryPoint == null)
            {
                return new { reference };
            }

            return new { reference, entryPoint };
        }
        
        private class PathByActionArguments
        {
            public readonly string Action;
            public readonly string Controller;
            public readonly object Values;
        
            public PathByActionArguments(string action, string controller, object values = null)
            {
                Action = action;
                Controller = controller;
                Values = values;
            }
        }
    }
}
