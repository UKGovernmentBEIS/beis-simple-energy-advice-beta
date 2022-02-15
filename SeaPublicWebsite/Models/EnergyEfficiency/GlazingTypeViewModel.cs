using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class GlazingTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of glazing does your property have?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select glazing type")]
        public GlazingType? Answer { get; set; }
    }
}