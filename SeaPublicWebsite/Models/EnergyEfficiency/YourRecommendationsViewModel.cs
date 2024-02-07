using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourRecommendationsViewModel : QuestionFlowViewModel
    {
        public int NumberOfPropertyRecommendations { get; set; }
        public string Reference { get; set; }

        [ModelBinder(typeof(GovUkCheckboxBoolBinder))]
        public bool HasEmailAddress { get; set; }
        
        [GovUkValidateRequiredIf(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.EmailAddressRequired), 
            IsRequiredPropertyName = nameof(HasEmailAddress))]
        public string EmailAddress { get; set; }
    }
}