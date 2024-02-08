using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmEpcDetailsViewModel : QuestionFlowViewModel
    {
        public string SelectedEpcId { get; set; }
        
        public Epc Epc { get; set; }
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.CertificateBelongsToAddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public EpcDetailsConfirmed? EpcDetailsConfirmed { get; set; }
    }
}