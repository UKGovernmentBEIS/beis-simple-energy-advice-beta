using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class YourSavedRecommendationsEmailViewModel : QuestionFlowViewModel
    {
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.EmailAddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public string EmailAddress { get; set; }
        public bool EmailSent { get; set; }
        public string PostAction { get; set; }
    }
}