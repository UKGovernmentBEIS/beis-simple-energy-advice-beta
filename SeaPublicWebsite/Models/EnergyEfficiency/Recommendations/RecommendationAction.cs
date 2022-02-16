using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.Recommendations
{
    public enum RecommendationAction
    {
        [GovUkRadioCheckboxLabelText(Text = "Save this recommendation to my action plan")]
        SaveToAction,
        [GovUkRadioCheckboxLabelText(Text = "Discard this recommendation")]
        Discard,
        [GovUkRadioCheckboxLabelText(Text = "Decide later")]
        DecideLater
    }
}
