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

        public QuestionSection Section = QuestionSection.YourHome;

        public Address Address { get; set; }
    }
}