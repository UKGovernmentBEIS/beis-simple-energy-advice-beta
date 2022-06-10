﻿using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FloorConstructionViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select how your floor is built")]
        public FloorConstruction? FloorConstruction { get; set; }

        public string Reference { get; set; }
        public int? YearBuilt { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public Epc Epc { get; set; }
    }
}