using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum CavityWallsInsulated
    {
        [Display(ResourceType = typeof(Resources.Enum.CavityWallsInsulated), Description = nameof(Resources.Enum.CavityWallsInsulated.All))]
        All,
        [Display(ResourceType = typeof(Resources.Enum.CavityWallsInsulated), Description = nameof(Resources.Enum.CavityWallsInsulated.Some))]
        Some,
        [Display(ResourceType = typeof(Resources.Enum.CavityWallsInsulated), Description = nameof(Resources.Enum.CavityWallsInsulated.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.CavityWallsInsulated), Description = nameof(Resources.Enum.CavityWallsInsulated.DoNotKnow))]
        DoNotKnow, 
    }
}