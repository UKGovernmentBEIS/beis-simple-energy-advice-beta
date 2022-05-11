using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your main heating system")]
        public HeatingType? HeatingType { get; set; }

        public string Reference { get; set; }
        public PageName? EntryPoint { get; set; }
        public Epc Epc { get; set; }
        
        public string BackLink { get; set; }
    }
}