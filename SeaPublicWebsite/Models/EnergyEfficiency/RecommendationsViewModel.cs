using System.Collections.Generic;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationsViewModel
    {
        public List<PropertyRecommendation> PropertyRecommendations { get; set; }
        public EnergyPriceCapInfo EnergyPriceCapInfo { get; set; }
    }
}