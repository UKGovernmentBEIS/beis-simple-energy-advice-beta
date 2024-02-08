using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class LoftSpaceViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.LoftSpaceRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public LoftSpace? LoftSpace { get; set; }
        public bool HintHaveLoftAndAccess { get; set; }
        public string Reference { get; set; }
    }
}