using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum WallConstruction
    {
        [GovUkRadioCheckboxLabelText(Text = "Cavity")]
        Cavity,
        [GovUkRadioCheckboxLabelText(Text = "Solid")]
        Solid,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow
    }
}