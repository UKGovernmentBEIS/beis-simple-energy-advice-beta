using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HasHotWaterCylinder
    {
        [Display(ResourceType = typeof(Resources.Enum.HasHotWaterCylinder), Description = nameof(Resources.Enum.HasHotWaterCylinder.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.HasHotWaterCylinder), Description = nameof(Resources.Enum.HasHotWaterCylinder.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.HasHotWaterCylinder), Description = nameof(Resources.Enum.HasHotWaterCylinder.DoNotKnow))]
        DoNotKnow,
    }
}