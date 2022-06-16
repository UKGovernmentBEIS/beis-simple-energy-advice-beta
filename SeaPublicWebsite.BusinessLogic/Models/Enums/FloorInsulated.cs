using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum FloorInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, my floor is insulated")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No, my floors are not insulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}