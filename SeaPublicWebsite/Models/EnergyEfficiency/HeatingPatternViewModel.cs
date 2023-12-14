using GovUkDesignSystem.Attributes.DataBinding;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.GlazingTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public HeatingPattern? HeatingPattern { get; set; }
        
        [ModelBinder(typeof(GovUkOptionalIntBinder))]
        [GovUkDataBindingOptionalIntErrorText("Number of hours in the morning")] //TODO
        [GovUkValidateRequiredIf(ErrorMessageResourceName = nameof(ErrorMessages.HeatingInMorningHoursRequired), ErrorMessageResourceType = typeof(ErrorMessages), IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateIntRange("Number of hours", 0, 12, ErrorMessageResourceName = nameof(ErrorMessages.NumberOfHoursIntRange), ErrorMessageResourceType = typeof(ErrorMessages))]
        public int? HoursOfHeatingMorning { get; set; }
        
        [ModelBinder(typeof(GovUkOptionalIntBinder))]
        [GovUkDataBindingOptionalIntErrorText("Number of hours in the afternoon and evening")] //TODO
        [GovUkValidateRequiredIf(ErrorMessageResourceName = nameof(ErrorMessages.HeatingInAfternoonAndEveningHoursRequired), ErrorMessageResourceType = typeof(ErrorMessages), IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [GovUkValidateIntRange(propertyName:"Number of hours", minimum:0, maximum:12, ErrorMessageResourceName = nameof(ErrorMessages.NumberOfHoursIntRange), ErrorMessageResourceType = typeof(ErrorMessages))]
        public int? HoursOfHeatingEvening { get; set; }

        public string Reference { get; set; }

        public bool IsRequiredHoursOfHeating => HeatingPattern == BusinessLogic.Models.Enums.HeatingPattern.Other;
    }
}
