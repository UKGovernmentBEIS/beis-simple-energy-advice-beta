using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourRecommendationsViewModel
    {
        public int NumberOfUserRecommendations { get; set; }
        public string Reference { get; set; }
        public int FirstReferenceId { get; set; }

        public HasEmailAddress? HasEmailAddress { get; set; }

        public string EmailAddress { get; set; }
    }
}