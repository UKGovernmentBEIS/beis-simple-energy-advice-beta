using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class SolidWallsInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.SolidWallsInsulatedRequired))]
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}