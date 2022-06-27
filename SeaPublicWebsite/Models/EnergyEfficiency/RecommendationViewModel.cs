using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationViewModel
    {
        public PropertyRecommendation PropertyRecommendation { get; set; }
        public PropertyData PropertyData { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select what to do with this recommendation")]
        public RecommendationAction? RecommendationAction { get; set; }
        public bool FromActionPlan { get; set; }

        public int GetCurrentIndex()
        {
            return PropertyData.PropertyRecommendations.FindIndex(r => r.Key == PropertyRecommendation.Key);
        }
        public bool HasNextIndex()
        {
            var nextIndex = GetCurrentIndex() + 1;
            return nextIndex < PropertyData.PropertyRecommendations.Count;
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
                return PropertyData.PropertyRecommendations[GetCurrentIndex() + 1].Key;
            }
            return null;
        }

        public RecommendationKey? PreviousRecommendationKey()
        {
            if (HasPreviousIndex())
            {
                return PropertyData.PropertyRecommendations[GetCurrentIndex() - 1].Key;
            }

            return null;
        }

        public List<PropertyRecommendation> GetSavedRecommendations()
        {
            return PropertyData.PropertyRecommendations.Where(r => r.RecommendationAction == BusinessLogic.Models.Enums.RecommendationAction.SaveToActionPlan).ToList();
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