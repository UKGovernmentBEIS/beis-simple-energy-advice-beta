using GovUkDesignSystem;
using SeaPublicWebsite.ExternalServices;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : GovUkViewModel
    {
        public ConfirmAddressViewModel(Address address)
        {
            Address = address;
        }

        public Address Address { get; set; }
    }
}