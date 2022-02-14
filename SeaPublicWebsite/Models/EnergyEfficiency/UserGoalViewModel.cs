using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class UserGoalViewModel : GovUkViewModel
    {
        public string Title = "Which of these is the most important thing for you to achieve?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.UserNeeds;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Please select what is most important to you")]
        public UserGoal? Answer { get; set; }
    }
}