﻿using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum OtherHeatingType
    {
        [GovUkRadioCheckboxLabelText(Text = "Biomass boiler")]
        Biomass,
        [GovUkRadioCheckboxLabelText(Text = "Coal or solid fuel heating")]
        CoalOrSolidFuel,
        [GovUkRadioCheckboxLabelText(Text = "Other")]
        Other,
    }
}
