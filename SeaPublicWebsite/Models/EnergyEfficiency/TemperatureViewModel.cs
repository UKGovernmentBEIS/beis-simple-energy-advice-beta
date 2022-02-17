using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the temperature that you heat your home to, or choose 'Skip this question'")]
        [GovUkValidateIntRange(Minimum = 16, Maximum = 30)]
        public int? Temperature { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}