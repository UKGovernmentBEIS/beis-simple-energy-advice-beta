using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum OtherHeatingType
    {
        [Display(ResourceType = typeof(Resources.Enum.OtherHeatingType), Description = nameof(Resources.Enum.OtherHeatingType.Biomass))]
        Biomass,
        [Display(ResourceType = typeof(Resources.Enum.OtherHeatingType), Description = nameof(Resources.Enum.OtherHeatingType.CoalOrSolidFuel))]
        CoalOrSolidFuel,
        [Display(ResourceType = typeof(Resources.Enum.OtherHeatingType), Description = nameof(Resources.Enum.OtherHeatingType.Other))]
        Other,
    }
}
