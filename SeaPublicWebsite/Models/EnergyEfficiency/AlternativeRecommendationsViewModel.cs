using System.Collections.Generic;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class AlternativeRecommendationsViewModel
{
    public string Reference { get; set; }
    public bool FromActionPlan { get; set; }
    public bool ShowAltRadiatorPanels { get; set; }
    public bool ShowAltHeatPump { get; set; }
    public bool ShowAltDraughtProofFloors { get; set; }
    public bool ShowAltDraughtProofWindowsAndDoors { get; set; }
    public bool ShowAltDraughtProofLoftAccess { get; set; }
    public int LastIndex { get; set; }
    public string BackLink { get; set; }
}