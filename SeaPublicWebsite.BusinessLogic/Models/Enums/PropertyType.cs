using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum PropertyType
    {
        [Display(ResourceType = typeof(Resources.Enum.PropertyType), Description = nameof(Resources.Enum.PropertyType.House))]
        House,
        [Display(ResourceType = typeof(Resources.Enum.PropertyType), Description = nameof(Resources.Enum.PropertyType.Bungalow))]
        Bungalow,
        [Display(ResourceType = typeof(Resources.Enum.PropertyType), Description = nameof(Resources.Enum.PropertyType.ApartmentFlatOrMaisonette))]
        ApartmentFlatOrMaisonette
    }
}