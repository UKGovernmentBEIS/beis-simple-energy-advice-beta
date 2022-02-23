using System.Collections.Generic;
using System.Security.Permissions;
using GovUkDesignSystem;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : GovUkViewModel
    {
        public List<Epc> EPCList { get; set; }
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
    }
}