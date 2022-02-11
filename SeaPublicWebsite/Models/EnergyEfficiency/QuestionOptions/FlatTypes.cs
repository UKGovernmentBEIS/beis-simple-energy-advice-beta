using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum FlatType
    {
        [GovUkRadioCheckboxLabelText(Text = "Top floor")]
        TopFloor,
        [GovUkRadioCheckboxLabelText(Text = "Middle floor")]
        MiddleFloor,
        [GovUkRadioCheckboxLabelText(Text = "Ground floor")]
        GroundFloor,
    }
}