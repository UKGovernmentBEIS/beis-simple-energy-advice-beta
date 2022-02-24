using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum WallsInsulated
    {
        Yes,
        No,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}