using System.ComponentModel.DataAnnotations;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Models;

public class PropertyRecommendation
{
    //PRIMARY KEY
    public int PropertyRecommendationId { get; set; }
    public RecommendationKey Key { get; set; }
    public int MinInstallCost { get; set; }
    public int MaxInstallCost { get; set; }
    public int Saving { get; set; }
    public int LifetimeSaving { get; set; }
    public int Lifetime { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public RecommendationAction? RecommendationAction { get; set; }
    public int PropertyDataId { get; set; }
    public PropertyData PropertyData { get; set; }
    [Timestamp]
    public byte[] Timestamp { get; set; }
}