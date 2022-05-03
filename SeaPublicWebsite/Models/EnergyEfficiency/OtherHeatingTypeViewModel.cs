using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OtherHeatingTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your main heating system")]
        public OtherHeatingType? OtherHeatingType { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
        public Epc Epc { get; set; }
    }
}