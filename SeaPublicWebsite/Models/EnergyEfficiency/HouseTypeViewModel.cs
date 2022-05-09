using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HouseTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select house type")]
        public HouseType? HouseType { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
        
        public string BackLink { get; set; }
    }
}