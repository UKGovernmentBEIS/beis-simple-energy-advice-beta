using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum SolidWallsInsulated
    {
        [Display(ResourceType = typeof(Resources.Enum.SolidWallsInsulated), Description = nameof(Resources.Enum.SolidWallsInsulated.All))]
        All,
        [Display(ResourceType = typeof(Resources.Enum.SolidWallsInsulated), Description = nameof(Resources.Enum.SolidWallsInsulated.Some))]
        Some,
        [Display(ResourceType = typeof(Resources.Enum.SolidWallsInsulated), Description = nameof(Resources.Enum.SolidWallsInsulated.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.SolidWallsInsulated), Description = nameof(Resources.Enum.SolidWallsInsulated.DoNotKnow))]
        DoNotKnow,
    }
}