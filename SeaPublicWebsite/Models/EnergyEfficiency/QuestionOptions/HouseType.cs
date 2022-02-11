using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HouseType
    {
        [GovUkRadioCheckboxLabelText(Text = "Detatched")]
        Detatched,
        [GovUkRadioCheckboxLabelText(Text = "Semi-detatched")]
        SemiDetatched,
        [GovUkRadioCheckboxLabelText(Text = "Terraced")]
        Terraced,
        [GovUkRadioCheckboxLabelText(Text = "End terrace")]
        EndTerrace
    }
}