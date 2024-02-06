using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class GlazingTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.GlazingTypeRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public GlazingType? GlazingType { get; set; }
        public bool? HintSingleGlazing { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}