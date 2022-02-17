using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HomeAgeViewModel : GovUkViewModel
    {
        [GovUkDisplayNameForErrors(NameAtStartOfSentence = "The year your property was built", NameWithinSentence = "the year your property was built")]
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the approximate year that your property was built")]
        [GovUkValidateIntRange(Minimum = 1000, Maximum = 2022)]
        public int? YearBuilt { get; set; }

        public string Reference { get; set; }
        
        public PropertyType? PropertyType { get; set; }
        public bool Change { get; set; }
    }
}