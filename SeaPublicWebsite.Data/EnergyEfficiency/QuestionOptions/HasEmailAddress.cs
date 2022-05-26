using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum HasEmailAddress
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, email a link and the reference number to me")]
        Yes,
        No
    }
}
