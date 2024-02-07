using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum LoftAccess
    {
        [Display(ResourceType = typeof(Resources.Enum.LoftAccess), Description = nameof(Resources.Enum.LoftAccess.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.LoftAccess), Description = nameof(Resources.Enum.LoftAccess.No))]
        No,
    }
}