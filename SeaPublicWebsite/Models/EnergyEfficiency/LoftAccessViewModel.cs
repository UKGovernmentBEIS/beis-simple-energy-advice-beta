using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class LoftAccessViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.LoftAccessRequired))]
        public LoftAccess? LoftAccess { get; set; }
        public bool HintHaveLoftAndAccess { get; set; }

        public string Reference { get; set; }
    }
}