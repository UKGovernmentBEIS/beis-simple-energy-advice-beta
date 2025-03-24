using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.Services.EnergyEfficiency;
using Moq;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;

namespace Tests;

[TestFixture]
public class PropertyDataServiceTests
{
    private Mock<IPropertyDataStore> mockPropertyDataStore;
    private Mock<IRecommendationService> mockRecommendationService;
    private PropertyDataService underTest ;
    
    private PropertyData InitializePropertyDataWithRecommendationsFirstRetrievedAt(DateTime? dateTime)
    {
        return new PropertyData
        {
            Reference = "ABCDEFGH",
            OwnershipStatus = OwnershipStatus.Landlord,
            Country = Country.England,
            Epc = new Epc(),
            SearchForEpc = SearchForEpc.Yes,
            EpcDetailsConfirmed = EpcDetailsConfirmed.Yes,
            PropertyType = PropertyType.Bungalow,
            HouseType = HouseType.Detached,
            BungalowType = BungalowType.Detached,
            FlatType = FlatType.GroundFloor,
            YearBuilt = YearBuilt.Pre1900,
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
            RecommendationsFirstRetrievedAt = dateTime,
            PropertyRecommendations = null
        };
    }
    
    public PropertyDataServiceTests()
    {
        mockPropertyDataStore = new Mock<IPropertyDataStore>();
        mockRecommendationService = new Mock<IRecommendationService>();
        underTest = new PropertyDataService(mockPropertyDataStore.Object, mockRecommendationService.Object);
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledOnReferenceThatAlreadyHasTimestamp_ReturnsPropertyDataWithSameTimestamp()
    {
        // Arrange
        var testTime = new DateTime(2000, 1, 1);
        mockRecommendationService.Setup(rs => rs.GetRecommendationsForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new List<BreRecommendation>());
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(InitializePropertyDataWithRecommendationsFirstRetrievedAt(testTime));
        
        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");
        
        // Assert
        Assert.That(testTime, Is.EqualTo(returnedPropertyData.RecommendationsFirstRetrievedAt));
    }
}