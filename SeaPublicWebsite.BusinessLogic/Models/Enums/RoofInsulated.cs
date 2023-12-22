using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum RoofInsulated
    {
        [Display(ResourceType = typeof(Resources.Enum.RoofInsulated), Description = nameof(Resources.Enum.RoofInsulated.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.RoofInsulated), Description = nameof(Resources.Enum.RoofInsulated.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.RoofInsulated), Description = nameof(Resources.Enum.RoofInsulated.DoNotKnow))]
        DoNotKnow,
    }
}