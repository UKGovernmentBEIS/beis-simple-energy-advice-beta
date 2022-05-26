﻿using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class EmailAddressViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select 'Yes' if you have an email address")]
        public HasEmailAddress? HasEmailAddress { get; set; }

        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter your email address, or select 'No'", 
            IsRequiredPropertyName = nameof(IsRequiredEmailAddress))]
        public string EmailAddress { get; set; }
        
        public string Reference { get; set; }

        public bool IsRequiredEmailAddress => HasEmailAddress == Data.EnergyEfficiency.QuestionOptions.HasEmailAddress.Yes;
    }
}