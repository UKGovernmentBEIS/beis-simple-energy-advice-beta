using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofConstructionViewModel : GovUkViewModel
    {
        public string Title = "Is your roof flat or pitched at an angle?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select roof type")]
        public RoofConstruction? Answer { get; set; }
    }
}