using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AccessibleLoftSpaceViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if you have accessible loft space")]
        public AccessibleLoftSpace? AccessibleLoftSpace { get; set; }

        public string Reference { get; set; }
        public PageName? EntryPoint { get; set; }
        
        public string BackLink { get; set; }
    }
}