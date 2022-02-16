using GovUkDesignSystem;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : GovUkViewModel
    {
        public Address Address { get; set; }
        public string Reference { get; set; }
    }
}