using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class CountryViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.SelectPropertyCountryRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public Country? Country { get; set; }
        
        public string Reference { get; set; }
    }
}
