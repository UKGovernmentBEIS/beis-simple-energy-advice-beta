using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

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