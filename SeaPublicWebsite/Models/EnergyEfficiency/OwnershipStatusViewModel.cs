using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OwnershipStatusViewModel : GovUkViewModel
    {
        public string Title = "Which if the following statements best describes your circumstances";
        public string Description = "What does that mean?";
        public QuestionTheme Theme = QuestionTheme.Suitability;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Must select tenancy type")]
        public OwnershipStatus? Answer { get; set; }
    }
}