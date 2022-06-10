using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.DataModels;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallConstructionViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select wall type")]
        public WallConstruction? WallConstruction { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public Epc Epc { get; set; }
    }
}