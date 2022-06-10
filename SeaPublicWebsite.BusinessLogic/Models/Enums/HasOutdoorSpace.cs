using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HasOutdoorSpace
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        Yes,
        No,
    }
}