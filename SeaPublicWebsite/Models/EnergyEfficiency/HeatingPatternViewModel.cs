using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating pattern")]
        public HeatingPattern? HeatingPattern { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter number of hours", IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateIntRange("Number of hours", 0, 12)]
        public int? HoursOfHeatingMorning { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageIfMissing = "Enter number of hours", IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateIntRange("Number of hours", 0, 12)]
        public int? HoursOfHeatingEvening { get; set; }

        public string Reference { get; set; }

        public bool IsRequiredHoursOfHeating => HeatingPattern == QuestionOptions.HeatingPattern.Other;
    }
}