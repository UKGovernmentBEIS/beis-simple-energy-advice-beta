using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class OwnershipStatusViewModel : GovUkViewModel
    {
        public string Reference { get; set; }
        public string Title = "Which if the following statements best describes your circumstances";
        public string Description = "What does that mean?";
        public QuestionSection Section = QuestionSection.Suitability;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your circumstances")]
        public OwnershipStatus? Answer { get; set; }
    }
}