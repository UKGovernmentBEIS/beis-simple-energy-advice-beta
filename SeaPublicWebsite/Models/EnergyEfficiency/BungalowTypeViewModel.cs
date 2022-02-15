using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class BungalowTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of bungalow do you have?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select bungalow type")]
        public BungalowType? Answer { get; set; }
    }
}