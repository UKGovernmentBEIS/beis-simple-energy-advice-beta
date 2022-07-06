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
using SeaPublicWebsite.ExternalServices.GoogleAnalytics;
using SeaPublicWebsite.ExternalServices.PostcodesIo;
using SeaPublicWebsite.Helpers;
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

                return await ReturningUser_Get(viewModel.Reference, fromMagicLink: false);
            }

            string reference = await propertyDataStore.CreateNewPropertyDataAsync();

            return RedirectToAction("Country_Get", "EnergyEfficiency", new { reference = reference });
        }

        
        [HttpGet("ownership-status/{reference}")]
        public async Task<IActionResult> OwnershipStatus_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs =
                questionFlowService.BackLinkArguments(QuestionFlowPage.OwnershipStatus, propertyData);
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
                QuestionFlowPage.OwnershipStatus);
        }

        
        [HttpGet("country/{reference}")]
        public async Task<IActionResult> Country_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Country, propertyData);
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
                QuestionFlowPage.Country);
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
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_BEFORE_ANSWER_SUMMARY;
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

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
            
            propertyData.SearchForEpc = viewModel.FindEpc;
            propertyData.EpcDetailsConfirmed = null;
            propertyData.Epc = null;
            propertyData.PropertyType = null;
            propertyData.YearBuilt = null;
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
                    QuestionFlowPage.AskForPostcode);
            }
            
            if (viewModel.Postcode is not null && !(await postcodesIoApi.IsValidPostcode(viewModel.Postcode)))
            {
                ModelState.AddModelError(nameof(AskForPostcodeViewModel.Postcode), "Enter a valid UK postcode");
            }
            
            if (!ModelState.IsValid)
            {
                return await AskForPostcode_Get(viewModel.Reference);
            }
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AskForPostcode, propertyData);

            return RedirectToAction(
                forwardArgs.Action,
                forwardArgs.Controller,
                // TODO: seabeta-579 we'd rather use the values from the question flow service
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
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.ConfirmAddress, propertyData);
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
                QuestionFlowPage.ConfirmAddress);
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
                QuestionFlowPage.ConfirmAddress);
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
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);
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
            PropertyDataHelper.ResetUnusedFields(propertyData);
            await propertyDataStore.SavePropertyDataAsync(propertyData);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.ConfirmEpcDetails, propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }

        [HttpGet("no-epc-found/{reference}")]
        public async Task<IActionResult> NoEpcFound_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NoEpcFound, propertyData);

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
            
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference); 
            
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
                QuestionFlowPage.PropertyType,
                viewModel.EntryPoint);
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
                QuestionFlowPage.HouseType,
                viewModel.EntryPoint);
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
                QuestionFlowPage.BungalowType, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.FlatType, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("home-age/{reference}")]
        public async Task<IActionResult> HomeAge_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.HomeAge, propertyData, entryPoint);
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
                QuestionFlowPage.HomeAge, 
                viewModel.EntryPoint);
        }

        [HttpGet("check-your-unchangeable-answers/{reference}")]
        public async Task<IActionResult> CheckYourUnchangeableAnswers_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // If the user comes back to this page and their changes haven't been saved,
            // we need to revert to their old answers.
            if (propertyData.UneditedData is not null)
            {
                propertyData.RevertToUneditedData();
                await propertyDataStore.SavePropertyDataAsync(propertyData);
            }
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.CheckYourUnchangeableAnswers, propertyData, entryPoint);
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

            var propertyData = await propertyDataStore.LoadPropertyDataAsync(viewModel.Reference);

            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.CheckYourUnchangeableAnswers, propertyData, viewModel.EntryPoint);
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
                QuestionFlowPage.WallConstruction, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("cavity-walls-insulated/{reference}")]
        public async Task<IActionResult> CavityWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.CavityWallsInsulated, propertyData, entryPoint);
            var viewModel = new CavityWallsInsulatedViewModel
            {
                CavityWallsInsulated = propertyData.CavityWallsInsulated,
                YearBuilt = propertyData.YearBuilt,
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
                QuestionFlowPage.CavityWallsInsulated, 
                viewModel.EntryPoint);
        }


        [HttpGet("solid-walls-insulated/{reference}")]
        public async Task<IActionResult> SolidWallsInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.SolidWallsInsulated, propertyData, entryPoint);
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
                QuestionFlowPage.SolidWallsInsulated, 
                viewModel.EntryPoint);
        }


        [HttpGet("floor-construction/{reference}")]
        public async Task<IActionResult> FloorConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorConstruction, propertyData, entryPoint);
            var viewModel = new FloorConstructionViewModel
            {
                FloorConstruction = propertyData.FloorConstruction,
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
                QuestionFlowPage.FloorConstruction, 
                viewModel.EntryPoint);
        }

        
        [HttpGet("floor-insulated/{reference}")]
        public async Task<IActionResult> FloorInsulated_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.FloorInsulated, propertyData, entryPoint);
            var viewModel = new FloorInsulatedViewModel
            {
                FloorInsulated = propertyData.FloorInsulated,
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
                QuestionFlowPage.FloorInsulated, 
                viewModel.EntryPoint);
        }

        [HttpGet("roof-construction/{reference}")]
        public async Task<IActionResult> RoofConstruction_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.RoofConstruction, propertyData, entryPoint);
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
                QuestionFlowPage.RoofConstruction, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.LoftSpace, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.LoftAccess, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.RoofInsulated, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.GlazingType, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.OutdoorSpace, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.HeatingType, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.OtherHeatingType, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.HotWaterCylinder, 
                viewModel.EntryPoint);
        }
        
        [HttpGet("number-of-occupants/{reference}")]
        public async Task<IActionResult> NumberOfOccupants_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NumberOfOccupants, propertyData, entryPoint);
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
                    QuestionFlowPage.NumberOfOccupants, 
                    viewModel.EntryPoint);
            }
            
            if (!ModelState.IsValid)
            {
                return await NumberOfOccupants_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.NumberOfOccupants = viewModel.NumberOfOccupants,
                viewModel.Reference,
                QuestionFlowPage.NumberOfOccupants, 
                viewModel.EntryPoint);
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
                QuestionFlowPage.HeatingPattern, 
                viewModel.EntryPoint);
        }
        
        [HttpGet("temperature/{reference}")]
        public async Task<IActionResult> Temperature_Get(string reference, QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.Temperature, propertyData, entryPoint);
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
                    QuestionFlowPage.Temperature, 
                    viewModel.EntryPoint);
            }
            
            if (!ModelState.IsValid)
            {
                return await Temperature_Get(viewModel.Reference, viewModel.EntryPoint);
            }
            
            return await UpdatePropertyAndRedirect(
                p => p.Temperature = viewModel.Temperature,
                viewModel.Reference,
                QuestionFlowPage.Temperature, 
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

            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.AnswerSummary, propertyData);
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
            
            var forwardArgs = questionFlowService.ForwardLinkArguments(QuestionFlowPage.AnswerSummary, propertyData);
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
        public async Task<IActionResult> NoRecommendations_Get(string reference)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var backArgs = questionFlowService.BackLinkArguments(QuestionFlowPage.NoRecommendations, propertyData);
            var viewModel = new NoRecommendationsViewModel
            {
                BackLink = Url.Action(backArgs.Action, backArgs.Controller, backArgs.Values)
            };
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
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
            
            ViewBag.FeedbackUrl = propertyData.ReturningUser
                ? Constants.FEEDBACK_URL_BANNER_RETURNING_USER
                : Constants.FEEDBACK_URL_BANNER_AFTER_ANSWER_SUMMARY;
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
                            ModelState.AddModelError(nameof(viewModel.EmailAddress), "Unable to send email due to unexpected error. Please make a note of your reference code");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return await YourRecommendations_Get(viewModel.Reference);
                }
            }
            
            return RedirectToAction("Recommendation_Get", new { id = (int)propertyData.GetFirstRecommendationKey(), reference = viewModel.Reference });
        }

        [HttpGet("your-recommendations/{id}/{reference}")]
        public async Task<IActionResult> Recommendation_Get(int id, string reference, bool fromActionPlan = false)
        {
            var recommendationKey = (RecommendationKey)id;
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            var recommendationIndex = propertyData.GetRecommendationIndex(recommendationKey);
            string backLink = null;

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

            switch(command)
            {
                case "goForwards":
                    return RedirectToAction(nameof(Recommendation_Get),
                        new {id = (int)propertyData.GetNextRecommendationKey(recommendationKey), reference = propertyData.Reference});
                case "goToActionPlan":
                    return RedirectToAction(nameof(ActionPlan_Get), new {reference = propertyData.Reference});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("action-plan/{reference}")]
        public async Task<IActionResult> ActionPlan_Get(string reference, string emailAddress = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            var viewModel = new ActionPlanViewModel
            {
                BackLink = Url.Action(nameof(Recommendation_Get), new { id = (int)propertyData.GetLastRecommendationKey(), reference }),
                PropertyData = propertyData,
                EmailAddress = emailAddress
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
                return await ActionPlan_Get(viewModel.Reference);
            }
            
            try
            {
                emailApi.SendReferenceNumberEmail(viewModel.EmailAddress, viewModel.Reference);
            }
            catch (EmailSenderException e)
            {
                switch (e.Type)
                {
                    case EmailSenderExceptionType.InvalidEmailAddress:
                        ModelState.AddModelError(nameof(viewModel.EmailAddress), "Enter a valid email address");
                        break;
                    case EmailSenderExceptionType.Other:
                        ModelState.AddModelError(nameof(viewModel.EmailAddress), "Unable to send email due to unexpected error. Please make a note of your reference code.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return await ActionPlan_Get(viewModel.Reference);
            }
            return await ActionPlan_Get(viewModel.Reference, emailAddress: viewModel.EmailAddress);
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
            QuestionFlowPage currentPage,
            QuestionFlowPage? entryPoint = null)
        {
            var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
            // If entryPoint is set then the user is editing their answers, so we need to take a copy of the current answers
            if (entryPoint is not null && propertyData.UneditedData is null)
            {
                propertyData.CreateUneditedData();
            }
            update(propertyData);
            PropertyDataHelper.ResetUnusedFields(propertyData);
            var forwardArgs = questionFlowService.ForwardLinkArguments(currentPage, propertyData, entryPoint);
            
            // If the user is going back to the answer summary page or the check your unchangeable answers page then they finished editing and we
            // can get rid of the old answers
            if (entryPoint is not null &&
                (forwardArgs.Action.Equals(nameof(AnswerSummary_Get)) ||
                 forwardArgs.Action.Equals(nameof(CheckYourUnchangeableAnswers_Get))))
            {
                propertyData.CommitEdits();
            }
            await propertyDataStore.SavePropertyDataAsync(propertyData);
            return RedirectToAction(forwardArgs.Action, forwardArgs.Controller, forwardArgs.Values);
        }
    }
}
