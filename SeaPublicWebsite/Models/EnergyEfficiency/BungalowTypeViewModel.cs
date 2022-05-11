using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class BungalowTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select bungalow type")]
        public BungalowType? BungalowType { get; set; }

        public string Reference { get; set; }
        public PageName? Change { get; set; }
        
        public string BackLink { get; set; }
    }
}