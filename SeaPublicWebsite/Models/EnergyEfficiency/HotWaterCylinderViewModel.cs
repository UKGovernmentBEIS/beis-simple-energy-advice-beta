using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HotWaterCylinderViewModel : GovUkViewModel
    {
        public string Title = "Do you have a hot water cylinder?";
        public string Description = "";
        public QuestionSection Section = QuestionSection.Heating;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you have a hot water cylinder")]
        public HasHotWaterCylinder? Answer { get; set; }
    }
}