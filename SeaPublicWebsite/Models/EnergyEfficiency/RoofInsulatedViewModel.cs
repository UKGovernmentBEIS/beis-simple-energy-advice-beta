using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofInsulatedViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if the roof is insulated")]
        public RoofInsulated? RoofInsulated { get; set; }

        public string Reference { get; set; }
        public PageName? EntryPoint { get; set; }
        public int? YearBuilt { get; set; }
        
        public string BackLink { get; set; }
    }
}