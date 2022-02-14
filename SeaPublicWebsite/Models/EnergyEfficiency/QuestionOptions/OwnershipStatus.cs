using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum OwnershipStatus
    {
        [GovUkRadioCheckboxLabelText(Text = "I own my own home and live in it")]
        OwnerOccupancy,
        [GovUkRadioCheckboxLabelText(Text = "I own my own home and have one or more tenants")]
        Landlord,
        [GovUkRadioCheckboxLabelText(Text = "I am a tenant")]
        PrivateTenancy
    }
}         