using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofInsulatedViewModel : GovUkViewModel
    {
        public string Title = "Is your roof insulated?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;
        
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if the roof is insulated")]
        public RoofInsulated? Answer { get; set; }
    }
}