using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum CavityWallsInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow, 
        [GovUkRadioCheckboxLabelText(Text = "Yes, they are all insulated")]
        All,
        [GovUkRadioCheckboxLabelText(Text = "Some are insulated and some not")]
        Some,
        [GovUkRadioCheckboxLabelText(Text = "No, they are not insulated")]
        No,
    }
}