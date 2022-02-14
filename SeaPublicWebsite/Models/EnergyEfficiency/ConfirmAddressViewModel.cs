using GovUkDesignSystem;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : GovUkViewModel
    {
        public ConfirmAddressViewModel(Address address)
        {
            Address = address;
        }

        public QuestionTheme Theme = QuestionTheme.YourHome;

        public Address Address { get; set; }
    }
}