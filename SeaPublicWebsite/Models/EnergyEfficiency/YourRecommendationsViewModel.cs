using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourRecommendationsViewModel : GovUkViewModel
    {
        public int NumberOfUserRecommendations { get; set; }
        public string Reference { get; set; }
        public int FirstReferenceId { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select 'Yes' if you want to be emailed your recommendations")]
        public HasEmailAddress? HasEmailAddress { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter your email address, or select 'No'")]
        public string EmailAddress { get; set; }
    }
}