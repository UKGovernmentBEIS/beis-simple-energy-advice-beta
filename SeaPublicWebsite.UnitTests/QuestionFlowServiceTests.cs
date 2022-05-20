using NUnit.Framework;
using FluentAssertions;
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
    
    [Theory]
    public void RunForwardLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Precondition
        Assume.That(testCase.TestType is TestType.Forward);
        
        // Act
        var output = QuestionFlowService.ForwardLinkArguments(
            testCase.Input.Page,
            testCase.Input.UserData,
            testCase.Input.EntryPoint);
        
        // Assert
        output.Should().BeEquivalentTo(testCase.ExpectedOutput);
    }
    
    [Theory]
    public void RunSkipLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Precondition
        Assume.That(testCase.TestType is TestType.Skip);
        
        // Act
        var output = QuestionFlowService.SkipLinkArguments(
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
        "Changing outdoor space goes back to summary",
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
    public QuestionFlowServiceTestCase YourRecommendationsBack1 = new(
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

    [Datapoint] 
    public QuestionFlowServiceTestCase CountryForward1 = new(
        "Country continues to service unsuitable if the service is not available",
        TestType.Forward,
        new Input(
            QuestionFlowPage.Country,
            reference: "ABCDEFGH",
            country: Country.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.ServiceUnsuitable),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CountryForward2 = new(
        "Country continues to ownership status",
        TestType.Forward,
        new Input(
            QuestionFlowPage.Country,
            reference: "ABCDEFGH",
            country: Country.England
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OwnershipStatus_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CountryForward3 = new(
        "Changing country continues to answer summary",
        TestType.Forward,
        new Input(
            QuestionFlowPage.Country,
            reference: "ABCDEFGH",
            country: Country.England,
            entryPoint: QuestionFlowPage.Country
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OwnershipStatusForward1 = new(
        "Ownership status continue to service unsuitable if user is a private tenant",
        TestType.Forward,
        new Input(
            QuestionFlowPage.OwnershipStatus,
            reference: "ABCDEFGH",
            ownershipStatus: OwnershipStatus.PrivateTenancy
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.ServiceUnsuitable),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OwnershipStatusForward2 = new(
        "Ownership status continue to postcode",
        TestType.Forward,
        new Input(
            QuestionFlowPage.OwnershipStatus,
            reference: "ABCDEFGH",
            ownershipStatus: OwnershipStatus.OwnerOccupancy
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AskForPostcode_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OwnershipStatusForward3 = new(
        "Changing ownership status continue to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase PostcodeForward1 = new(
        "Postcode continues to confirm address",
        TestType.Forward,
        new Input(
            QuestionFlowPage.AskForPostcode,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.ConfirmAddress_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase ConfirmAddressForward1 = new(
        "Confirm address continues to property type",
        TestType.Forward,
        new Input(
            QuestionFlowPage.ConfirmAddress,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.PropertyType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase PropertyTypeForward1 = new(
        "Property type continues to the relevant specific type of property",
        TestType.Forward,
        new Input(
            QuestionFlowPage.PropertyType,
            reference: "ABCDEFGH",
            propertyType: PropertyType.Bungalow
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.BungalowType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HouseTypeForward1 = new(
        "House type continues to home age",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HouseType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HomeAge_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HouseTypeForward2 = new(
        "Changing house type continues to summary",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HouseType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.PropertyType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase BungalowTypeForward1 = new(
        "Bungalow type continues to home age",
        TestType.Forward,
        new Input(
            QuestionFlowPage.BungalowType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HomeAge_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase BungalowTypeForward2 = new(
        "Changing bungalow type continues to summary",
        TestType.Forward,
        new Input(
            QuestionFlowPage.BungalowType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.PropertyType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FlatTypeForward1 = new(
        "Flat type continues to home age",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FlatType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HomeAge_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FlatTypeForward2 = new(
        "Changing flat type continues to summary",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FlatType,
            reference: "ABCDEFGH",
            entryPoint: QuestionFlowPage.PropertyType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeForward1 = new(
        "Home age continues to wall construction",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HomeAge,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeForward2 = new(
        "Changing home age continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase WallConstructionForward1 = new(
        "Wall construction continues to the respective type of wall insulation",
        TestType.Forward,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Solid
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionForward2 = new(
        "Wall construction continues to floor construction if user's walls are not known and the user can answer floor questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            wallConstruction: WallConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionForward3 = new(
        "Wall construction continues to roof construction if user's walls are not known, can not answer floor questions but can answer roof ones",
        TestType.Forward,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.TopFloor,
            wallConstruction: WallConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionForward4 = new(
        "Wall construction continues to glazing type if user's walls are not known and can not answer neither floor questions nor roof ones",
        TestType.Forward,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor,
            wallConstruction: WallConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase WallConstructionForward5 = new(
        "Changing wall construction continues to summary if walls are not known",
        TestType.Forward,
        new Input(
            QuestionFlowPage.WallConstruction,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Other,
            entryPoint: QuestionFlowPage.WallConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedForward1 = new(
        "Changing cavity walls insulated continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase CavityWallsInsulatedForward2 = new(
        "Cavity walls insulated continue to solid walls insulated if user has mixed walls",
        TestType.Forward,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH",
            wallConstruction: WallConstruction.Mixed
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.SolidWallsInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedForward3 = new(
        "Cavity walls insulated continue to floor construction if user only does not have solid walls and can answer floor questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            wallConstruction: WallConstruction.Cavity
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedForward4 = new(
        "Cavity walls insulated continue to roof construction if user only does not have solid walls, can not answer floor questions but can answer roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.TopFloor,
            wallConstruction: WallConstruction.Cavity
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase CavityWallsInsulatedForward5 = new(
        "Cavity walls insulated continue to glazing type if user only does not have solid walls and can not answer neither floor questions nor roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.CavityWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor,
            wallConstruction: WallConstruction.Cavity
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedForward1 = new(
        "Changing solid walls insulated continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase SolidWallsInsulatedForward2 = new(
        "Solid walls insulated continue to floor construction if user can answer floor questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedForward3 = new(
        "Solid walls insulated continue to roof construction if user can not answer floor questions but can answer roof ones",
        TestType.Forward,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.TopFloor
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase SolidWallsInsulatedForward4 = new(
        "Solid walls insulated continue to glazing type if user can not answer neither floor questions nor roof ones",
        TestType.Forward,
        new Input(
            QuestionFlowPage.SolidWallsInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionForward1 = new(
        "Floor construction continues to floor insulated if the floors are known",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            floorConstruction: FloorConstruction.Mix
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.FloorInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionForward2 = new(
        "Changing floor construction continues to summary if floor are not known",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            floorConstruction: FloorConstruction.Other,
            entryPoint: QuestionFlowPage.FloorConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionForward3 = new(
        "Floor construction continues to roof construction if floors are unknown and the user can answer roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House,
            floorConstruction: FloorConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorConstructionForward4 = new(
        "Floor construction continues to glazing type if floor are unknown and the user can not answer roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorConstruction,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor,
            floorConstruction: FloorConstruction.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorInsulatedForward1 = new(
        "Changing floor insulated continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase FloorInsulatedForward2 = new(
        "Floor insulated continues to roof construction if user can answer roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.House
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase FloorInsulatedForward3 = new(
        "Floor insulated continues to glazing type if user can not answer roof questions",
        TestType.Forward,
        new Input(
            QuestionFlowPage.FloorInsulated,
            reference: "ABCDEFGH",
            propertyType: PropertyType.ApartmentFlatOrMaisonette,
            flatType: FlatType.MiddleFloor
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstructionForward1 = new(
        "Roof construction continues to accessible loft space if roof is pitched",
        TestType.Forward,
        new Input(
            QuestionFlowPage.RoofConstruction,
            reference: "ABCDEFGH",
            roofConstruction: RoofConstruction.Mixed
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AccessibleLoftSpace_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstructionForward2 = new(
        "Changing roof construction continues to summary if roof is not pitched",
        TestType.Forward,
        new Input(
            QuestionFlowPage.RoofConstruction,
            reference: "ABCDEFGH",
            roofConstruction: RoofConstruction.Flat,
            entryPoint: QuestionFlowPage.RoofConstruction
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofConstructionForward3 = new(
        "Roof construction continues to glazing type space if roof is not pitched",
        TestType.Forward,
        new Input(
            QuestionFlowPage.RoofConstruction,
            reference: "ABCDEFGH",
            roofConstruction: RoofConstruction.Flat
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AccessibleLoftSpaceForward1 = new(
        "Accessible loft space continues to roof insulated if user has it",
        TestType.Forward,
        new Input(
            QuestionFlowPage.AccessibleLoftSpace,
            reference: "ABCDEFGH",
            accessibleLoftSpace: AccessibleLoftSpace.Yes
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.RoofInsulated_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AccessibleLoftSpaceForward2 = new(
        "Changing accessible loft space continues to answer summary if user does not have loft space",
        TestType.Forward,
        new Input(
            QuestionFlowPage.AccessibleLoftSpace,
            reference: "ABCDEFGH",
            accessibleLoftSpace: AccessibleLoftSpace.No,
            entryPoint: QuestionFlowPage.AccessibleLoftSpace
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase AccessibleLoftSpaceForward3 = new(
        "Accessible loft space continues to glazing type if user does not have it",
        TestType.Forward,
        new Input(
            QuestionFlowPage.AccessibleLoftSpace,
            reference: "ABCDEFGH",
            accessibleLoftSpace: AccessibleLoftSpace.No
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofInsulatedForward1 = new(
        "Roof insulated continues to glazing type",
        TestType.Forward,
        new Input(
            QuestionFlowPage.RoofInsulated,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.GlazingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase RoofInsulatedForward2 = new(
        "Changing roof insulated continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase GlazingTypeForward1 = new(
        "Glazing type continues to outdoor space",
        TestType.Forward,
        new Input(
            QuestionFlowPage.GlazingType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OutdoorSpace_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase GlazingTypeForward2 = new(
        "Changing glazing type continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase OutdoorSpaceForward1 = new(
        "Outdoor space continues to heating type",
        TestType.Forward,
        new Input(
            QuestionFlowPage.OutdoorSpace,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OutdoorSpaceForward2 = new(
        "Changing outdoor space continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase HeatingTypeForward1 = new(
        "Heating type continues to other heating type in that scenario",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH",
            heatingType: HeatingType.Other
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.OtherHeatingType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingTypeForward2 = new(
        "Heating type continues to hot water cylinder if user has a boiler",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH",
            heatingType: HeatingType.GasBoiler
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HotWaterCylinder_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingTypeForward3 = new(
        "Changing heating type continues to summary if user doesn't have a boiler or other heating type",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH",
            heatingType: HeatingType.HeatPump,
            entryPoint: QuestionFlowPage.HeatingType
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingTypeForward4 = new(
        "Heating type continues to number of occupants if user doesn't have a boiler or other heating",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HeatingType,
            reference: "ABCDEFGH",
            heatingType: HeatingType.HeatPump
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OtherHeatingTypeForward1 = new(
        "Other heating type continues to number of occupants",
        TestType.Forward,
        new Input(
            QuestionFlowPage.OtherHeatingType,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase OtherHeatingTypeForward2 = new(
        "Changing other heating type continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase HotWaterCylinderForward1 = new(
        "Hot water cylinder continues to number of occupants",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HotWaterCylinder,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.NumberOfOccupants_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HotWaterCylinderForward2 = new(
        "Changing hot water cylinder continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase NumberOfOccupantsForward1 = new(
        "Number of occupants continues to heating pattern",
        TestType.Forward,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingPattern_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsForward2 = new(
        "Changing number of occupants continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase HeatingPatternForward1 = new(
        "Heating pattern continues to temperature",
        TestType.Forward,
        new Input(
            QuestionFlowPage.HeatingPattern,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Temperature_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HeatingPatternForward2 = new(
        "Changing heating pattern continues to summary",
        TestType.Forward,
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
    public QuestionFlowServiceTestCase TemperatureForward1 = new(
        "Temperature continues to summary",
        TestType.Forward,
        new Input(
            QuestionFlowPage.Temperature,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase PostcodeSkip1 = new(
        "Postcode skips to property type",
        TestType.Skip,
        new Input(
            QuestionFlowPage.AskForPostcode,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.PropertyType_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeSkip1 = new(
        "Home age skips to wall construction",
        TestType.Skip,
        new Input(
            QuestionFlowPage.HomeAge,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.WallConstruction_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase HomeAgeSkip2 = new(
        "Changing home age and skipping skips to summary",
        TestType.Skip,
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
    public QuestionFlowServiceTestCase NumberOfOccupantsSkip1 = new(
        "Number of occupants skips to heating pattern",
        TestType.Skip,
        new Input(
            QuestionFlowPage.NumberOfOccupants,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.HeatingPattern_Get),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
    [Datapoint] 
    public QuestionFlowServiceTestCase NumberOfOccupantsSkip2 = new(
        "Changing number of occupants and skipping skips to summary",
        TestType.Skip,
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
    public QuestionFlowServiceTestCase TemperatureSkip1 = new(
        "Temperature skips to summary",
        TestType.Skip,
        new Input(
            QuestionFlowPage.Temperature,
            reference: "ABCDEFGH"
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.AnswerSummary),
            "EnergyEfficiency",
            new {reference = "ABCDEFGH"}
        ));
    
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