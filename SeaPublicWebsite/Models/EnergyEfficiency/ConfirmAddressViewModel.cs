using System.Collections.Generic;
using System.Security.Permissions;
using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel
    {
        public List<Epc> EPCList { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your address")]
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
        
        public string BackLink { get; set; }
    }
}