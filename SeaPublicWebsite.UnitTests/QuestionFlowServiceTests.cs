using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SeaPublicWebsite.Controllers;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using SeaPublicWebsite.Services;

namespace Tests;

[TestFixture]
public class QuestionFlowServiceTests
{
    private IQuestionFlowService QuestionFlowService;

    [SetUp]
    public void Setup()
    {
        QuestionFlowService = new QuestionFlowService();
    }

    [Theory]
    public void RunBackLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Precondition
        Assume.That(testCase.TestType is TestType.Back);
        
        // Act
        var output = QuestionFlowService.BackLinkArguments(
            testCase.Input.Page,
            testCase.Input.UserData,
            testCase.Input.EntryPoint);
        
        // Assert
        output.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }

    [Datapoint] 
    public QuestionFlowServiceTestCase NewOrReturningUserBack1 = new(
        "A new or returning user goes back to the Index page",
        TestType.Back,
        new Input(
            QuestionFlowPage.NewOrReturningUser
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Index),
            "EnergyEfficiency"
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CountryBack1 = new(
        "Country goes back to new or returning user",
        TestType.Back,
        new Input(
            QuestionFlowPage.Country
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.NewOrReturningUser_Get),
            "EnergyEfficiency"
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CountryBack2 = new(
        "Changing country goes to Summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.Country,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.Country
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OwnershipStatusBack1 = new(
        "Ownership status goes back to Country",
        TestType.Back,
        new Input(
            QuestionFlowPage.OwnershipStatus,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Country_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OwnershipStatusBack2 = new(
        "Changing ownership status goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.OwnershipStatus,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.OwnershipStatus
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase ServiceUnsuitableBack1 = new(
        "Service unsuitable goes back to the country you came from",
        TestType.Back,
        new Input(
            QuestionFlowPage.ServiceUnsuitable,
            reference: "ABCDEFGH",
            country: Country.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Country_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase ServiceUnsuitableBack2 = new(
        "Service unsuitable goes back to ownership status if user is a private tenant",
        TestType.Back,
        new Input(
            QuestionFlowPage.ServiceUnsuitable,
            reference: "ABCDEFGH",
            country: Country.England,
            ownershipStatus: OwnershipStatus.PrivateTenancy
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OwnershipStatus_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase PostcodeBack1 = new(
        "Postcode goes back to ownership status",
        TestType.Back,
        new Input(
            QuestionFlowPage.AskForPostcode,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OwnershipStatus_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase ConfirmAddressBack1 = new(
        "Confirm address goes back to postcode",
        TestType.Back,
        new Input(
            QuestionFlowPage.ConfirmAddress,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AskForPostcode_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase PropertyTypeBack1 = new(
        "Property type goes back to Postcode",
        TestType.Back,
        new Input(
            QuestionFlowPage.PropertyType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AskForPostcode_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase PropertyTypeBack2 = new(
        "Change property type goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.PropertyType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.PropertyType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HouseTypeBack1 = new(
        "House type goes back to property type and preserves entry point",
        TestType.Back,
        new Input(
            QuestionFlowPage.HouseType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.HouseType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.PropertyType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH", entryPoint = QuestionFlowPage.HouseType}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase BungalowTypeBack1 = new(
        "Bungalow type goes back to property type and preserves entry point",
        TestType.Back,
        new Input(
            QuestionFlowPage.BungalowType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.BungalowType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.PropertyType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH", entryPoint = QuestionFlowPage.BungalowType}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FlatTypeBack1 = new(
        "Flat type goes back to property type and preserves entry point",
        TestType.Back,
        new Input(
            QuestionFlowPage.FlatType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.FlatType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.PropertyType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH", entryPoint = QuestionFlowPage.FlatType}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeBack1 = new(
        "Home age goes back to the property type it came from",
        TestType.Back,
        new Input(
            QuestionFlowPage.HomeAge,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FlatType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeBack2 = new(
        "Changing home age goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.HomeAge,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.HomeAge
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionBack1 = new(
        "Wall construction goes back to home age",
        TestType.Back,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HomeAge_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionBack2 = new(
        "Changing wall construction goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.WallConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedBack1 = new(
        "Cavity walls insulated goes back to wall construction",
        TestType.Back,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedBack2 = new(
        "Changing cavity walls insulated goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.CavityWallsInsulated
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedBack1 = new(
        "Solid walls insulated goes back to cavity walls insulated if user has mixed walls",
        TestType.Back,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Mixed
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.CavityWallsInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedBack2 = new(
        "Solid walls insulated goes back to wall construction if user does not have mixed walls",
        TestType.Back,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Solid
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedBack3 = new(
        "Changing solid walls insulated goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.SolidWallsInsulated
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionBack1 = new(
        "Floor construction goes back to the wall insulation the user answered last",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Solid
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionBack2 = new(
        "Floor construction goes back to the wall insulation the user answered last",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Cavity
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.CavityWallsInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionBack3 = new(
        "Floor construction goes back to wall construction if user has neither cavity not solid walls",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionBack4 = new(
        "Changing floor construction goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.FloorConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorInsulated1 = new(
        "Floor insulated goes back to floor construction",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorInsulated,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorInsulated2 = new(
        "Changing floor insulated goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.FloorInsulated,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.FloorInsulated
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstruction1 = new(
        "Roof construction goes back to floor insulation if the user has timber or concrete floors",
        TestType.Back,
        new Input(
            QuestionFlowPage.RoofConstruction,
            propertyType: PropertyType.House,
            floorConstruction: FloorConstruction.SuspendedTimber,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstruction2 = new(
        "Roof construction goes back to floor construction if the user has different floors",
        TestType.Back,
        new Input(
            QuestionFlowPage.RoofConstruction,
            propertyType: PropertyType.House,
            floorConstruction: FloorConstruction.Other,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstruction3 = new(
        "Roof construction goes back to wall insulation if user was not asked about their floor but has insulated walls",
        TestType.Back,
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
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstruction4 = new(
        "Roof construction goes back to wall construction if user was not asked about their floor and has other walls",
        TestType.Back,
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
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstruction5 = new(
        "Changing roof construction goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.RoofConstruction,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.RoofConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AccessibleLoftSpaceBack1 = new(
        "Accessible loft space goes back to roof construction",
        TestType.Back,
        new Input(
            QuestionFlowPage.AccessibleLoftSpace,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AccessibleLoftSpaceBack2 = new(
        "Changing accessible loft space goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.AccessibleLoftSpace,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.AccessibleLoftSpace
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofInsulatedBack1 = new(
        "Roof insulated goes back to accessible loft space",
        TestType.Back,
        new Input(
            QuestionFlowPage.RoofInsulated,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofInsulatedBack2 = new(
        "Changing roof insulated goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.RoofInsulated,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.RoofInsulated
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack1 = new(
        "Glazing type goes back to roof construction if the user has flat roof",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            roofConstruction: RoofConstruction.Flat
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack2 = new(
        "Glazing type goes back to accessible loft space if the user does not have flat roof nor accessible loft space",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            roofConstruction: RoofConstruction.Pitched,
            accessibleLoftSpace: AccessibleLoftSpace.No
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack3 = new(
        "Glazing type goes back to roof insulation if the user does not have flat roof but has accessible loft space",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            roofConstruction: RoofConstruction.Pitched,
            accessibleLoftSpace: AccessibleLoftSpace.Yes
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack4 = new(
        "Glazing type goes back to floor construction if the user was not asked about their roof",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.GroundFloor,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack5 = new(
        "Glazing type goes back to wall construction if the user was not asked about neither their roof or floor",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeBack6 = new(
        "Changing glazing type goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.GlazingType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.GlazingType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OutdoorSpaceBack1 = new(
        "Outdoor space goes back to glazing type",
        TestType.Back,
        new Input(
            QuestionFlowPage.OutdoorSpace,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OutdoorSpaceBack2 = new(
        "Changing outdor space goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.OutdoorSpace,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.OutdoorSpace
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingTypeBack1 = new(
        "Heating type goes back to outdoor space",
        TestType.Back,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OutdoorSpace_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingTypeBack2 = new(
        "Changing heating type goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.HeatingType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OtherHeatingTypeBack1 = new(
        "Other heating type goes back to outdoor space",
        TestType.Back,
        new Input(
            QuestionFlowPage.OtherHeatingType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OtherHeatingTypeBack2 = new(
        "Changing other heating type goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.OtherHeatingType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.OtherHeatingType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HotWaterCylinderBack1 = new(
        "Hot water cylinder goes back to heating type",
        TestType.Back,
        new Input(
            QuestionFlowPage.HotWaterCylinder,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HotWaterCylinderBack2 = new(
        "Changing hot water cylinder goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.HotWaterCylinder,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.HotWaterCylinder
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsBack1 = new(
        "Number of occupants goes back to hot water cylinder if user has boiler",
        TestType.Back,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH",
            heatingType: HeatingType.GasBoiler
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HotWaterCylinder_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsBack2 = new(
        "Number of occupants goes back to other heating type if the user selected so",
        TestType.Back,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH",
            heatingType: HeatingType.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OtherHeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsBack3 = new(
        "Number of occupants goes back to heating type if the user does not have a boiler",
        TestType.Back,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH",
            heatingType: HeatingType.HeatPump
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsBack4 = new(
        "Changing number of occupants goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.NumberOfOccupants
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingPatternBack1 = new(
        "Heating pattern goes back to number of occupants",
        TestType.Back,
        new Input(
            QuestionFlowPage.HeatingPattern,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingPatternBack2 = new(
        "Changing heating pattern goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.HeatingPattern,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.HeatingPattern
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase TemperatureBack1 = new(
        "Temperature goes back to heating pattern",
        TestType.Back,
        new Input(
            QuestionFlowPage.Temperature,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingPattern_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase TemperatureBack2 = new(
        "Changing temperature goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.Temperature,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.Temperature
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AnswerSummaryBack1 = new(
        "Answer summary goes back to temperature",
        TestType.Back,
        new Input(
            QuestionFlowPage.AnswerSummary,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Temperature_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase YourReccomendationsBack1 = new(
        "Your recommendations goes back to summary",
        TestType.Back,
        new Input(
            QuestionFlowPage.YourRecommendations,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    /*
    
    [Datapoint] 
    public QuestionFlowServiceTestCase x = new(
        "",
        TestType.Back,
        new Input(
            QuestionFlowPage.,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
        
        */


    public class QuestionFlowServiceTestCase
    {
        public readonly string Description;
        public readonly TestType TestType;
        public readonly Input Input;
        public readonly PathByActionArguments ExpectedOutput;

        public QuestionFlowServiceTestCase(
            string description,
            TestType testType,
            Input input,
            PathByActionArguments expectedOutput
        )
        {
            Description = description;
            TestType = testType;
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
        public readonly UserDataModel UserData;
        public QuestionFlowPage? EntryPoint;
        public Input(
            QuestionFlowPage page,
            string? reference = null,
            OwnershipStatus? ownershipStatus = null,
            Country? country = null,
            Epc? epc = null,
            string? postcode = null,
            string? houseNameOrNumber = null,
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
            decimal? hoursOfHeating = null,
            decimal? temperature = null,
            HasEmailAddress? hasEmailAddress = null,
            string? emailAddress = null,
            QuestionFlowPage? entryPoint = null)
        {
            Page = page;
            UserData = new UserDataModel
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
                HoursOfHeating = hoursOfHeating,
                Temperature = temperature,
                HasEmailAddress = hasEmailAddress,
                EmailAddress = emailAddress,
            };
            EntryPoint = entryPoint;
        }
    }
    
    public enum TestType
    {
        Back,
        Forward,
        Skip
    }
}