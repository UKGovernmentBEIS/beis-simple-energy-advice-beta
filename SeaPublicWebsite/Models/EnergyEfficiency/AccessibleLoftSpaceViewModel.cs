using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AccessibleLoftSpaceViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have accessible loft space")]
        public AccessibleLoftSpace? AccessibleLoftSpace { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}