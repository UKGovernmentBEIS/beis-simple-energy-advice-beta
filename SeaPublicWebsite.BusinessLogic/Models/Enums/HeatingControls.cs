using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum HeatingControls
{
    [Display(ResourceType = typeof(Resources.Enum.HeatingControls),
        Description = nameof(Resources.Enum.HeatingControls.Programmer))]
    Programmer,

    [Display(ResourceType = typeof(Resources.Enum.HeatingControls),
        Description = nameof(Resources.Enum.HeatingControls.RoomThermostat))]
    RoomThermostats,

    [Display(ResourceType = typeof(Resources.Enum.HeatingControls),
        Description = nameof(Resources.Enum.HeatingControls.ThermostaticRadiatorValves))]
    ThermostaticRadiatorValves,

    [Display(ResourceType = typeof(Resources.Enum.HeatingControls),
        Description = nameof(Resources.Enum.HeatingControls.None))]
    None,

    [Display(ResourceType = typeof(Resources.Enum.HeatingControls),
        Description = nameof(Resources.Enum.HeatingControls.DoNotKnow))]
    DoNotKnow
}