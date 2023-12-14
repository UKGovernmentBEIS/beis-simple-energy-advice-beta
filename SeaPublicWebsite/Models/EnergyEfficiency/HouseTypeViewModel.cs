using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HouseTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.HouseTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public HouseType? HouseType { get; set; }

        public string Reference { get; set; }
    }
}