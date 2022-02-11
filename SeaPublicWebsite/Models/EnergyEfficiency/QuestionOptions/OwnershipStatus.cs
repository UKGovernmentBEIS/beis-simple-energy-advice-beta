using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum OwnershipStatus
    {
        [GovUkRadioCheckboxLabelText(Text = "I own my own home")]
        OwnerOccupancy,
        [GovUkRadioCheckboxLabelText(Text = "I rent my home")]
        PrivateTenancy
    }
}         