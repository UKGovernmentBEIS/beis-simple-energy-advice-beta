using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class EmailAddressViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select 'Yes' if you have an email address")]
        public HasEmailAddress? HasEmailAddress { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter your email address, or select 'No'")]
        public string EmailAddress { get; set; }
        
        public string Reference { get; set; }
        public bool Change { get; set; }
    }
    
    public enum HasEmailAddress
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, I have an email address")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No")]
        No
    }
}