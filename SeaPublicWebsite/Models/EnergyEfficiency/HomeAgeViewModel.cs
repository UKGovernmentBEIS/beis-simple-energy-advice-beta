using GovUkDesignSystem.Attributes.DataBinding;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HomeAgeViewModel : QuestionFlowViewModel
    {
        [ModelBinder(typeof(GovUkMandatoryIntBinder))]
        [GovUkDataBindingMandatoryIntErrorText("Enter the approximate year that your property was built", "The year your property was built")]
        [GovUkValidateIntRange("The year your property was built", 1000, 2022)]
        public int? YearBuilt { get; set; }

        public string Reference { get; set; }
        
        public PropertyType? PropertyType { get; set; }
    }
}