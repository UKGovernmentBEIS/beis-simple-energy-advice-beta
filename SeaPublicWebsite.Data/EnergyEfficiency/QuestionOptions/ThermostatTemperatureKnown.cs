using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum ThermostatTemperatureKnown
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No")]
        No,
    }
}
