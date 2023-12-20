using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using GovUkDesignSystem.Attributes.DataBinding;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Localization;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.GlazingTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public HeatingPattern? HeatingPattern { get; set; }
        
        [ModelBinder(typeof(GovUkOptionalIntBinder))]
        [GovUkDataBindingOptionalIntErrorText("Number of hours in the morning")] //TODO error message localise
        [GovUkValidateRequiredIf(ErrorMessageResourceName = nameof(ErrorMessages.HeatingInMorningHoursRequired), ErrorMessageResourceType = typeof(ErrorMessages), IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [Range(minimum:0, maximum:12, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName= nameof(ErrorMessages.NumberOfHoursIntRange))]
        public int? HoursOfHeatingMorning { get; set; }

        [ModelBinder(typeof(GovUkOptionalIntBinder))]
        [GovUkDataBindingOptionalIntErrorText("Number of hours in the afternoon and evening")] //TODO error message localise
        [GovUkValidateRequiredIf(
            ErrorMessageResourceName = nameof(ErrorMessages.HeatingInAfternoonAndEveningHoursRequired),
            ErrorMessageResourceType = typeof(ErrorMessages),
            IsRequiredPropertyName = nameof(IsRequiredHoursOfHeating))]
        [Range(minimum:0, maximum:12, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName= nameof(ErrorMessages.NumberOfHoursIntRange))]
        public int? HoursOfHeatingEvening { get; set; }

        public string Reference { get; set; }
        
        public bool IsRequiredHoursOfHeating => HeatingPattern == BusinessLogic.Models.Enums.HeatingPattern.Other;
    }
}
