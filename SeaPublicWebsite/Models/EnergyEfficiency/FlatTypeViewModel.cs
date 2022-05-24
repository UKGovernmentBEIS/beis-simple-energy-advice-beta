﻿using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FlatTypeViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select flat type")]
        public FlatType? FlatType { get; set; }

        public string Reference { get; set; }
    }
}