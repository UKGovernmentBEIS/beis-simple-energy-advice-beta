using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.Extensions.Localization;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationViewModel
    {
        private readonly IStringLocalizer<SharedResources> sharedLocalizer;
        
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.RecommendationActionRequired))]
        public RecommendationAction? RecommendationAction { get; set; }
        public int RecommendationIndex { get; set; }
        public List<PropertyRecommendation> PropertyRecommendations { get; set; }
        public bool FromActionPlan { get; set; }
        public string BackLink { get; set; }
        public string Reference { get; set; }
        public PropertyRecommendation GetCurrentPropertyRecommendation() => PropertyRecommendations[RecommendationIndex];

        public RecommendationViewModel(IStringLocalizer<SharedResources> localizer)
        {
            sharedLocalizer = localizer;
        }

        public bool HasPreviousIndex()
        {
            return RecommendationIndex > 0;
        }
        
        public bool HasNextIndex()
        {
            return RecommendationIndex < PropertyRecommendations.Count - 1;
        }

        public List<PropertyRecommendation> GetSavedRecommendations()
        {
            return PropertyRecommendations.Where(r => r.RecommendationAction == BusinessLogic.Models.Enums.RecommendationAction.SaveToActionPlan).ToList();
        }
        
        public string GetTotalInstallationCostText()
        {
            var minCost = GetSavedRecommendations().Sum(r => r.MinInstallCost);
            var maxCost = GetSavedRecommendations().Sum(r => r.MaxInstallCost);
            return $"£{minCost:N0} - £{maxCost:N0}";
        }

        public string GetTotalSavingText()
        {
            var saving = GetSavedRecommendations().Sum(r => r.Saving);
            return sharedLocalizer[$"£{saving:N0} a year"].Value;
        }
    }
}