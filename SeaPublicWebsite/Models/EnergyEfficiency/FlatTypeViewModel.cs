using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FlatTypeViewModel : GovUkViewModel
    {
        public string Title = "What kind of flat do you have?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select flat type")]
        public FlatType? Answer { get; set; }
    }
}