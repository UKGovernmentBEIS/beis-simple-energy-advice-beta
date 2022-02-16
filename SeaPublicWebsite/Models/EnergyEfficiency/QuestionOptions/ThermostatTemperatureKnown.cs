using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum ThermostatTemperatureKnown
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No")]
        No,
    }
}
