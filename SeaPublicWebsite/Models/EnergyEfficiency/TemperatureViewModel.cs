using System;
using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes.DataBinding;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : QuestionFlowViewModel
    {
        [ModelBinder(typeof(GovUkMandatoryDecimalBinder))]
        [GovUkDataBindingMandatoryDecimalErrorText(nameof(ErrorMessages.TemperatureRequired), "The temperature", typeof(ErrorMessages), "SeaPublicWebsite.Resources.ErrorMessages", mustBeNumberErrorMessage: nameof(ErrorMessages.TemperatureNumber) )]
        [Range(minimum:5.0, maximum:35.0 , ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName= nameof(ErrorMessages.TemperatureDecimalRange))]
        public decimal? Temperature { get; set; }
        public string Reference { get; set; }
    }
}