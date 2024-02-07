using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum BungalowType
    {
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.Detached))]
        Detached,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.SemiDetached))]
        SemiDetached,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.Terraced))]
        Terraced,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.EndTerrace))]
        EndTerrace
    }
}