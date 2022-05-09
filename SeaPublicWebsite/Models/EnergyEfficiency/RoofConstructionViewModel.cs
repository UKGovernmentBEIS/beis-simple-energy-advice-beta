using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofConstructionViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select roof type")]
        public RoofConstruction? RoofConstruction { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
        public PropertyType? PropertyType { get; set; }
        public FlatType? FlatType { get; set; }
        
        public string BackLink { get; set; }
    }
}