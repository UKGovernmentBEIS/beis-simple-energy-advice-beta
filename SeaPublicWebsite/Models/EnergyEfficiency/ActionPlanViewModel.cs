using System.Collections.Generic;
using System.Linq;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ActionPlanViewModel
    {
        private readonly IStringLocalizer<SharedResources> sharedLocalizer;
        public PropertyData PropertyData { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailSent { get; set; }
        public string BackLink { get; set; }
        public bool IsPdf { get; set; }
        public string UrlPrefix { get; set; }
        
        public ActionPlanViewModel(IStringLocalizer<SharedResources> localizer)
        {
            sharedLocalizer = localizer;
        }

        public string GetTotalInstallationCostText()
        {
            var minCost = GetSavedRecommendations().Where(r => r.Key != RecommendationKey.InstallHeatPump).Sum(r => r.MinInstallCost);
            var maxCost = GetSavedRecommendations().Where(r => r.Key != RecommendationKey.InstallHeatPump).Sum(r => r.MaxInstallCost);
            return $"£{minCost:N0} - £{maxCost:N0}";
        }

        public List<PropertyRecommendation> GetSavedRecommendations()
        {
            return PropertyData.PropertyRecommendations.Where(r => r.RecommendationAction == RecommendationAction.SaveToActionPlan).ToList();
        }
        public List<PropertyRecommendation> GetDecideLaterRecommendations()
        {
            return PropertyData.PropertyRecommendations.Where(r => r.RecommendationAction == RecommendationAction.DecideLater).ToList();
        }
        public List<PropertyRecommendation> GetDiscardedRecommendations()
        {
            return PropertyData.PropertyRecommendations.Where(r => r.RecommendationAction == RecommendationAction.Discard).ToList();
        }

        public string GetTotalSavingText()
        {
            var saving = GetSavedRecommendations().Where(r => r.Key != RecommendationKey.InstallHeatPump).Sum(r => r.Saving);
            return string.Format(sharedLocalizer["£AYearString"].Value, $"{saving:N0}" );
        }
        public string GetInstallationCostText(PropertyRecommendation recommendation)
        {
            return recommendation.Key == RecommendationKey.InstallHeatPump ? "-" : $"£{recommendation.MinInstallCost:N0} - £{recommendation.MaxInstallCost:N0}";
        }

        public string GetSavingText(PropertyRecommendation recommendation)
        {
            return recommendation.Key == RecommendationKey.InstallHeatPump ? "-" : $"£{recommendation.Saving:N0}";
        }

        public string GetUrlWithPrefix(string url)
        {
            return $"{UrlPrefix}{url}";
        }
    }
}