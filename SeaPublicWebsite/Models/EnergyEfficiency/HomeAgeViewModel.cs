using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HomeAgeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the approximate year that your property was built")]
        [GovUkValidateIntRange("The year your property was build", 1000, 2022)]
        public int? YearBuilt { get; set; }

        public string Reference { get; set; }
        
        public PropertyType? PropertyType { get; set; }
        public bool Change { get; set; }
    }
}