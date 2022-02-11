using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HouseTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of house do you have?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select house type")]
        public HouseType? Answer { get; set; }
    }
}