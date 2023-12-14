using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.HeatingSystemRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public HeatingType? HeatingType { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}