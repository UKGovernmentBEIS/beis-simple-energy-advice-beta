using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum RoofConstruction
    { 
        [GovUkRadioCheckboxLabelText(Text = "I only have a flat roof or roofs")]
        Flat,
        [GovUkRadioCheckboxLabelText(Text = "Some of my roof is flat and some of my roof is pitched")]
        Mixed,
        [GovUkRadioCheckboxLabelText(Text = "I only have a pitched roof or roofs")]
        Pitched,
    }
}
