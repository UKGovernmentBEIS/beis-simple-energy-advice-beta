using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.Recommendations
{
    public enum RecommendationAction
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, save this recommendation to my action plan")]
        SaveToActionPlan,
        [GovUkRadioCheckboxLabelText(Text = "Maybe, but I’d like more information")]
        DecideLater,
        [GovUkRadioCheckboxLabelText(Text = "No, discard this recommendation")]
        Discard,
    }
}
