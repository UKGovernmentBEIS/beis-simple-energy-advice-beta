using System.Collections.Generic;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.DataModels
{
    public class UserDataModel
    {
        public string Reference { get; set; }
        
        public OwnershipStatus? OwnershipStatus { get; set; }
        public Country? Country { get; set; }
        
        public Epc Epc { get; set; }
        
        // public UserGoal? UserGoal { get; set; }
        // public List<UserInterests> UserInterests { get; set; }
        public string Postcode { get; set; }
        // public string EpcLmkKey { get; set; }
        
        public PropertyType? PropertyType { get; set; }
        public HouseType? HouseType { get; set; }
        public BungalowType? BungalowType { get; set; }
        public FlatType? FlatType { get; set; }
        
        public int? YearBuilt { get; set; }
        
        public WallConstruction? WallConstruction { get; set; }
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }
        public FloorConstruction? FloorConstruction { get; set; }
        public FloorInsulated? FloorInsulated { get; set; }
        public RoofConstruction? RoofConstruction { get; set; }
        public AccessibleLoftSpace? AccessibleLoftSpace { get; set; }
        public RoofInsulated? RoofInsulated { get; set; }
        public HasOutdoorSpace? HasOutdoorSpace { get; set; }
        public GlazingType? GlazingType { get; set; }
        public HeatingType? HeatingType { get; set; }
        public OtherHeatingType? OtherHeatingType { get; set; }
        public HasHotWaterCylinder? HasHotWaterCylinder { get; set; }
        
        public int? NumberOfOccupants { get; set; }
        public HeatingPattern? HeatingPattern { get; set; }
        public decimal? HoursOfHeating { get; set; }
        public decimal? Temperature { get; set; }
        
        public HasEmailAddress? HasEmailAddress { get; set; }
        public string EmailAddress { get; set; }

        public List<UserRecommendation> UserRecommendations { get; set; }
        
        public string BackLink { get; set; }
    }
}