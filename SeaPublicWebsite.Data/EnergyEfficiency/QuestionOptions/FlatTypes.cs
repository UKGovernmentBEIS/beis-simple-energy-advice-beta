using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
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