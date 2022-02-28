using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum SolidWallsInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "No, my solid walls are uninsulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "Yes, but only some of my solid walls are insulated")]
        Some,
        [GovUkRadioCheckboxLabelText(Text = "Yes, all my solid walls are insulated")]
        All,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}