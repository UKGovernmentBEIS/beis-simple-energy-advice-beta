using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace Tests;

public class PropertyDataTests
{
    private PropertyData InitializePropertyData()
    {
        return new PropertyData
        {
            PropertyDataId = 0,
            Reference = "ABCDEFGH",
            OwnershipStatus = OwnershipStatus.Landlord,
            Country = Country.England,
            Epc = new Epc(),
            Postcode = "postcode",
            HouseNameOrNumber = "House Name",
            PropertyType = PropertyType.Bungalow,
            HouseType = HouseType.Detached,
            BungalowType = BungalowType.Detached,
            FlatType = FlatType.GroundFloor,
            YearBuilt = YearBuilt.Pre1930,
            WallConstruction = WallConstruction.Cavity,
            CavityWallsInsulated = CavityWallsInsulated.All,
            SolidWallsInsulated = SolidWallsInsulated.All,
            FloorConstruction = FloorConstruction.Mix,
            FloorInsulated = FloorInsulated.No,
            RoofConstruction = RoofConstruction.Flat,
            LoftSpace = LoftSpace.No,
            LoftAccess = LoftAccess.No,
            RoofInsulated = RoofInsulated.No,
            HasOutdoorSpace = HasOutdoorSpace.No,
            GlazingType = GlazingType.Both,
            HeatingType = HeatingType.Other,
            OtherHeatingType = OtherHeatingType.Biomass,
            HasHotWaterCylinder = HasHotWaterCylinder.No,
            NumberOfOccupants = 2,
            HeatingPattern = HeatingPattern.Other,
            HoursOfHeatingMorning = 2,
            HoursOfHeatingEvening = 2,
            Temperature = 20,
            UneditedData = new PropertyData(),
            HasSeenRecommendations = false,
            HasEditedData = true,
            PropertyRecommendations = new List<PropertyRecommendation>()
        };
    }

    [Test]
    public void CopiesAllAnswers()
    {
        // Arrange
        var propertyData = InitializePropertyData();
        
        // Act
        propertyData.CreateUneditedData();
        
        // Assert
        foreach (var propertyInfo in propertyData.GetType().GetProperties())
        {
            if (propertyInfo.Name.Equals(nameof(PropertyData.PropertyDataId)) ||
                propertyInfo.Name.Equals(nameof(PropertyData.UneditedData)) ||
                propertyInfo.Name.Equals(nameof(PropertyData.HasSeenRecommendations)) ||
                propertyInfo.Name.Equals(nameof(propertyData.HasEditedData)))
            {
                continue;
            }
            
            propertyInfo.GetValue(propertyData.UneditedData).Should().NotBeNull();
        }
    }
}