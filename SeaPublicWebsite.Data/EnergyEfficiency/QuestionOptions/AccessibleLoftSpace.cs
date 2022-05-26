using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum AccessibleLoftSpace
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Yes, I have a loft space that I think is accessible")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text= "No, I don’t have any accessible loft space")]
        No,
    }
}