using GovUkDesignSystem;
using SeaPublicWebsite.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourSavedRecommendationsViewModel : GovUkViewModel
    {
        public List<Recommendation> Recommendations { get; set; }
        public UserDataModel UserData { get; set; }

        public string GetInstallationCostText()
        {
            var minCost = Recommendations.Sum(r => r.MinInstallCost);
            var maxCost = Recommendations.Sum(r => r.MaxInstallCost);
            return $"£{minCost} - £{maxCost}";
        }

        public string GetSavingText()
        {
            var saving = Recommendations.Sum(r => r.Saving);
            return $"£{saving} per year";
        }
        public string GetInstallationCostText(Recommendation recommendation)
        {
            return $"£{recommendation.MinInstallCost} - £{recommendation.MinInstallCost}";
        }

        public string GetSavingText(Recommendation recommendation)
        {
            return $"£{recommendation.Saving} per year";
        }
    }
}