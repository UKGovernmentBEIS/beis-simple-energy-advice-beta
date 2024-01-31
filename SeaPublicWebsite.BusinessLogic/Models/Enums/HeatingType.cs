using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HeatingType
    {
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.GasBoiler))]
        GasBoiler,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.OilBoiler))]
        OilBoiler,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.LpgBoiler))]
        LpgBoiler,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.Storage))]
        Storage,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.DirectActionElectric))]
        DirectActionElectric,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.HeatPump))]
        HeatPump,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.Other))]
        Other,
        [Display(ResourceType = typeof(Resources.Enum.HeatingType), Description = nameof(Resources.Enum.HeatingType.DoNotKnow))]
        DoNotKnow,
    }
}
