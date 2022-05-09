using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class GlazingTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select glazing type")]
        public GlazingType? GlazingType { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
        
        public string BackLink { get; set; }
    }
}