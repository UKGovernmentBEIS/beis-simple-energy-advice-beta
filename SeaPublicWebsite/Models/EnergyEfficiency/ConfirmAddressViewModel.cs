using System.Collections.Generic;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : QuestionFlowViewModel
    {
        public List<EpcInformation> EpcInformationList { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your address")]
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please confirm whether or not this certificate belongs to your address before continuing")]
        public AddressConfirmed? AddressConfirmed { get; set; }
    }
}