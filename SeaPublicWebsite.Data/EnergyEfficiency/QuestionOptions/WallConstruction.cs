using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum WallConstruction
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Solid walls")]
        Solid,
        [GovUkRadioCheckboxLabelText(Text = "Cavity walls")]
        Cavity,
        [GovUkRadioCheckboxLabelText(Text = "Mix of solid and cavity walls")]
        Mixed,
        [GovUkRadioCheckboxLabelText(Text = "I don’t see my option listed")]
        Other
    }
}