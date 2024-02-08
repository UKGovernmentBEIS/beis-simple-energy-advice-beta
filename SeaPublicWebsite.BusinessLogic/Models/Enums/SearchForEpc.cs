using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum SearchForEpc
{
    [Display(ResourceType = typeof(Resources.Enum.SearchForEpc), Description = nameof(Resources.Enum.SearchForEpc.Yes))]
    Yes,
    [Display(ResourceType = typeof(Resources.Enum.SearchForEpc), Description = nameof(Resources.Enum.SearchForEpc.No))]
    No,
}