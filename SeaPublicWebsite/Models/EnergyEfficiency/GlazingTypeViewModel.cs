using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class GlazingTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of glazing does your property have?";
        public string Description = "Homes of your type have ";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select glazing type")]
        public GlazingType? Answer { get; set; }
    }
}