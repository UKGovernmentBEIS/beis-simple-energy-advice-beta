using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class UserInterestsViewModel : GovUkViewModel
    {
        public string Title = "Are any of these measures of particular interest to you?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.UserNeeds;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select what you are interested in seeing")]
        public UserInterests? Answer { get; set; }
    }
} 