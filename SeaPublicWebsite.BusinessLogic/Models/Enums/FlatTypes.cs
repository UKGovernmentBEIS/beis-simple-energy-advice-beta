using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum FlatType
    {
        [Display(ResourceType = typeof(Resources.Enum.FlatTypes), Description = nameof(Resources.Enum.FlatTypes.TopFloor))]
        TopFloor,
        [Display(ResourceType = typeof(Resources.Enum.FlatTypes), Description = nameof(Resources.Enum.FlatTypes.MiddleFloor))]
        MiddleFloor,
        [Display(ResourceType = typeof(Resources.Enum.FlatTypes), Description = nameof(Resources.Enum.FlatTypes.GroundFloor))]
        GroundFloor
    }
}