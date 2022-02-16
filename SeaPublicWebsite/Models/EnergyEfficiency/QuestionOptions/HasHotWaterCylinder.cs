using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HasHotWaterCylinder
    {
        Yes,
        No,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow
    }
}