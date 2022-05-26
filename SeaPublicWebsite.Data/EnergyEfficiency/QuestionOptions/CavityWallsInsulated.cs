using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum CavityWallsInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow, 
        [GovUkRadioCheckboxLabelText(Text = "No, my cavity walls are uninsulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "Yes, but only some of my cavity walls are insulated")]
        Some,
        [GovUkRadioCheckboxLabelText(Text = "Yes, all my cavity walls are insulated")]
        All,
    }
}