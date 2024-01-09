using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
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
        [GovUkDataBindingMandatoryIntErrorText(nameof(ErrorMessages.NumberOfOccupantsRequired), "The number of people who live in the property", typeof(ErrorMessages), "SeaPublicWebsite.Resources.ErrorMessages", nameof(ErrorMessages.NumberOfOccupantsWholeNumber), nameof(ErrorMessages.NumberOfOccupantsNumber))]
        [Range(minimum:1, maximum:9, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName= nameof(ErrorMessages.NumberOfOccupantsIntRange))]
        public int? NumberOfOccupants { get; set; }

        public string Reference { get; set; }
    }
}