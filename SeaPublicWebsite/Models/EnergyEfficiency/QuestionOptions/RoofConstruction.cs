using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum RoofConstruction
    {
        [GovUkRadioCheckboxLabelText(Text = "Pitched roof")]
        Pitched,
        [GovUkRadioCheckboxLabelText(Text = "Flat roof")]
        Flat,
        [GovUkRadioCheckboxLabelText(Text = "Don't know")]
        DoNotKnow,
    }
}
