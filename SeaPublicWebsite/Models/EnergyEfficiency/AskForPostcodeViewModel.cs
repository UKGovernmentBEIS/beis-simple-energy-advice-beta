using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AskForPostcodeViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter a UK post code")]
        public string Postcode { get; set; }
    }
}