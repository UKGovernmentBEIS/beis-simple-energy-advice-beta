using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourRecommendationsViewModel : QuestionFlowViewModel
    {
        public int NumberOfUserRecommendations { get; set; }
        public string Reference { get; set; }
        public int FirstReferenceId { get; set; }

        [ModelBinder(typeof(GovUkCheckboxBoolBinder))]
        public bool HasEmailAddress { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter a valid email address", 
            IsRequiredPropertyName = nameof(HasEmailAddress))]
        public string EmailAddress { get; set; }
    }
}