using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum FloorInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "No, my floors are not insulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "Yes, my floor is insulated")]
        Yes,
        // [GovUkRadioCheckboxLabelText(Text = "Yes, my solid floor is insulated but my suspended timber floor is not insulated")]
        // SolidFloorOnly,
        // [GovUkRadioCheckboxLabelText(Text = "Yes, my suspended timber floor is insulated but my solid floor is not insulated")]
        // SuspendedTimberFloorOnly,
    }
}