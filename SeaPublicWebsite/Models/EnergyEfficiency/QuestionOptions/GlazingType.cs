using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum GlazingType
    {
        [GovUkRadioCheckboxLabelText(Text = "Double or triple glazed")]
        DoubleOrTripleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "Single glazed")]
        SingleGlazed
    }
}
