﻿using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select wall type")]
        public WallType? WallType { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public PageName? EntryPoint { get; set; }
    }
}