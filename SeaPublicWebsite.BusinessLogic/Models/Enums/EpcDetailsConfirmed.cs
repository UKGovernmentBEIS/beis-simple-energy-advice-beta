using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum EpcDetailsConfirmed
{
    [Display(ResourceType = typeof(Resources.Enum.EpcDetailsConfirmed), Description = nameof(Resources.Enum.EpcDetailsConfirmed.Yes))]
    Yes,
    [Display(ResourceType = typeof(Resources.Enum.EpcDetailsConfirmed), Description = nameof(Resources.Enum.EpcDetailsConfirmed.No))]
    No
}