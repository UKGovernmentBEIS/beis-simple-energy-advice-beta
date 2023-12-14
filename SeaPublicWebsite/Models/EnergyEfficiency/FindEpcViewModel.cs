using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class FindEpcViewModel : QuestionFlowViewModel
{
    [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.SearchForEPCConfirmationRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
    public SearchForEpc? FindEpc { get; set; }

    public string Reference { get; set; }
}