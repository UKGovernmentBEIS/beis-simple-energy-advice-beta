using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum HasEmailAddress
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, I have an email address")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No")]
        No
    }
}
