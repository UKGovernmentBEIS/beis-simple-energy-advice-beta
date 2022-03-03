using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HasHotWaterCylinder
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        Yes,
        No
    }
}