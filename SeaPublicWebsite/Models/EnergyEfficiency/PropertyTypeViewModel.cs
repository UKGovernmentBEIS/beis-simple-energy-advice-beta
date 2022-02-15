using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class PropertyTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of property do you have?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select property type")]
        public PropertyType? Answer { get; set; }
    }
}