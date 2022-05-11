using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class PropertyTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select property type")]
        public PropertyType? PropertyType { get; set; }

        public string Reference { get; set; }
        public PageName? Change { get; set; }
        
        public string BackLink { get; set; }
    }
}