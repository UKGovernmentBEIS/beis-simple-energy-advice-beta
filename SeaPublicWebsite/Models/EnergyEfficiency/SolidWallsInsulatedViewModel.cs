using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class SolidWallsInsulatedViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if your solid walls are insulated")]
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public PageName? EntryPoint { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public Epc Epc { get; set; }
        
        public string BackLink { get; set; }
    }
}