using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum FloorConstruction
    {
        [Display(ResourceType = typeof(Resources.Enum.FloorConstruction), Description = nameof(Resources.Enum.FloorConstruction.SuspendedTimber))]
        SuspendedTimber,
        [Display(ResourceType = typeof(Resources.Enum.FloorConstruction), Description = nameof(Resources.Enum.FloorConstruction.SolidConcrete))]
        SolidConcrete,
        [Display(ResourceType = typeof(Resources.Enum.FloorConstruction), Description = nameof(Resources.Enum.FloorConstruction.Mix))]
        Mix,
        [Display(ResourceType = typeof(Resources.Enum.FloorConstruction), Description = nameof(Resources.Enum.FloorConstruction.Other))]
        Other,
        [Display(ResourceType = typeof(Resources.Enum.FloorConstruction), Description = nameof(Resources.Enum.FloorConstruction.DoNotKnow))]
        DoNotKnow,
    }
    
}
