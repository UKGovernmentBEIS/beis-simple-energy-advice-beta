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
        [GovUkDataBindingMandatoryDecimalErrorText("Enter a number between 5 and 35, or skip this question", "The temperature")] //TODO
        [GovUkValidateDecimalRange("The temperature", 5, 35, ErrorMessageResourceName = nameof(ErrorMessages.TemperatureDecimalRange), ErrorMessageResourceType = typeof(ErrorMessages))]
        public decimal? Temperature { get; set; }
        public string Reference { get; set; }
    }
}