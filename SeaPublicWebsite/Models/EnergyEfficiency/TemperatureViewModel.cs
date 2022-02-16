using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you know the temperature your thermostat is set to")]
        public ThermostatTemperatureKnown? ThermostatTemperatureKnown { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the temperature that your thermostat is set to or select no")]
        [GovUkValidateIntRange(Minimum = 18, Maximum = 30)]
        public int? Temperature { get; set; }

        public string Reference { get; set; }
    }
}