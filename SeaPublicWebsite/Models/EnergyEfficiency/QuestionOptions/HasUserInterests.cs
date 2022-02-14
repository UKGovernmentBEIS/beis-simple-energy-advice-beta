using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HasUserInterests
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No, nothing in particular. I’d like to know about everything.")]
        No,
    }
}