using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum RoofConstruction
    { 
        [Display(ResourceType = typeof(Resources.Enum.RoofConstruction), Description = nameof(Resources.Enum.RoofConstruction.Flat))]
        Flat,
        [Display(ResourceType = typeof(Resources.Enum.RoofConstruction), Description = nameof(Resources.Enum.RoofConstruction.Mixed))]
        Mixed,
        [Display(ResourceType = typeof(Resources.Enum.RoofConstruction), Description = nameof(Resources.Enum.RoofConstruction.Pitched))]
        Pitched,
    }
}
