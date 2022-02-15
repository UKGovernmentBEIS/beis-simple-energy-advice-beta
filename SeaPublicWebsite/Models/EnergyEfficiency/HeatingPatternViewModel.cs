using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel : GovUkViewModel
    {
        public string Title = "When would you normally heat your home on a typical weekday?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.Behaviour;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating pattern")]
        public HeatingPattern? Answer { get; set; }
    }
}