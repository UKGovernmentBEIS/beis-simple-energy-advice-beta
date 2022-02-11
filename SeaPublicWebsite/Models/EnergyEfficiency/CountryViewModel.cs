using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class CountryViewModel : GovUkViewModel
    {
        public string Title = "Which country is your property located in?";
        public string Description = "What does that mean?";
        public QuestionTheme Theme = QuestionTheme.Suitability;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select country")]
        public Country? Answer { get; set; }
    }
}