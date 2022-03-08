using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HouseType
    {
        [GovUkRadioCheckboxLabelText(Text = "Detached")]
        Detached,
        [GovUkRadioCheckboxLabelText(Text = "Semi-detached")]
        SemiDetached,
        [GovUkRadioCheckboxLabelText(Text = "Terraced")]
        Terraced,
        [GovUkRadioCheckboxLabelText(Text = "End terrace")]
        EndTerrace
    }
}