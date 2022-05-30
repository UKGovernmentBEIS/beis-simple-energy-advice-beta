﻿using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HeatingPattern
    {
        [GovUkRadioCheckboxLabelText(Text = "All day and all night")]
        AllDayAndNight,
        [GovUkRadioCheckboxLabelText(Text = "All day and off at night")]
        AllDayNotNight,
        [GovUkRadioCheckboxLabelText (Text = "I'd like to input my hours")]
        Other
    }
}