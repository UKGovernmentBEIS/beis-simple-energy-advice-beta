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
        public RecommendationAction? State { get; set; }
    }
}
