using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class CountryViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select which country the property is located in")]
        public Country? Country { get; set; }
        
        public string Reference { get; set; }
        public PageName? Change { get; set; }
        
        public string BackLink { get; set; }
    }
}
