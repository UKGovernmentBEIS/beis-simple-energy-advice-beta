using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class HomeAgeViewModel : GovUkViewModel
    {
        public string Title = "Roughly what year was your home built?";
        public string Description = "Enter the year your home was built, approximately. It does not have to be exact. e.g. 1950";
        public string HelpTitle = "Help me determine the year my home was built";
        public string HelpText = "Typically...";
        public QuestionTheme Theme = QuestionTheme.YourHome ;

        [GovUkDisplayNameForErrors(NameAtStartOfSentence = "Year that you home was built", NameWithinSentence = "year that your home was built")]
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the approximate year that your home was built")]
        [GovUkValidateIntRange(Minimum = 1000, Maximum = 2022)]
        public int? HomeAge { get; set; }
    }
}