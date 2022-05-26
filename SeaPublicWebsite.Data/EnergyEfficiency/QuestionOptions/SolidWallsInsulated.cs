using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum SolidWallsInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "No, my solid walls are uninsulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "Yes, but only some of my solid walls are insulated")]
        Some,
        [GovUkRadioCheckboxLabelText(Text = "Yes, all my solid walls are insulated")]
        All,
    }
}