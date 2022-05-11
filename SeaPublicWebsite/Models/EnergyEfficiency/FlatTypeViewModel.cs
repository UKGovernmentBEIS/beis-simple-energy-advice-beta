using SeaPublicWebsite.Helpers.UserFlow;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class FlatTypeViewModel
    {
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select flat type")]
        public FlatType? FlatType { get; set; }

        public string Reference { get; set; }
        public PageName? EntryPoint { get; set; }
        
        public string BackLink { get; set; }
    }
}