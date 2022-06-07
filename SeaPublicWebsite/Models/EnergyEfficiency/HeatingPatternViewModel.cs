using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating pattern")]
        public HeatingPattern? HeatingPattern { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter number of hours", IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateDecimalRange("Number of hours", 0, 12)]
        public decimal? HoursOfHeatingMorning { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter number of hours", IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateDecimalRange("Number of hours", 0, 12)]
        public decimal? HoursOfHeatingEvening { get; set; }

        public string Reference { get; set; }

        public bool IsRequiredHoursOfHeating => HeatingPattern == Data.EnergyEfficiency.QuestionOptions.HeatingPattern.Other;
    }
}