using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum WallConstruction
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Solid walls")]
        Solid,
        [GovUkRadioCheckboxLabelText(Text = "Cavity walls")]
        Cavity,
        [GovUkRadioCheckboxLabelText(Text = "Mix of both")]
        Mixed,
    }
}