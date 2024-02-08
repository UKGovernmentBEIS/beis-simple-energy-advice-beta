using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FloorConstructionViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.FloorConstructionTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public FloorConstruction? FloorConstruction { get; set; }
        public bool? HintSuspendedTimber { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}