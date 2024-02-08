using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FloorInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.FloorInsulatedRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public FloorInsulated? FloorInsulated { get; set; }
        public bool? HintUninsulatedFloor { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}