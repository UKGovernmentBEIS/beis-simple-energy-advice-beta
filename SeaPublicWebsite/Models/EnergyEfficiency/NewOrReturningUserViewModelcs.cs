using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class NewOrReturningUserViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have used this service before")]
        public NewOrReturningUser? NewOrReturningUser { get; set; }

        [GovUkValidateRequiredIf(
            ErrorMessageIfMissing = "Enter your 8-digit reference (or choose the 'This is my first visit' option)", 
            IsRequiredPropertyName = nameof(RefRequired))]
        public string Reference { get; set; }
        public bool RefRequired => NewOrReturningUser == EnergyEfficiency.NewOrReturningUser.ReturningUser;
    }
    
    public enum NewOrReturningUser
    {
        [GovUkRadioCheckboxLabelText(Text = "No, this is my first visit or I don’t have a reference number")]
        NewUser,
        [GovUkRadioCheckboxLabelText(Text = "Yes, and I have the 8-digit reference number from my previous visit")]
        ReturningUser,
    }
}