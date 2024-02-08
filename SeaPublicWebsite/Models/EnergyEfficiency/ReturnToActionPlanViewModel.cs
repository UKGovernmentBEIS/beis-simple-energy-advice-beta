using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class ReturnToActionPlanViewModel
{
    public string Reference { get; set; }
    [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.EmailAddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
    public string EmailAddress { get; set; }
    public bool EmailSent { get; set; }
    public bool IsPdf { get; set; }
}