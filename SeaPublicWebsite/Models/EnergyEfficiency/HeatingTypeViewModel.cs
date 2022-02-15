using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingTypeViewModel : GovUkViewModel
    {
        public string Title = "What is your main heating system?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.Heating;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating system")]
        public HeatingType? Answer { get; set; }
    }
}