using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum GlazingType
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Single glazing")]
        SingleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "Double, triple or secondary glazing")]
        DoubleOrTripleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "A mix of both")]
        Both
    }
}
