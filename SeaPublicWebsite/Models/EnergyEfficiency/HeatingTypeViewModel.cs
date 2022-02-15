using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of heating does your property have?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.Heating;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating type")]
        public HeatingType? Answer { get; set; }
    }
}