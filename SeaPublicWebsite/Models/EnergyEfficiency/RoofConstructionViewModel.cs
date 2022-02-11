using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofConstructionViewModel : GovUkViewModel
    {
        public string Title = "What kind of roof does your property have?";
        public string Description = "Homes of your type have ";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select roof type")]
        public RoofConstruction? Answer { get; set; }
    }
}