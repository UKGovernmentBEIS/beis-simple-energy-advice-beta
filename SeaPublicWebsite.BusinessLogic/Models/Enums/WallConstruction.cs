using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum WallConstruction
    {
        [Display(ResourceType = typeof(Resources.Enum.WallConstruction), Description = nameof(Resources.Enum.WallConstruction.Solid))]
        Solid,
        [Display(ResourceType = typeof(Resources.Enum.WallConstruction), Description = nameof(Resources.Enum.WallConstruction.Cavity))]
        Cavity,
        [Display(ResourceType = typeof(Resources.Enum.WallConstruction), Description = nameof(Resources.Enum.WallConstruction.Mixed))]
        Mixed,
        [Display(ResourceType = typeof(Resources.Enum.WallConstruction), Description = nameof(Resources.Enum.WallConstruction.Other))]
        Other,
        [Display(ResourceType = typeof(Resources.Enum.WallConstruction), Description = nameof(Resources.Enum.WallConstruction.DoNotKnow))]
        DoNotKnow,
    }
}