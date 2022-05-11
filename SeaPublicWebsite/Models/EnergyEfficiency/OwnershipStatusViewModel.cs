using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OwnershipStatusViewModel
    {
        public string Reference { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your circumstances")]
        public OwnershipStatus? OwnershipStatus { get; set; }

        public PageName? Change { get; set; }
        
        public string BackLink { get; set; }
    }
}