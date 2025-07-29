using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Models;

public class PropertyRecommendation : IEntityWithRowVersioning
{
    public RecommendationKey Key { get; set; }
    public int MinInstallCost { get; set; }
    public int MaxInstallCost { get; set; }
    public int Saving { get; set; }
    public int LifetimeSaving { get; set; }
    public int Lifetime { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public RecommendationAction? RecommendationAction { get; set; }
    public uint Version { get; set; }

    public int PropertyDataId { get; set; }
    public PropertyData PropertyData { get; set; }

    public bool HasSameImpactAs(PropertyRecommendation other)
    {
        if (other == null) return false;

        return Key == other.Key &&
               MinInstallCost == other.MinInstallCost &&
               MaxInstallCost == other.MaxInstallCost &&
               Saving == other.Saving &&
               LifetimeSaving == other.LifetimeSaving &&
               Lifetime == other.Lifetime;
    }
}