using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum EpcAddressConfirmed
{
    [Display(ResourceType = typeof(Resources.Enum.EpcAddressConfirmed), Description = nameof(Resources.Enum.EpcAddressConfirmed.Yes))]
    Yes,
    [Display(ResourceType = typeof(Resources.Enum.EpcAddressConfirmed), Description = nameof(Resources.Enum.EpcAddressConfirmed.No))]
    No
}