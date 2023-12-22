using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum LoftSpace
    {
        [Display(ResourceType = typeof(Resources.Enum.LoftSpace), Description = nameof(Resources.Enum.LoftSpace.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.LoftSpace), Description = nameof(Resources.Enum.LoftSpace.No))]
        No,
    }
}