using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HotWaterCylinderViewModel : GovUkViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you have a hot water cylinder")]
        public HasHotWaterCylinder? HasHotWaterCylinder { get; set; }

        public string Reference { get; set; }
    }
}