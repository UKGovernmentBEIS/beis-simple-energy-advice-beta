using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FlatTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.FlatTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public FlatType? FlatType { get; set; }

        public string Reference { get; set; }
    }
}