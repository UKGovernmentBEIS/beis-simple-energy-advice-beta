using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AccessibleLoftSpaceViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have accessible loft space")]
        public AccessibleLoftSpace? AccessibleLoftSpace { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}