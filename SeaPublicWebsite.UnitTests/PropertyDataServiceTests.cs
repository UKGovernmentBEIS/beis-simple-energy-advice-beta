using System;
using System.Collections.Generic;
using System.Linq;
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
    private PropertyDataService underTest;

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
            PropertyRecommendations = [],
            EnergyPriceCapInfoRequested = false
        };
    }

    public PropertyDataServiceTests()
    {
        mockPropertyDataStore = new Mock<IPropertyDataStore>();
        mockRecommendationService = new Mock<IRecommendationService>();
        underTest = new PropertyDataService(mockPropertyDataStore.Object, mockRecommendationService.Object);
    }

    [Test]
    public async Task
        UpdatePropertyDataWithRecommendations_WhenCalledOnReferenceThatAlreadyHasTimestamp_ReturnsPropertyDataWithSameTimestamp()
    {
        // Arrange
        var testTime = new DateTime(2000, 1, 1);
        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = [],
                EnergyPriceCapInfo = null
            });
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(InitializePropertyDataWithRecommendationsFirstRetrievedAt(testTime));

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(testTime, Is.EqualTo(returnedPropertyData.RecommendationsFirstRetrievedAt));
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalled_SetsEnergyPriceCapInfoRequestedToTrue()
    {
        // Arrange
        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = [],
                EnergyPriceCapInfo = null
            });
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(InitializePropertyDataWithRecommendationsFirstRetrievedAt(null));

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.EnergyPriceCapInfoRequested, Is.True);
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledAndResponseHasEnergyPriceCapInfo_ParsesTheInfo()
    {
        // Arrange
        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = [],
                EnergyPriceCapInfo = new BreEnergyPriceCapInfo()
                {
                    Year = 2000,
                    MonthIndex = 1
                }
            });
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(InitializePropertyDataWithRecommendationsFirstRetrievedAt(null));

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.EnergyPriceCapYear, Is.EqualTo(2000));
        Assert.That(returnedPropertyData.EnergyPriceCapMonthIndex, Is.EqualTo(1));
    }

    [Test]
    public async Task
        UpdatePropertyDataWithRecommendations_WhenCalledAndResponseHasNoEnergyPriceCapInfo_SetsInfoToNull()
    {
        // Arrange
        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = [],
                EnergyPriceCapInfo = null
            });
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(InitializePropertyDataWithRecommendationsFirstRetrievedAt(null));

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.EnergyPriceCapYear, Is.Null);
        Assert.That(returnedPropertyData.EnergyPriceCapMonthIndex, Is.Null);
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledWithNoRecommendations_UsesRecommendationsFromApi()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.PropertyRecommendations[0].Key, Is.EqualTo(RecommendationKey.InstallHeatPump));
        Assert.That(returnedPropertyData.PropertyRecommendations[1].Key,
            Is.EqualTo(RecommendationKey.SolarElectricPanels));
        Assert.That(returnedPropertyData.PropertyRecommendations.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledWithRecommendations_OverwritesWithRecommendationsFromApi()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();
        var existingRecommendations = ExampleRecommendations.Skip(2).Take(1).ToList();

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        propertyData.PropertyRecommendations = existingRecommendations;
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.PropertyRecommendations[0].Key, Is.EqualTo(RecommendationKey.InstallHeatPump));
        Assert.That(returnedPropertyData.PropertyRecommendations[1].Key,
            Is.EqualTo(RecommendationKey.SolarElectricPanels));
        Assert.That(returnedPropertyData.PropertyRecommendations.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledWithRecommendationsWithRecommendationsThatHaveActions_SetsNewRecommendationsToTheseActionsAndOthersToNull()
    {
        var expectedRecommendations = ExampleRecommendations.Take(3).ToList();
        var existingRecommendations = ExampleRecommendations.Take(2).ToList();

        existingRecommendations[0].RecommendationAction = RecommendationAction.SaveToActionPlan;
        existingRecommendations[1].RecommendationAction = RecommendationAction.DecideLater;

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        propertyData.PropertyRecommendations = existingRecommendations;
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.PropertyRecommendations[0].RecommendationAction, Is.EqualTo(RecommendationAction.SaveToActionPlan));
        Assert.That(returnedPropertyData.PropertyRecommendations[1].RecommendationAction, Is.EqualTo(RecommendationAction.DecideLater));
        Assert.That(returnedPropertyData.PropertyRecommendations[2].RecommendationAction, Is.Null);
    }

    [Test]
    public async Task UpdatePropertyDataWithRecommendations_WhenCalledWithRecommendationsWithRecommendationsThatHaveActionsAndApiReturnsDifferentValues_SetsActionsToNull()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();
        var existingRecommendations = ExampleRecommendations.Take(2).ToList();

        existingRecommendations[0].RecommendationAction = RecommendationAction.SaveToActionPlan;
        existingRecommendations[1].RecommendationAction = RecommendationAction.DecideLater;
        existingRecommendations[1].MaxInstallCost = 1000;

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        propertyData.PropertyRecommendations = existingRecommendations;
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.PropertyRecommendations[0].RecommendationAction, Is.EqualTo(RecommendationAction.SaveToActionPlan));
        Assert.That(returnedPropertyData.PropertyRecommendations[1].RecommendationAction, Is.Null);
    }

    [Test]
    public async Task WhenCalledWithRecommendationsWithRecommendationsAndApiReturnsSameValues_SetsRecommendationsUpdatedSinceLastVisitToFalse()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();
        var existingRecommendations = ExampleRecommendations.Take(2).ToList();

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        propertyData.PropertyRecommendations = existingRecommendations;
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.RecommendationsUpdatedSinceLastVisit, Is.False);
    }

    [Test]
    public async Task WhenCalledWithRecommendationsWithRecommendationsAndApiReturnsDifferentValues_SetsRecommendationsUpdatedSinceLastVisitToTrue()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();
        var existingRecommendations = ExampleRecommendations.Take(2).ToList();

        existingRecommendations[1].MaxInstallCost = 1000;

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        propertyData.PropertyRecommendations = existingRecommendations;
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.RecommendationsUpdatedSinceLastVisit, Is.True);
    }

    [Test]
    public async Task WhenCalledWithRecommendationsWithNoRecommendations_SetsRecommendationsUpdatedSinceLastVisitToFalse()
    {
        var expectedRecommendations = ExampleRecommendations.Take(2).ToList();

        mockRecommendationService
            .Setup(rs => rs.GetRecommendationsWithPriceCapForPropertyAsync(It.IsAny<PropertyData>()))
            .ReturnsAsync(new BreRecommendationsWithPriceCap
            {
                Recommendations = ToBreRecommendationList(expectedRecommendations),
                EnergyPriceCapInfo = null
            });
        var propertyData = InitializePropertyDataWithRecommendationsFirstRetrievedAt(null);
        mockPropertyDataStore.Setup(ds => ds.LoadPropertyDataAsync("222222"))
            .ReturnsAsync(propertyData);

        // Act
        var returnedPropertyData = await underTest.UpdatePropertyDataWithRecommendations("222222");

        // Assert
        Assert.That(returnedPropertyData.RecommendationsUpdatedSinceLastVisit, Is.False);
    }

    private static IEnumerable<PropertyRecommendation> ExampleRecommendations => new List<PropertyRecommendation>
    {
        new()
        {
            Key = RecommendationKey.InstallHeatPump,
            Title = "Install Heat Pump",
            MinInstallCost = 10000,
            MaxInstallCost = 15000,
            Saving = 500,
            LifetimeSaving = 5000,
            Lifetime = 10,
            Summary = "Install a heat pump to improve energy efficiency."
        },
        new()
        {
            Key = RecommendationKey.SolarElectricPanels,
            Title = "Install Solar Panels",
            MinInstallCost = 5000,
            MaxInstallCost = 8000,
            Saving = 300,
            LifetimeSaving = 3000,
            Lifetime = 10,
            Summary = "Install solar panels to generate renewable energy."
        },
        new()
        {
            Key = RecommendationKey.InsulateYourLoft,
            Title = "Insulate Loft",
            MinInstallCost = 500,
            MaxInstallCost = 1000,
            Saving = 200,
            LifetimeSaving = 2000,
            Lifetime = 10,
            Summary = "Insulate your loft to reduce heat loss."
        }
    };

    private static List<BreRecommendation> ToBreRecommendationList(List<PropertyRecommendation> recommendations)
    {
        return recommendations.Select(recommendation => new BreRecommendation
        {
            Key = recommendation.Key,
            Title = recommendation.Title,
            MinInstallCost = recommendation.MinInstallCost,
            MaxInstallCost = recommendation.MaxInstallCost,
            Saving = recommendation.Saving,
            LifetimeSaving = recommendation.LifetimeSaving,
            Lifetime = recommendation.Lifetime,
            Summary = recommendation.Summary
        }).ToList();
    }
}