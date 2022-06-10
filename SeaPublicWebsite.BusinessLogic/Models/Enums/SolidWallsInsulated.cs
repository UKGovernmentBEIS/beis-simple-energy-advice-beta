﻿using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum SolidWallsInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Yes, they are all insulated")]
        All,
        [GovUkRadioCheckboxLabelText(Text = "Some are insulated and some are not")]
        Some,
        [GovUkRadioCheckboxLabelText(Text = "No, they are not insulated")]
        No
    }
}