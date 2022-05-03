using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a number between 5 and 35, or skip this question")]
        [GovUkValidateDecimalRange("The temperature", 5, 35)]
        public decimal? Temperature { get; set; }
        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}