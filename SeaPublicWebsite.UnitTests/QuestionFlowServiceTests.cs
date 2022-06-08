using FluentAssertions;
using NUnit.Framework;
using SeaPublicWebsite.Controllers;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Services;

namespace Tests;

[TestFixture]
public class QuestionFlowServiceTests
{
    [SetUp]
    public void Setup()
    {
        QuestionFlowService = new QuestionFlowService();
    }

    private IQuestionFlowService QuestionFlowService;

    [TestCaseSource(nameof(BackTestCases))]
    public void RunBackLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Act
        var output = QuestionFlowService.BackLinkArguments(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }

    [TestCaseSource(nameof(ForwardTestCases))]
    public void RunForwardLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Act
        var output = QuestionFlowService.ForwardLinkArguments(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }

    [TestCaseSource(nameof(SkipTestCases))]
    public void RunSkipLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Act
        var output = QuestionFlowService.SkipLinkArguments(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }

    private static QuestionFlowServiceTestCase[] BackTestCases =
    {
        new(
            "A new or returning user goes back to the Index page",
            new Input(
                QuestionFlowPage.NewOrReturningUser
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.Index),
                "EnergyEfficiency"
            )),
        new(
            "Country goes back to new or returning user",
            new Input(
                QuestionFlowPage.Country
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.NewOrReturningUser_Get),
                "EnergyEfficiency"
            )),
        new(
            "Changing country goes to Summary",
            new Input(
                QuestionFlowPage.Country,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.Country
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Ownership status goes back to Country",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.Country_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing ownership status goes back to summary",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OwnershipStatus
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Service unsuitable goes back to the country you came from",
            new Input(
                QuestionFlowPage.ServiceUnsuitable,
                "ABCDEFGH",
                country: Country.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.Country_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Service unsuitable goes back to ownership status if user is a private tenant",
            new Input(
                QuestionFlowPage.ServiceUnsuitable,
                "ABCDEFGH",
                country: Country.England,
                ownershipStatus: OwnershipStatus.PrivateTenancy
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OwnershipStatus_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Postcode goes back to ownership status",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OwnershipStatus_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Confirm address goes back to postcode",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AskForPostcode_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),

        new(
            "Property type goes back to Postcode",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AskForPostcode_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Change property type goes back to summary",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "House type goes back to property type and preserves entry point",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HouseType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.PropertyType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH", entryPoint = QuestionFlowPage.HouseType }
            )),
        new(
            "Bungalow type goes back to property type and preserves entry point",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.BungalowType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.PropertyType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH", entryPoint = QuestionFlowPage.BungalowType }
            )),
        new(
            "Flat type goes back to property type and preserves entry point",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FlatType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.PropertyType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH", entryPoint = QuestionFlowPage.FlatType }
            )),
        new(
            "Home age goes back to the property type it came from (house)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HouseType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Home age goes back to the property type it came from (bungalow)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.Bungalow
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.BungalowType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Home age goes back to the property type it came from (flat)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FlatType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing home age goes back to summary",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HomeAge
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Wall construction goes back to home age",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HomeAge_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing wall construction goes back to summary",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.WallConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Cavity walls insulated goes back to wall construction",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing cavity walls insulated goes back to summary",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.CavityWallsInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Solid walls insulated goes back to cavity walls insulated if user has mixed walls",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Mixed
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.CavityWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Solid walls insulated goes back to wall construction if user does not have mixed walls",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing solid walls insulated goes back to summary",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.SolidWallsInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction goes back to the wall insulation the user answered last",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction goes back to the wall insulation the user answered last",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Cavity
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.CavityWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction goes back to wall construction if user has neither cavity not solid walls",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing floor construction goes back to summary",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor insulated goes back to floor construction",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing floor insulated goes back to summary",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction goes back to floor insulation if the user has timber or concrete floors",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.SuspendedTimber,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction goes back to floor construction if the user has different floors",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.Other,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction goes back to wall insulation if user was not asked about their floor but has insulated walls",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Solid,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction goes back to wall construction if user was not asked about their floor and has other walls",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.DoNotKnow,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing roof construction goes back to summary",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Accessible loft space goes back to roof construction",
            new Input(
                QuestionFlowPage.AccessibleLoftSpace,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing accessible loft space goes back to summary",
            new Input(
                QuestionFlowPage.AccessibleLoftSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.AccessibleLoftSpace
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof insulated goes back to accessible loft space",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing roof insulated goes back to summary",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type goes back to roof construction if the user has flat roof",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Flat
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type goes back to accessible loft space if the user does not have flat roof nor accessible loft space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Pitched,
                accessibleLoftSpace: AccessibleLoftSpace.No
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type goes back to roof insulation if the user does not have flat roof but has accessible loft space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Pitched,
                accessibleLoftSpace: AccessibleLoftSpace.Yes
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type goes back to floor construction if the user was not asked about their roof",
            new Input(
                QuestionFlowPage.GlazingType,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.GroundFloor,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type goes back to wall construction if the user was not asked about neither their roof or floor",
            new Input(
                QuestionFlowPage.GlazingType,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                reference: "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing glazing type goes back to summary",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.GlazingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Outdoor space goes back to glazing type",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing outdoor space goes back to summary",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OutdoorSpace
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating type goes back to outdoor space",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OutdoorSpace_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing heating type goes back to summary",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Other heating type goes back to outdoor space",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing other heating type goes back to summary",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OtherHeatingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Hot water cylinder goes back to heating type",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing hot water cylinder goes back to summary",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HotWaterCylinder
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Number of occupants goes back to hot water cylinder if user has boiler",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.GasBoiler
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HotWaterCylinder_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Number of occupants goes back to other heating type if the user selected so",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OtherHeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Number of occupants goes back to heating type if the user does not have a boiler",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing number of occupants goes back to summary",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.NumberOfOccupants
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating pattern goes back to number of occupants",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing heating pattern goes back to summary",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingPattern
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Temperature goes back to heating pattern",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingPattern_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing temperature goes back to summary",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.Temperature
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Answer summary goes back to temperature",
            new Input(
                QuestionFlowPage.AnswerSummary,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.Temperature_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Your recommendations goes back to summary",
            new Input(
                QuestionFlowPage.YourRecommendations,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            ))
    };

    private static QuestionFlowServiceTestCase[] ForwardTestCases =
    {
        new(
            "Country continues to service unsuitable if the service is not available",
            new Input(
                QuestionFlowPage.Country,
                "ABCDEFGH",
                country: Country.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.ServiceUnsuitable),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Country continues to ownership status",
            new Input(
                QuestionFlowPage.Country,
                "ABCDEFGH",
                country: Country.England
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OwnershipStatus_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing country continues to answer summary",
            new Input(
                QuestionFlowPage.Country,
                "ABCDEFGH",
                country: Country.England,
                entryPoint: QuestionFlowPage.Country
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Ownership status continue to service unsuitable if user is a private tenant",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                OwnershipStatus.PrivateTenancy
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.ServiceUnsuitable),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Ownership status continue to postcode",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                OwnershipStatus.OwnerOccupancy
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AskForPostcode_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing ownership status continue to summary",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OwnershipStatus
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Postcode continues to confirm address",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.ConfirmAddress_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Confirm address continues to property type",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.PropertyType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Property type continues to the relevant specific type of property (house)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HouseType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Property type continues to the relevant specific type of property (bungalow)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.Bungalow
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.BungalowType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Property type continues to the relevant specific type of property (flat)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FlatType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "House type continues to home age",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HomeAge_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing house type continues to summary",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Bungalow type continues to home age",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HomeAge_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing bungalow type continues to summary",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Flat type continues to home age",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HomeAge_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing flat type continues to summary",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Home age continues to wall construction",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing home age continues to summary",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HomeAge
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Wall construction continues to the respective type of wall insulation",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Wall construction continues to floor construction if user's walls are not known and the user can answer floor questions",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                wallConstruction: WallConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Wall construction continues to roof construction if user's walls are not known, can not answer floor questions but can answer roof ones",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Wall construction continues to glazing type if user's walls are not known and can not answer neither floor questions nor roof ones",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                wallConstruction: WallConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing wall construction continues to summary if walls are not known",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Other,
                entryPoint: QuestionFlowPage.WallConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing cavity walls insulated continues to summary",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.CavityWallsInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Cavity walls insulated continue to solid walls insulated if user has mixed walls",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Mixed
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Cavity walls insulated continue to floor construction if user only does not have solid walls and can answer floor questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                wallConstruction: WallConstruction.Cavity
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Cavity walls insulated continue to roof construction if user only does not have solid walls, can not answer floor questions but can answer roof questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Cavity
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Cavity walls insulated continue to glazing type if user only does not have solid walls and can not answer neither floor questions nor roof questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                wallConstruction: WallConstruction.Cavity
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing solid walls insulated continues to summary",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.SolidWallsInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Solid walls insulated continue to floor construction if user can answer floor questions",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Solid walls insulated continue to roof construction if user can not answer floor questions but can answer roof ones",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Solid walls insulated continue to glazing type if user can not answer neither floor questions nor roof ones",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction continues to floor insulated if the floors are known",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                floorConstruction: FloorConstruction.Mix
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.FloorInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing floor construction continues to summary if floor are not known",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                floorConstruction: FloorConstruction.Other,
                entryPoint: QuestionFlowPage.FloorConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction continues to roof construction if floors are unknown and the user can answer roof questions",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor construction continues to glazing type if floor are unknown and the user can not answer roof questions",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                floorConstruction: FloorConstruction.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing floor insulated continues to summary",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor insulated continues to roof construction if user can answer roof questions",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Floor insulated continues to glazing type if user can not answer roof questions",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction continues to accessible loft space if roof is pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Mixed
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing roof construction continues to summary if roof is not pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Flat,
                entryPoint: QuestionFlowPage.RoofConstruction
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof construction continues to glazing type space if roof is not pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Flat
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Accessible loft space continues to roof insulated if user has it",
            new Input(
                QuestionFlowPage.AccessibleLoftSpace,
                "ABCDEFGH",
                accessibleLoftSpace: AccessibleLoftSpace.Yes
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.RoofInsulated_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing accessible loft space continues to answer summary if user does not have loft space",
            new Input(
                QuestionFlowPage.AccessibleLoftSpace,
                "ABCDEFGH",
                accessibleLoftSpace: AccessibleLoftSpace.No,
                entryPoint: QuestionFlowPage.AccessibleLoftSpace
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Accessible loft space continues to glazing type if user does not have it",
            new Input(
                QuestionFlowPage.AccessibleLoftSpace,
                "ABCDEFGH",
                accessibleLoftSpace: AccessibleLoftSpace.No
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Roof insulated continues to glazing type",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.GlazingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing roof insulated continues to summary",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofInsulated
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Glazing type continues to outdoor space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OutdoorSpace_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing glazing type continues to summary",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.GlazingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Outdoor space continues to heating type",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing outdoor space continues to summary",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OutdoorSpace
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating type continues to other heating type in that scenario",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.Other
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.OtherHeatingType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating type continues to hot water cylinder if user has a boiler",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.GasBoiler
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HotWaterCylinder_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing heating type continues to summary if user doesn't have a boiler or other heating type",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump,
                entryPoint: QuestionFlowPage.HeatingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating type continues to number of occupants if user doesn't have a boiler or other heating",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Other heating type continues to number of occupants",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing other heating type continues to summary",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OtherHeatingType
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Hot water cylinder continues to number of occupants",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing hot water cylinder continues to summary",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HotWaterCylinder
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Number of occupants continues to heating pattern",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingPattern_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing number of occupants continues to summary",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.NumberOfOccupants
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Heating pattern continues to temperature",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.Temperature_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing heating pattern continues to summary",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingPattern
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Temperature continues to summary",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            ))
    };

    private static QuestionFlowServiceTestCase[] SkipTestCases =
    {
        new(
            "Postcode skips to property type",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.PropertyType_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Home age skips to wall construction",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.WallConstruction_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing home age and skipping skips to summary",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HomeAge
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Number of occupants skips to heating pattern",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.HeatingPattern_Get),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Changing number of occupants and skipping skips to summary",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.NumberOfOccupants
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            )),
        new(
            "Temperature skips to summary",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH"
            ),
            new PathByActionArguments(
                nameof(EnergyEfficiencyController.AnswerSummary),
                "EnergyEfficiency",
                new { reference = "ABCDEFGH" }
            ))
    };

    public class QuestionFlowServiceTestCase
    {
        public readonly string Description;
        public readonly PathByActionArguments ExpectedOutput;
        public readonly Input Input;

        public QuestionFlowServiceTestCase(
            string description,
            Input input,
            PathByActionArguments expectedOutput
        )
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

    public class Input
    {
        public readonly QuestionFlowPage Page;
        public readonly PropertyData PropertyData;
        public QuestionFlowPage? EntryPoint;

        public Input(
            QuestionFlowPage page,
            string reference = null,
            OwnershipStatus? ownershipStatus = null,
            Country? country = null,
            Epc epc = null,
            string postcode = null,
            string houseNameOrNumber = null,
            PropertyType? propertyType = null,
            HouseType? houseType = null,
            BungalowType? bungalowType = null,
            FlatType? flatType = null,
            int? yearBuilt = null,
            WallConstruction? wallConstruction = null,
            CavityWallsInsulated? cavityWallsInsulated = null,
            SolidWallsInsulated? solidWallsInsulated = null,
            FloorConstruction? floorConstruction = null,
            FloorInsulated? floorInsulated = null,
            RoofConstruction? roofConstruction = null,
            AccessibleLoftSpace? accessibleLoftSpace = null,
            RoofInsulated? roofInsulated = null,
            HasOutdoorSpace? hasOutdoorSpace = null,
            GlazingType? glazingType = null,
            HeatingType? heatingType = null,
            OtherHeatingType? otherHeatingType = null,
            HasHotWaterCylinder? hasHotWaterCylinder = null,
            int? numberOfOccupants = null,
            HeatingPattern? heatingPattern = null,
            int? hoursOfHeatingMorning = null,
            int? hoursOfHeatingEvening = null,
            decimal? temperature = null,
            QuestionFlowPage? entryPoint = null)
        {
            Page = page;
            PropertyData = new PropertyData
            {
                Reference = reference,
                OwnershipStatus = ownershipStatus,
                Country = country,
                Epc = epc,
                Postcode = postcode,
                HouseNameOrNumber = houseNameOrNumber,
                PropertyType = propertyType,
                HouseType = houseType,
                BungalowType = bungalowType,
                FlatType = flatType,
                YearBuilt = yearBuilt,
                WallConstruction = wallConstruction,
                CavityWallsInsulated = cavityWallsInsulated,
                SolidWallsInsulated = solidWallsInsulated,
                FloorConstruction = floorConstruction,
                FloorInsulated = floorInsulated,
                RoofConstruction = roofConstruction,
                AccessibleLoftSpace = accessibleLoftSpace,
                RoofInsulated = roofInsulated,
                HasOutdoorSpace = hasOutdoorSpace,
                GlazingType = glazingType,
                HeatingType = heatingType,
                OtherHeatingType = otherHeatingType,
                HasHotWaterCylinder = hasHotWaterCylinder,
                NumberOfOccupants = numberOfOccupants,
                HeatingPattern = heatingPattern,
                HoursOfHeatingMorning = hoursOfHeatingMorning,
                HoursOfHeatingEvening = hoursOfHeatingEvening,
                Temperature = temperature,
            };
            EntryPoint = entryPoint;
        }
    }
}