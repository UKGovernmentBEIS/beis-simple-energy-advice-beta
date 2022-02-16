using SeaPublicWebsite.Models.EnergyEfficiency;
using System.Collections.Generic;

namespace SeaPublicWebsite.DataModels
{
    public class Recommendation
    {
        public RecommendationKey Key { get; set; }
        public string Title { get; set; }
        public int MinInstallCost { get; set; }
        public int MaxInstallCost { get; set; }
        public int Saving { get; set; }
        public string InstallationTime { get; set; }
        public List<string> Description { get; set; }
        public string Considerations { get; set; }
        public string FurtherInfo { get; set; }
        public string Disruption { get; set; }
        public string Caution { get; set; }
        public RecommendationSuitability Suitability { get; set; }
        public string Summary { get; set; }
    }

    public class RecommendationSuitability
    {
        public string IntroText { get; set; }
        public List<string> SuitabilityPoints { get; set; }
    }
}
