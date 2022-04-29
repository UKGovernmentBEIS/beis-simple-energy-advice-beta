using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.CodeAnalysis.Operations;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class EmailAddressViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select 'Yes' if you have an email address")]
        public HasEmailAddress? HasEmailAddress { get; set; }

        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter your email address, or select 'No'", 
            IsRequiredPropertyName = nameof(IsRequiredEmailAddress))]
        public string EmailAddress { get; set; }
        
        public string Reference { get; set; }
        public bool Change { get; set; }

        public bool IsRequiredEmailAddress => HasEmailAddress == QuestionOptions.HasEmailAddress.Yes;
    }
}