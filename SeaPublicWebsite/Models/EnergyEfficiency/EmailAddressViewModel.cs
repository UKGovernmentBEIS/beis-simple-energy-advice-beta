using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class EmailAddressViewModel : GovUkViewModel
    {
        public HasEmailAddress? HasEmailAddress { get; set; }

        public string Reference { get; set; }
        public string EmailAddress { get; set; }
        public bool Change { get; set; }
    }
    
    public enum HasEmailAddress
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No")]
        No
    }
}