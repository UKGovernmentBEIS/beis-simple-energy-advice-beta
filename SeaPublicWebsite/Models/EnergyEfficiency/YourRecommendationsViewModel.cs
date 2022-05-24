using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourRecommendationsViewModel
    {
        public int NumberOfUserRecommendations { get; set; }
        public string Reference { get; set; }
        public int FirstReferenceId { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select 'Yes' if you have an email address")]
        public HasEmailAddress? HasEmailAddress { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter a valid email address", 
            IsRequiredPropertyName = nameof(IsRequiredEmailAddress))]
        public string EmailAddress { get; set; }

        public bool IsRequiredEmailAddress => HasEmailAddress == QuestionOptions.HasEmailAddress.Yes;
    }
}