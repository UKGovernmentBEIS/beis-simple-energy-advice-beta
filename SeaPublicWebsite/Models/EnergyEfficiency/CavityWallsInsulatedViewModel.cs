﻿using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class CavityWallsInsulatedViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select if your cavity walls are insulated")]
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }

        public string Reference { get; set; }
        
        public int? YearBuilt { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public Epc Epc { get; set; }
    }
}