using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum SolarElectricPanels
    {
        [Display(ResourceType = typeof(Resources.Enum.SolarElectricPanels), Description = nameof(Resources.Enum.SolarElectricPanels.Yes))]
        Yes,
        [Display(ResourceType = typeof(Resources.Enum.SolarElectricPanels), Description = nameof(Resources.Enum.SolarElectricPanels.No))]
        No,
        [Display(ResourceType = typeof(Resources.Enum.SolarElectricPanels), Description = nameof(Resources.Enum.SolarElectricPanels.DoNotKnow))]
        DoNotKnow
    }
}