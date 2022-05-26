using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class UserGoalViewModel
    {
        public string Title = "Which of these is the most important thing for you to achieve?";
        public string Description = "";

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select what is most important to you")]
        public UserGoal? Answer { get; set; }
    }
}