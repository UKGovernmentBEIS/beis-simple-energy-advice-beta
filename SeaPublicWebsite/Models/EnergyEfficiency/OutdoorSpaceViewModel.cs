using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OutdoorSpaceViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you have outdoor space")]
        public HasOutdoorSpace? HasOutdoorSpace { get; set; }

        public string Reference { get; set; }
    }
}