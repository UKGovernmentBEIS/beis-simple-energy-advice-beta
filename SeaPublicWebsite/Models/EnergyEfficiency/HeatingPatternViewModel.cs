using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating pattern")]
        public HeatingPattern? HeatingPattern { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter number of hours")]
        [GovUkValidateDecimalRange(Minimum = 0, Maximum = 24, OutOfRangeErrorMessage = "Enter a number between 0 and 24")]
        public decimal? HoursOfHeating { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}