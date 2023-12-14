using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.RoofInsulatedRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public RoofInsulated? RoofInsulated { get; set; }
        public bool? HintUninsulatedRoof { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}