using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallConstructionViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select wall type")]
        public WallConstruction? WallConstruction { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public bool Change { get; set; }
    }
}