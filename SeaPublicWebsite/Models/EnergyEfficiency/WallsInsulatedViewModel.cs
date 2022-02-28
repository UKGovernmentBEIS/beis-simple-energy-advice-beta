using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallsInsulatedViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if your walls are insulated")]
        public WallsInsulated? WallsInsulated { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public bool Change { get; set; }
        public WallConstruction? WallConstruction { get; set; }
    }
}