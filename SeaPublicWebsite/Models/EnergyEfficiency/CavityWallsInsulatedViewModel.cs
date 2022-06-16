using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class CavityWallsInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if your cavity walls are insulated")]
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }

        public string Reference { get; set; }
        
        public YearBuilt? YearBuilt { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public Epc Epc { get; set; }
    }
}