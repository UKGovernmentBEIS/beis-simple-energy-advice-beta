using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum Country
    {
        [Display(ResourceType = typeof(Resources.Enum.Country), Description = nameof(Resources.Enum.Country.England))]
        England,
        [Display(ResourceType = typeof(Resources.Enum.Country), Description = nameof(Resources.Enum.Country.Wales))]
        Wales,
        [Display(ResourceType = typeof(Resources.Enum.Country), Description = nameof(Resources.Enum.Country.Scotland))]
        Scotland,
        [Display(ResourceType = typeof(Resources.Enum.Country), Description = nameof(Resources.Enum.Country.NorthernIreland))]
        NorthernIreland,
        [Display(ResourceType = typeof(Resources.Enum.Country), Description = nameof(Resources.Enum.Country.Other))]
        Other
    }
}
