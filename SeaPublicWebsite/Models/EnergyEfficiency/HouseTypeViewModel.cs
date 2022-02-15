using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HouseTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of house do you have?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select house type")]
        public HouseType? Answer { get; set; }
    }
}