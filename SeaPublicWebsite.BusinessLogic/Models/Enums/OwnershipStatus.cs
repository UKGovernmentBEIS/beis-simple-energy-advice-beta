using System.ComponentModel.DataAnnotations;

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