using FluentAssertions;
using NUnit.Framework;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Helpers;

namespace Tests;

public class ResetUnusedFieldsTests
{
    
    [TestCaseSource(nameof(TestCases))]
    public void RunResetUnusedFieldTestCases(ResetUnusedFieldsTestCase testCase)
    {
        // Arrange
        PropertyData propertyData = testCase.Input;
        
        // Act
        PropertyDataHelper.ResetUnusedFields(propertyData);

        // Assert
        propertyData.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }

    private static ResetUnusedFieldsTestCase[] TestCases =
    {
        new(
            "Setting property type to house resets bungalow type and flat type",
            new()
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.Detached,
                BungalowType = BungalowType.Terraced,
                FlatType = FlatType.MiddleFloor
            },
            new()
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.Detached
            }
        ),
        new(
            "Setting property type to bungalow resets house type and flat type",
            new()
            {
                PropertyType = PropertyType.Bungalow,
                HouseType = HouseType.Detached,
                BungalowType = BungalowType.Terraced,
                FlatType = FlatType.MiddleFloor
            },
            new()
            {
                PropertyType = PropertyType.Bungalow,
                BungalowType = BungalowType.Terraced
            }
        ),
        new(
            "Setting property type to flat resets house type and bungalow type",
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                HouseType = HouseType.Detached,
                BungalowType = BungalowType.Terraced,
                FlatType = FlatType.MiddleFloor
            },
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.MiddleFloor
            }
        ),
        new(
            "Setting wall construction to solid resets cavity insulation",
            new()
            {
                WallConstruction = WallConstruction.Solid,
                SolidWallsInsulated = SolidWallsInsulated.Some,
                CavityWallsInsulated = CavityWallsInsulated.Some
            },
            new()
            {
                WallConstruction = WallConstruction.Solid,
                SolidWallsInsulated = SolidWallsInsulated.Some,
            }
        ),
        new(
            "Setting wall construction to cavity resets solid insulation",
            new()
            {
                WallConstruction = WallConstruction.Cavity,
                SolidWallsInsulated = SolidWallsInsulated.Some,
                CavityWallsInsulated = CavityWallsInsulated.Some
            },
            new()
            {
                WallConstruction = WallConstruction.Cavity,
                CavityWallsInsulated = CavityWallsInsulated.Some
            }
        ),
        new(
            "Setting property to a non-ground apartment resets floor fields",
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.TopFloor,
                FloorConstruction = FloorConstruction.Mix,
                FloorInsulated = FloorInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.TopFloor,
            }
        ),
        new(
            "Setting property to a non-roof apartment resets roof fields",
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.GroundFloor,
                RoofConstruction = RoofConstruction.Mixed,
                AccessibleLoftSpace = AccessibleLoftSpace.Yes,
                RoofInsulated = RoofInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.GroundFloor,
            }
        ),
        new(
            "Setting property to a middle apartment resets floor and roof fields",
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.MiddleFloor,
                FloorConstruction = FloorConstruction.Mix,
                FloorInsulated = FloorInsulated.Yes,
                RoofConstruction = RoofConstruction.Mixed,
                AccessibleLoftSpace = AccessibleLoftSpace.Yes,
                RoofInsulated = RoofInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.MiddleFloor,
            }
        ),
        new(
            "Setting floor construction to an unknown type resets floor insulation",
            new()
            {
                PropertyType = PropertyType.House,
                FloorConstruction = FloorConstruction.Other,
                FloorInsulated = FloorInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.House,
                FloorConstruction = FloorConstruction.Other,
            }
        ),
        new(
            "Setting roof construction to flat type resets accessible loft space and roof insulation",
            new()
            {
                PropertyType = PropertyType.House,
                RoofConstruction = RoofConstruction.Flat,
                AccessibleLoftSpace = AccessibleLoftSpace.Yes,
                RoofInsulated = RoofInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.House,
                RoofConstruction = RoofConstruction.Flat,
            }
        ),
        new(
            "Setting accessible loft space to not be 'Yes' resets roof insulation",
            new()
            {
                PropertyType = PropertyType.House,
                RoofConstruction = RoofConstruction.Mixed,
                AccessibleLoftSpace = AccessibleLoftSpace.DoNotKnow,
                RoofInsulated = RoofInsulated.Yes
            },
            new()
            {
                PropertyType = PropertyType.House,
                RoofConstruction = RoofConstruction.Mixed,
                AccessibleLoftSpace = AccessibleLoftSpace.DoNotKnow,
            }
        ),
        new(
            "Setting heating type not to 'Other' resets other heating type",
            new()
            {
                HeatingType = HeatingType.GasBoiler,
                OtherHeatingType = OtherHeatingType.Biomass,
                HasHotWaterCylinder = HasHotWaterCylinder.Yes
            },
            new()
            {
                HeatingType = HeatingType.GasBoiler,
                HasHotWaterCylinder = HasHotWaterCylinder.Yes
            }
        ),
        new(
            "Setting heating type not to Gas boiler, Oil boiler or Lpg boiler resets has hot water cylinder",
            new()
            {
                HeatingType = HeatingType.Storage,
                OtherHeatingType = OtherHeatingType.Biomass,
                HasHotWaterCylinder = HasHotWaterCylinder.Yes
            },
            new()
            {
                HeatingType = HeatingType.Storage,
            }
        ),
    };
    
    public class ResetUnusedFieldsTestCase
    {
        public readonly string Description;
        public readonly PropertyData Input;
        public readonly UserDataModel ExpectedOutput;

        public ResetUnusedFieldsTestCase(string description, PropertyData input, UserDataModel expectedOutput)
        {
            Description = description;
            Input = input;
            ExpectedOutput = expectedOutput;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}