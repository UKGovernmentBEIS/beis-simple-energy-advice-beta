using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum GlazingType
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, all the windows are single glazed")]
        SingleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "Yes, at least one window is single glazed ")]
        Both,
        [GovUkRadioCheckboxLabelText(Text = "No, all my windows are double, triple or secondary glazed")]
        DoubleOrTripleGlazed,
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow
    }
}
