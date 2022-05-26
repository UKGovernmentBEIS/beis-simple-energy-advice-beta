using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FloorInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if the floor is insulated")]
        public FloorInsulated? FloorInsulated { get; set; }

        public string Reference { get; set; }
        public int? YearBuilt { get; set; }
        public Epc Epc { get; set; }
    }
}