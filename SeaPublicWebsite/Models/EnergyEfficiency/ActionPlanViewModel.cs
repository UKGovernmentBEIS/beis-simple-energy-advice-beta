using System.Collections.Generic;
using System.Linq;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ActionPlanViewModel
    {
        public PropertyData PropertyData { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailSent { get; set; }
        public string BackLink { get; set; }

        public string GetTotalInstallationCostText()
        {
            var minCost = GetSavedRecommendations().Sum(r => r.MinInstallCost);
            var maxCost = GetSavedRecommendations().Sum(r => r.MaxInstallCost);
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
            var saving = GetSavedRecommendations().Sum(r => r.Saving);
            return $"£{saving:N0} a year";
        }
        public string GetInstallationCostText(PropertyRecommendation recommendation)
        {
            //TODO: revert this conditional just to display the latter expression when energy prices stabilise
            return recommendation.Key == RecommendationKey.InstallHeatPump ? "-" : $"£{recommendation.MinInstallCost:N0} - £{recommendation.MaxInstallCost:N0}";
        }

        public string GetSavingText(PropertyRecommendation recommendation)
        {
            //TODO: revert this conditional just to display the latter expression when energy prices stabilise
            return recommendation.Key == RecommendationKey.InstallHeatPump ? "-" : $"£{recommendation.Saving:N0}";
        }
    }
}