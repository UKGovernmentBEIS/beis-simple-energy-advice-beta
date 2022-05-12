using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AccessibleLoftSpaceViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have accessible loft space")]
        public AccessibleLoftSpace? AccessibleLoftSpace { get; set; }

        public string Reference { get; set; }
    }
}