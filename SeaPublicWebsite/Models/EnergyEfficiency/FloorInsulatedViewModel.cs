using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FloorInsulatedViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if the floor is insulated")]
        public FloorInsulated? FloorInsulated { get; set; }

        public string Reference { get; set; }
        public PageName? EntryPoint { get; set; }
        public int? YearBuilt { get; set; }
        public Epc Epc { get; set; }
        
        public string BackLink { get; set; }
    }
}