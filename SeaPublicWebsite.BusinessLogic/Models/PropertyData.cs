﻿using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Models;

public class PropertyData
{
    //PRIMARY KEY
    public int PropertyDataId { get; set; }
    
    public string Reference { get; set; }

    public OwnershipStatus? OwnershipStatus { get; set; }
    public Country? Country { get; set; }
    
    public FindEpc? FindEpc { get; set; }
    
    public int? EpcCount { get; set; }

    public EpcAddressConfirmed? EpcAddressConfirmed { get; set; }

    public EpcDetailsConfirmed? EpcDetailsConfirmed { get; set; }

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
    public PropertyData UneditedData { get; set; }

    public List<PropertyRecommendation> PropertyRecommendations { get; set; }

    public void CreateUneditedData()
    {
        UneditedData = new PropertyData();
        CopyAnswersTo(UneditedData);
    }
    
    public void RevertToUneditedData()
    {
        UneditedData.CopyAnswersTo(this);
        DeleteUneditedData();
    }
    
    public void DeleteUneditedData()
    {
        UneditedData = null;
    }

    private void CopyAnswersTo(PropertyData other)
    {
        other.OwnershipStatus = OwnershipStatus;
        other.Country = Country;
        other.Epc = Epc;
        other.Postcode = Postcode;
        other.HouseNameOrNumber = HouseNameOrNumber;
        other.PropertyType = PropertyType;
        other.HouseType = HouseType;
        other.BungalowType = BungalowType;
        other.FlatType = FlatType;
        other.YearBuilt = YearBuilt;
        other.WallConstruction = WallConstruction;
        other.CavityWallsInsulated = CavityWallsInsulated;
        other.SolidWallsInsulated = SolidWallsInsulated;
        other.FloorConstruction = FloorConstruction;
        other.FloorInsulated = FloorInsulated;
        other.RoofConstruction = RoofConstruction;
        other.LoftSpace = LoftSpace;
        other.LoftAccess = LoftAccess;
        other.RoofInsulated = RoofInsulated;
        other.HasOutdoorSpace = HasOutdoorSpace;
        other.GlazingType = GlazingType;
        other.HeatingType = HeatingType;
        other.OtherHeatingType = OtherHeatingType;
        other.HasHotWaterCylinder = HasHotWaterCylinder;
        other.NumberOfOccupants = NumberOfOccupants;
        other.HeatingPattern = HeatingPattern;
        other.HoursOfHeatingMorning = HoursOfHeatingMorning;
        other.HoursOfHeatingEvening = HoursOfHeatingEvening;
        other.Temperature = Temperature;
        other.PropertyRecommendations = PropertyRecommendations;
    }

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
