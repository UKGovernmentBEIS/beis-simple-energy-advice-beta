using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AccessibleLoftViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have an accessible loft")]
        public AccessibleLoft? AccessibleLoft { get; set; }

        public string Reference { get; set; }
    }
}