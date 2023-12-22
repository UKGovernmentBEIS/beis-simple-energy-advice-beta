using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HeatingPattern
    {
        [Display(ResourceType = typeof(Resources.Enum.HeatingPattern), Description = nameof(Resources.Enum.HeatingPattern.AllDayAndNight))]
        AllDayAndNight,
        [Display(ResourceType = typeof(Resources.Enum.HeatingPattern), Description = nameof(Resources.Enum.HeatingPattern.AllDayNotNight))]
        AllDayNotNight,
        [Display(ResourceType = typeof(Resources.Enum.HeatingPattern), Description = nameof(Resources.Enum.HeatingPattern.Other))]
        Other
    }
}