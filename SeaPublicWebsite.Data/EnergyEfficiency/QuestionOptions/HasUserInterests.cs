using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum HasUserInterests
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No, nothing in particular. I’d like to know about everything.")]
        No,
    }
}