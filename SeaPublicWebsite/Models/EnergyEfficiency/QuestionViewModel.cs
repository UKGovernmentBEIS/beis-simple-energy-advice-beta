using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }

        public bool IsApplicable { get; set; }

        public bool IsAnswered { get; set; }
    }
}