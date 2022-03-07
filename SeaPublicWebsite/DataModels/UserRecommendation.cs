using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.Recommendations;

namespace SeaPublicWebsite.DataModels
{
    public class UserRecommendation
    {       
        public RecommendationKey Key { get; set; }
        public int MinInstallCost { get; set; }
        public int MaxInstallCost { get; set; }
        public int Saving { get; set; }
        public int LifetimeSaving { get; set; }
        public int Lifetime { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public RecommendationAction? RecommendationAction { get; set; }
    }
}
