using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.DataModels;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OtherHeatingTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your main heating system")]
        public OtherHeatingType? OtherHeatingType { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}