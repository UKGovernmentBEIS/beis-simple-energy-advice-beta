using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        public string Title = "What temperature do you set your thermostat to?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.Behaviour ;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the temperature that your thermostat is set to or select No")]
        [GovUkValidateIntRange(Minimum = 12, Maximum = 28)]
        public int? Temperature { get; set; }
    }
}