using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OutdoorSpaceViewModel : GovUkViewModel
    {
        public string Title = "Do you have outdoor space?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.YourHome;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you have outdoor space")]
        public HasOutdoorSpace? Answer { get; set; }
    }
}