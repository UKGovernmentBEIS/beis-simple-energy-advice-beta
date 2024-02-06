using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum FloorInsulated
    {
        [Display(ResourceType = typeof(Resources.Enum.FloorInsulated), Description = nameof(Resources.Enum.FloorInsulated.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.FloorInsulated), Description = nameof(Resources.Enum.FloorInsulated.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.FloorInsulated), Description = nameof(Resources.Enum.FloorInsulated.DoNotKnow))]
        DoNotKnow,
    }
}