using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HouseType
    {
        [Display(ResourceType = typeof(Resources.Enum.HouseType), Description = nameof(Resources.Enum.HouseType.Detached))]
        Detached,
        [Display(ResourceType = typeof(Resources.Enum.HouseType), Description = nameof(Resources.Enum.HouseType.SemiDetached))]
        SemiDetached,
        [Display(ResourceType = typeof(Resources.Enum.HouseType), Description = nameof(Resources.Enum.HouseType.Terraced))]
        Terraced,
        [Display(ResourceType = typeof(Resources.Enum.HouseType), Description = nameof(Resources.Enum.HouseType.EndTerrace))]
        EndTerrace
    }
}