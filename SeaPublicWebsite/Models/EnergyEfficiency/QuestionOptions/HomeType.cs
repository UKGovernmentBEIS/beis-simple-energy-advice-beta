using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HomeType
    {
        [GovUkRadioCheckboxLabelText(Text = "House")]
        House,
        [GovUkRadioCheckboxLabelText(Text = "Bungalow")]
        Bungalow,
        [GovUkRadioCheckboxLabelText(Text = "Flat, Duplex or Maisonette")]
        FlatDuplexOrMaisonette,
        [GovUkRadioCheckboxLabelText(Text = "Park or Mobile Home")]
        ParkHomeOrMobileHome
    }
}