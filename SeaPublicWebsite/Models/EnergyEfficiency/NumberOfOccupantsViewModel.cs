using GovUkDesignSystem.Attributes.DataBinding;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class NumberOfOccupantsViewModel : QuestionFlowViewModel
    {
        [ModelBinder(typeof(GovUkMandatoryIntBinder))]
        [GovUkDataBindingMandatoryIntErrorText("Enter the number of people who live in the property", "The number of people who live in the property")] //TODO
        [GovUkValidateIntRange("The number of people who live in the property", 1, 9, ErrorMessageResourceName = nameof(ErrorMessages.NumberOfOccupantsIntRange), ErrorMessageResourceType = typeof(ErrorMessages))]
        public int? NumberOfOccupants { get; set; }

        public string Reference { get; set; }
    }
}