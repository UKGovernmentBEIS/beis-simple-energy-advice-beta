using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum UserInterests
    {
        [GovUkRadioCheckboxLabelText(Text = "Heating systems")]
        HeatingSystems,
        [GovUkRadioCheckboxLabelText(Text = "Insulation")]
        Insulation,
    }
}