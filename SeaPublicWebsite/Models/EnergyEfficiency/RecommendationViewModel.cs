using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency;
using SeaPublicWebsite.Data.EnergyEfficiency.Recommendations;
using SeaPublicWebsite.DataModels;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationViewModel
    {
        public UserRecommendation UserRecommendation { get; set; }
        public UserDataModel UserDataModel { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select what to do with this recommendation")]
        public RecommendationAction? RecommendationAction { get; set; }

        public int GetCurrentIndex()
        {
            return UserDataModel.UserRecommendations.FindIndex(r => r.Key == UserRecommendation.Key);
        }
        public bool HasNextIndex()
        {
            var nextIndex = GetCurrentIndex() + 1;
            return nextIndex < UserDataModel.UserRecommendations.Count;
        }

        public bool HasPreviousIndex()
        {
            var previousIndex = GetCurrentIndex() - 1;
            return previousIndex >= 0;
        }

        public RecommendationKey? NextRecommendationKey()
        {
            if (HasNextIndex())
            {
                return UserDataModel.UserRecommendations[GetCurrentIndex() + 1].Key;
            }
            return null;
        }

        public RecommendationKey? PreviousRecommendationKey()
        {
            if (HasPreviousIndex())
            {
                return UserDataModel.UserRecommendations[GetCurrentIndex() - 1].Key;
            }

            return null;
        }

        public List<UserRecommendation> GetSavedRecommendations()
        {
            return UserDataModel.UserRecommendations.Where(r => r.RecommendationAction == Data.EnergyEfficiency.Recommendations.RecommendationAction.SaveToActionPlan).ToList();
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
            return $"£{saving:N0} a year";
        }
    }
}