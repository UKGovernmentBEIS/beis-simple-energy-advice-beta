using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum PropertyType
    {
        [GovUkRadioCheckboxLabelText(Text = "House")]
        House,
        [GovUkRadioCheckboxLabelText(Text = "Bungalow")]
        Bungalow,
        [GovUkRadioCheckboxLabelText(Text = "Apartment, flat or maisonette")]
        ApartmentFlatOrMaisonette
    }
}