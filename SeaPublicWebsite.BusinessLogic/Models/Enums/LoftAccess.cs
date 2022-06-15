using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum LoftAccess
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Yes, I have an accessible loft")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text= "No, my loft is not accessible")]
        No,
    }
}