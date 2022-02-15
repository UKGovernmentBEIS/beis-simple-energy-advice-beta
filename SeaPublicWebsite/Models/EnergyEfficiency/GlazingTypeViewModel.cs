using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class GlazingTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of windows do you have?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select glazing type")]
        public GlazingType? Answer { get; set; }
    }
}