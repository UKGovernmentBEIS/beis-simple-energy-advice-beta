using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of walls does your property have?";
        public string Description = "Homes of your type have ";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select wall type")]
        public WallType? Answer { get; set; }
    }
}