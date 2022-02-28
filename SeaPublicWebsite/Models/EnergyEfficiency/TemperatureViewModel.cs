using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a whole number between 16 and 30, or skip this question")]
        [GovUkValidateIntRange(Minimum = 16, Maximum = 30, OutOfRangeErrorMessage = "Please enter a whole number between 16 and 30, or skip this question")]
        public int? Temperature { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}