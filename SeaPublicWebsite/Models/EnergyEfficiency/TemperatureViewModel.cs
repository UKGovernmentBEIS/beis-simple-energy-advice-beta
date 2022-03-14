using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a number between 5 and 35, or skip this question")]
        [GovUkValidateDecimalRange(Minimum = 5, Maximum = 35, OutOfRangeErrorMessage = "Please enter a number between 5 and 35, or skip this question")]
        public decimal? Temperature { get; set; }
        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}