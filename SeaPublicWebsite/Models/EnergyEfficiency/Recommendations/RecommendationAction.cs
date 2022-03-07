﻿using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.Recommendations
{
    public enum RecommendationAction
    {
        [GovUkRadioCheckboxLabelText(Text = "Yes, save this recommendation to my action plan")]
        SaveToActionPlan,
        [GovUkRadioCheckboxLabelText(Text = "Maybe, but I’d like to decide later")]
        DecideLater,
        [GovUkRadioCheckboxLabelText(Text = "No, discard this recommendation")]
        Discard,
    }
}
