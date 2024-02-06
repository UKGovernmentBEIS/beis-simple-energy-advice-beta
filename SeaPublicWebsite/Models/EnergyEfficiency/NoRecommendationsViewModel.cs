using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class NoRecommendationsViewModel: QuestionFlowViewModel
{
    public string Reference { get; set; }
    [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.EmailAddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
    public string EmailAddress { get; set; }
    public bool EmailSent { get; set; }
    
    public HasOutdoorSpace? HasOutdoorSpace { get; set; }
}