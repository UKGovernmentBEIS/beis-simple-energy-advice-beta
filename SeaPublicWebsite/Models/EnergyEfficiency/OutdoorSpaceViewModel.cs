using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OutdoorSpaceViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.HasOutdoorSpaceRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public HasOutdoorSpace? HasOutdoorSpace { get; set; }
        public bool HintHasOutdoorSpace { get; set; }

        public string Reference { get; set; }
    }
}