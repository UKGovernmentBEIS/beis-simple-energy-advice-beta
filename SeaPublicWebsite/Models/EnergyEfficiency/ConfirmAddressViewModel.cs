using System.Collections.Generic;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : QuestionFlowViewModel
    {
        public List<EpcInformation> EpcInformationList { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your address")]
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
    }
}