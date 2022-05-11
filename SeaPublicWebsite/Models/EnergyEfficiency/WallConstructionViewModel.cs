using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallConstructionViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select wall type")]
        public WallConstruction? WallConstruction { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public PageName? Change { get; set; }
        public Epc Epc { get; set; }
        
        public string BackLink { get; set; }
    }
}