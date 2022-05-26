﻿using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OwnershipStatusViewModel : QuestionFlowViewModel
    {
        public string Reference { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your circumstances")]
        public OwnershipStatus? OwnershipStatus { get; set; }
    }
}