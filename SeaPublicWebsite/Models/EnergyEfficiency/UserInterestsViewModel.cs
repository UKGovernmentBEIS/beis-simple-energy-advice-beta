using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;
using System.Collections.Generic;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class UserInterestsViewModel
    {
        public string Title = "Did you have any particular improvements in mind you’d like to know more about?";
        public string Description = "";
        [GovUkValidateCheckboxNumberOfResponsesRange(ErrorMessageIfNothingSelected ="Select the improvements you'd like to know more about or select no")]
        public List<UserInterests> Answer { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether you have some improvements in mind")]
        public HasUserInterests? HasUserInterests { get; set; }
    }
} 