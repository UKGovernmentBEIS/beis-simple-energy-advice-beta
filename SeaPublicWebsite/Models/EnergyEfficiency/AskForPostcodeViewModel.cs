using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AskForPostcodeViewModel : GovUkViewModel
    {
        public string Title = "What is your home address?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter a UK post code")]
        public string Postcode { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter house name or number")]
        public string HouseNameOrNumber { get; set; }
    }
}