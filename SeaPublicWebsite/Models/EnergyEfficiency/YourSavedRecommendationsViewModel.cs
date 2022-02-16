using GovUkDesignSystem;
using SeaPublicWebsite.DataModels;
using System.Collections.Generic;
using System.Linq;
using SeaPublicWebsite.Models.EnergyEfficiency.Recommendations;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourSavedRecommendationsViewModel : GovUkViewModel
    {
        public UserDataModel UserDataModel { get; set; }

        public string GetInstallationCostText()
        {
            var minCost = UserDataModel.UserRecommendations.Sum(r => r.MinInstallCost);
            var maxCost = UserDataModel.UserRecommendations.Sum(r => r.MaxInstallCost);
            return $"£{minCost} - £{maxCost}";
        }

        public List<UserRecommendation> GetSavedRecommendations()
        {
            return UserDataModel.UserRecommendations.Where(r => r.RecommendationAction == RecommendationAction.SaveToActionPlan).ToList();
        }
        public List<UserRecommendation> GetDecideLaterRecommendations()
        {
            return UserDataModel.UserRecommendations.Where(r => r.RecommendationAction == RecommendationAction.DecideLater).ToList();
        }
         
        public string GetSavingText()
        {
            var saving = UserDataModel.UserRecommendations.Sum(r => r.Saving);
            return $"£{saving} per year";
        }
        public string GetInstallationCostText(UserRecommendation recommendation)
        {
            return $"£{recommendation.MinInstallCost} - £{recommendation.MinInstallCost}";
        }

        public string GetSavingText(UserRecommendation recommendation)
        {
            return $"£{recommendation.Saving} per year";
        }
    }
}