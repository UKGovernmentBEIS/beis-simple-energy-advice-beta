using System.Collections.Generic;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class AlternativeRecommendationsViewModel
{
    public string Reference { get; set; }
    public bool FromActionPlan { get; set; }
    public PropertyData PropertyData { get; set; }
    public string BackLink { get; set; }
}