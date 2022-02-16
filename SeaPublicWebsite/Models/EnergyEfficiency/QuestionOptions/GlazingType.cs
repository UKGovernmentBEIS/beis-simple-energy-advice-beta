using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum GlazingType
    {
        [GovUkRadioCheckboxLabelText(Text = "Single glazing")]
        SingleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "Double or triple glazing")]
        DoubleOrTripleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "A mix of both")]
        Both,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow
    }
}
