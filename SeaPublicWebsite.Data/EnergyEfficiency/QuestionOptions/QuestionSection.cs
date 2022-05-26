using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum QuestionSection
    {
        [Display(Name = "Section 1: Is this service right for you?")]
        Suitability,
        [Display(Name = "Section 2: Your property")]
        YourHome,
        [Display(Name = "Section 3: Your heating system")]
        Heating,
        [Display(Name = "Section 4: Your energy usage")]
        Behaviour
    }
}