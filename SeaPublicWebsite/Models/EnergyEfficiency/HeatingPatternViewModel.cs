using System;
using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HeatingPatternViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select heating pattern")]
        public HeatingPattern? HeatingPattern { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter number of hours")]
        [GovUkValidateDecimalRange("Heating hours", "0", "24")]
        [GovUkDisplayNameForErrors(NameAtStartOfSentence = "Number of hours")]
        public decimal? HoursOfHeating { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}