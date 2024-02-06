using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class PropertyTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.PropertyTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public PropertyType? PropertyType { get; set; }

        public string Reference { get; set; }
    }
}