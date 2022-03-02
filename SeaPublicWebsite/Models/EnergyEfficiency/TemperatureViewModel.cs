using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a number between 16 and 30, or skip this question")]
        [GovUkValidateDecimalRange(Minimum = 16, Maximum = 30, OutOfRangeErrorMessage = "Please enter a number between 16 and 30, or skip this question")]
        public decimal? Temperature { get; set; }
        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}