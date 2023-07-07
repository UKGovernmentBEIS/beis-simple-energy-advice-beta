using System.Collections.Generic;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class SingleRecommendationDetailsViewModel
    {
        public int RecommendationIndex { get; set; }
        public List<PropertyRecommendation> PropertyRecommendations { get; set; }
        public TagViewModel DisruptionTagViewModel { get; set;  }
        public TagViewModel DurationTagViewModel { get; set;  }

        public PropertyRecommendation GetCurrentPropertyRecommendation() => PropertyRecommendations[RecommendationIndex];
    }
}
