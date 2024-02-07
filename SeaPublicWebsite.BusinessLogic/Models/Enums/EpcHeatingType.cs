using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum EpcHeatingType
{
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.GasBoiler))]
    GasBoiler,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.OilBoiler))]
    OilBoiler,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.LpgBoiler))]
    LpgBoiler,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.Storage))]
    Storage,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.DirectActionElectric))]
    DirectActionElectric,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.HeatPump))]
    HeatPump,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.Other))]
    Other,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.DoNotKnow))]
    DoNotKnow,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.Biomass))]
    Biomass,
    [Display(ResourceType = typeof(Resources.Enum.EpcHeatingType), Description = nameof(Resources.Enum.EpcHeatingType.CoalOrSolidFuel))]
    CoalOrSolidFuel,
}