using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HasHotWaterCylinder
    {
        Yes,
        No,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}