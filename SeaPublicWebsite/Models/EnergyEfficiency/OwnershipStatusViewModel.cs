using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OwnershipStatusViewModel : GovUkViewModel
    {
        public string Reference { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your circumstances")]
        public OwnershipStatus? OwnershipStatus { get; set; }

        public bool Change { get; set; }
    }
}