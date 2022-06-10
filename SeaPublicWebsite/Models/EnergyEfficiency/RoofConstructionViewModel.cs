using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RoofConstructionViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select roof type")]
        public RoofConstruction? RoofConstruction { get; set; }

        public string Reference { get; set; }
        public PropertyType? PropertyType { get; set; }
        public FlatType? FlatType { get; set; }
        public Epc Epc { get; set; }
    }
}