using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HasOutdoorSpace
    {
        [Display(ResourceType = typeof(Resources.Enum.HasOutdoorSpace), Description = nameof(Resources.Enum.HasOutdoorSpace.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.HasOutdoorSpace), Description = nameof(Resources.Enum.HasOutdoorSpace.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.HasOutdoorSpace), Description = nameof(Resources.Enum.HasOutdoorSpace.DoNotKnow))]
        DoNotKnow,
    }
}