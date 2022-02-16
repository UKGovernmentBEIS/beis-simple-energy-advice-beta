using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using System.Collections.Generic;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationViewModel : GovUkViewModel
    {
        public Recommendation Recommendation { get; set; }
        public UserDataModel UserDataModel { get; set; }
    }
}