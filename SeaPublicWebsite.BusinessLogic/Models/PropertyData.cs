using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Models;

public class PropertyData
{
    //PRIMARY KEY
    public int PropertyDataId { get; set; }
    
    public string Reference { get; set; }

    public OwnershipStatus? OwnershipStatus { get; set; }
    public Country? Country { get; set; }

    public Epc Epc { get; set; }

    public string Postcode { get; set; }

    public string HouseNameOrNumber { get; set; }
    public PropertyType? PropertyType { get; set; }
    public HouseType? HouseType { get; set; }
    public BungalowType? BungalowType { get; set; }
    public FlatType? FlatType { get; set; }

    public YearBuilt? YearBuilt { get; set; }

    public WallConstruction? WallConstruction { get; set; }
    public CavityWallsInsulated? CavityWallsInsulated { get; set; }
    public SolidWallsInsulated? SolidWallsInsulated { get; set; }
    public FloorConstruction? FloorConstruction { get; set; }
    public FloorInsulated? FloorInsulated { get; set; }
    public RoofConstruction? RoofConstruction { get; set; }
    public LoftSpace? LoftSpace { get; set; }
    public LoftAccess? LoftAccess { get; set; }
    public RoofInsulated? RoofInsulated { get; set; }
    public HasOutdoorSpace? HasOutdoorSpace { get; set; }
    public GlazingType? GlazingType { get; set; }
    public HeatingType? HeatingType { get; set; }
    public OtherHeatingType? OtherHeatingType { get; set; }
    public HasHotWaterCylinder? HasHotWaterCylinder { get; set; }

    public int? NumberOfOccupants { get; set; }
    public HeatingPattern? HeatingPattern { get; set; }
    public int? HoursOfHeatingMorning { get; set; }
    public int? HoursOfHeatingEvening { get; set; }
    public decimal? Temperature { get; set; }

    public List<PropertyRecommendation> PropertyRecommendations { get; set; }

    public RecommendationKey GetFirstRecommendationKey()
    {
        return PropertyRecommendations[0].Key;
    }
    
    public RecommendationKey GetLastRecommendationKey()
    {
        return PropertyRecommendations[^1].Key;
    }
    
    public RecommendationKey GetNextRecommendationKey(RecommendationKey currentRecommendationKey)
    {
        var currentIndex = GetRecommendationIndex(currentRecommendationKey);
        return PropertyRecommendations[currentIndex + 1].Key;
    }
    
    public RecommendationKey GetPreviousRecommendationKey(RecommendationKey currentRecommendationKey)
    {
        var currentIndex = GetRecommendationIndex(currentRecommendationKey);
        return PropertyRecommendations[currentIndex - 1].Key;
    }

    public int GetRecommendationIndex(RecommendationKey currentRecommendationKey)
    {
        return PropertyRecommendations.FindIndex(r => r.Key == currentRecommendationKey);
    }
}