using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class AskForPostcodeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.BuildingNumberOrNameRequired))]
        public string HouseNameOrNumber { get; set; }
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.PostcodeRequired))]
        public string Postcode { get; set; }
        public string Reference { get; set; }
    }
}