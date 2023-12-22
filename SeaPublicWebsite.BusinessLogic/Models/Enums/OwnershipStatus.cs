using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum OwnershipStatus
    {
        [Display(ResourceType = typeof(Resources.Enum.OwnershipStatus), Description = nameof(Resources.Enum.OwnershipStatus.OwnerOccupancy))]
        OwnerOccupancy,
        [Display(ResourceType = typeof(Resources.Enum.OwnershipStatus), Description = nameof(Resources.Enum.OwnershipStatus.PrivateTenancy))]
        PrivateTenancy,
        [Display(ResourceType = typeof(Resources.Enum.OwnershipStatus), Description = nameof(Resources.Enum.OwnershipStatus.Landlord))]
        Landlord,
    }
}         