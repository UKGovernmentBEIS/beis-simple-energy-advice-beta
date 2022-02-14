using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using System.Collections.Generic;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class UserInterestsViewModel : GovUkViewModel
    {
        public string Title = "Did you have any particular improvements in mind you’d like to know more about?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.UserNeeds;

        public List<UserInterests> Answer { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please let us know if you have some improvements in mind")]
        public HasUserInterests? HasUserInterests { get; set; }
    }
} 