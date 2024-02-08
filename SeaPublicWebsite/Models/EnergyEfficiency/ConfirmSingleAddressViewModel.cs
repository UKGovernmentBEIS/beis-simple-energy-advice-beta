using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmSingleAddressViewModel : QuestionFlowViewModel
    {
        public EpcSearchResult EpcSearchResult { get; set; }
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.CertificateBelongsToAddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public EpcAddressConfirmed? EpcAddressConfirmed { get; set; }
        public string Postcode { get; set; }
        public string Number { get; set; }
        public string EpcId { get; set; }
    }
}