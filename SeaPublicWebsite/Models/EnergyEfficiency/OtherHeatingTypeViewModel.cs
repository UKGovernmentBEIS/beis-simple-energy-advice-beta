using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OtherHeatingTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your main heating system")]
        public OtherHeatingType? OtherHeatingType { get; set; }

        public string Reference { get; set; }
        public PageName? Change { get; set; }
        public Epc Epc { get; set; }
        
        public string BackLink { get; set; }
    }
}