﻿using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class NumberOfOccupantsViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the number of people who live in the property")]
        [GovUkValidateIntRange("Number of occupants", 1, 9)]
        public int? NumberOfOccupants { get; set; }

        public string Reference { get; set; }
        public PageName? Change { get; set; }
        
        public string BackLink { get; set; }
    }
}