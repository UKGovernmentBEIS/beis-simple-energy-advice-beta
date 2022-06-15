using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AskForPostcodeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter a UK post code")]
        public string Postcode { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter house name or number")]
        public string HouseNameOrNumber { get; set; }

        public string Reference { get; set; }
    }
}