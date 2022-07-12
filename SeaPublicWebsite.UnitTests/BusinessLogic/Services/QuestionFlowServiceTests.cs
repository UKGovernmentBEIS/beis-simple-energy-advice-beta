using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.BusinessLogic.Services;

namespace Tests.BusinessLogic.Services;

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
        var output = QuestionFlowService.BackDestination(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().Be(testCase.ExpectedOutput);
    }

    [TestCaseSource(nameof(ForwardTestCases))]
    public void RunForwardLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Act
        var output = QuestionFlowService.ForwardDestination(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().Be(testCase.ExpectedOutput);
    }

    [TestCaseSource(nameof(SkipTestCases))]
    public void RunSkipLinkTestCases(QuestionFlowServiceTestCase testCase)
    {
        // Act
        var output = QuestionFlowService.SkipDestination(
            testCase.Input.Page,
            testCase.Input.PropertyData,
            testCase.Input.EntryPoint);

        // Assert
        output.Should().Be(testCase.ExpectedOutput);
    }

    private static QuestionFlowServiceTestCase[] BackTestCases =
    {
        new(
            "A new or returning user goes back to the Index page",
            new Input(
                QuestionFlowPage.NewOrReturningUser
            ),
            QuestionFlowPage.Start),
        new(
            "Country goes back to new or returning user",
            new Input(
                QuestionFlowPage.Country
            ),
            QuestionFlowPage.NewOrReturningUser),
        new(
            "Ownership status goes back to Country",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH"
            ),
            QuestionFlowPage.Country),
        new(
            "Service unsuitable goes back to the country you came from",
            new Input(
                QuestionFlowPage.ServiceUnsuitable,
                "ABCDEFGH",
                country: Country.Other
            ),
            QuestionFlowPage.Country),
        new(
            "Service unsuitable goes back to ownership status if user is a private tenant",
            new Input(
                QuestionFlowPage.ServiceUnsuitable,
                "ABCDEFGH",
                country: Country.England,
                ownershipStatus: OwnershipStatus.PrivateTenancy
            ),
            QuestionFlowPage.OwnershipStatus),
        new(
            "Find EPC goes back to ownership status",
            new Input(
                QuestionFlowPage.FindEpc,
                "ABCDEFGH"
            ),
            QuestionFlowPage.OwnershipStatus),
        new(
            "Postcode goes back to find EPC",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH"
            ),
            QuestionFlowPage.FindEpc),
        new(
            "Confirm address goes back to postcode",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "Confirm EPC details goes back to ask for postcode",
            new Input(
                QuestionFlowPage.ConfirmEpcDetails,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "No EPC found goes back to postcode",
            new Input(
                QuestionFlowPage.NoEpcFound,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "Property type goes back to confirm EPC details if epc exists with property type and age",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                epc: new Epc
                {
                    PropertyType = PropertyType.House,
                    HouseType = HouseType.Detached,
                    ConstructionAgeBand = HomeAge.From1950To1966
                }
            ),
            QuestionFlowPage.ConfirmEpcDetails),
        new(
            "Property type goes back to postcode if epc doesn't contain property type",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                epc: new Epc
                {
                    ConstructionAgeBand = HomeAge.From1950To1966
                }
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "Property type goes back to postcode if epc doesn't contain property age",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                epc: new Epc()
                {
                    PropertyType = PropertyType.House,
                    HouseType = HouseType.Detached
                }
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "Property type goes back to no EPC found if searched for EPC but no EPC set",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                epc: null,
                searchForEpc: SearchForEpc.Yes
            ),
            QuestionFlowPage.NoEpcFound),
        new(
            "Property type goes back to find EPC if didn't search for EPC",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                searchForEpc: SearchForEpc.No
            ),
            QuestionFlowPage.FindEpc),
        new(
            "Changing property type goes back to check your unchangeable answers",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "House type goes back to property type",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Bungalow type goes back to property type",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Flat type goes back to property type",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Home age goes back to the property type it came from (house)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            QuestionFlowPage.HouseType),
        new(
            "Home age goes back to the property type it came from (bungalow)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.Bungalow
            ),
            QuestionFlowPage.BungalowType),
        new(
            "Home age goes back to the property type it came from (flat)",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette
            ),
            QuestionFlowPage.FlatType),
        new(
            "Changing home age goes back to check your unchangeable answers",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HomeAge
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Wall construction goes back to check your unchangeable answers if EPC details were not confirmed",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                epcDetailsConfirmed: EpcDetailsConfirmed.No

            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Wall construction goes back to confirm EPC details if EPC details were confirmed",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                epcDetailsConfirmed: EpcDetailsConfirmed.Yes
            ),
            QuestionFlowPage.ConfirmEpcDetails),
        new(
            "Changing wall construction goes back to summary",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.WallConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Cavity walls insulated goes back to wall construction",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH"
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Changing cavity walls insulated goes back to summary",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.CavityWallsInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Solid walls insulated goes back to cavity walls insulated if user has mixed walls",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Mixed
            ),
            QuestionFlowPage.CavityWallsInsulated),
        new(
            "Solid walls insulated goes back to wall construction if user does not have mixed walls",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Changing solid walls insulated goes back to summary",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.SolidWallsInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Floor construction goes back to the wall insulation the user answered last (solid walls)",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            QuestionFlowPage.SolidWallsInsulated),
        new(
            "Floor construction goes back to the wall insulation the user answered last (cavity walls)",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Cavity
            ),
            QuestionFlowPage.CavityWallsInsulated),
        new(
            "Floor construction goes back to wall construction if user has neither cavity not solid walls",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Other
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Changing floor construction goes back to summary",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Floor insulated goes back to floor construction",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH"
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Changing floor insulated goes back to summary",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Roof construction goes back to floor insulation if the user has timber or concrete floors",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.SuspendedTimber,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.FloorInsulated),
        new(
            "Roof construction goes back to floor construction if the user has different floors",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.Other,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Roof construction goes back to wall insulation if user was not asked about their floor but has insulated walls",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Solid,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.SolidWallsInsulated),
        new(
            "Roof construction goes back to wall construction if user was not asked about their floor and has other walls",
            new Input(
                QuestionFlowPage.RoofConstruction,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.DoNotKnow,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Changing roof construction goes back to summary",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Loft space goes back to roof construction",
            new Input(
                QuestionFlowPage.LoftSpace,
                "ABCDEFGH"
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Changing loft space goes back to summary",
            new Input(
                QuestionFlowPage.LoftSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.LoftSpace
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Loft access goes back to loft space",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH"
            ),
            QuestionFlowPage.LoftSpace),
        new(
            "Changing loft access goes back to summary",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.LoftAccess
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Roof insulated goes back to loft access",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH"
            ),
            QuestionFlowPage.LoftAccess),
        new(
            "Changing roof insulated goes back to summary",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Glazing type goes back to roof construction if the user has flat roof",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Flat
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Glazing type goes back to loft space if the user does not have flat roof nor loft space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Pitched,
                loftSpace: LoftSpace.No
            ),
            QuestionFlowPage.LoftSpace),
        new(
            "Glazing type goes back to loft access if the user does not have flat roof and has inaccessible loft space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Pitched,
                loftSpace: LoftSpace.Yes,
                accessibleLoft: LoftAccess.No
            ),
            QuestionFlowPage.LoftAccess),
        new(
            "Glazing type goes back to roof insulation if the user does not have flat roof but has accessible loft space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                roofConstruction: RoofConstruction.Pitched,
                loftSpace: LoftSpace.Yes,
                accessibleLoft: LoftAccess.Yes
            ),
            QuestionFlowPage.RoofInsulated),
        new(
            "Glazing type goes back to floor construction if the user was not asked about their roof",
            new Input(
                QuestionFlowPage.GlazingType,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.GroundFloor,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Glazing type goes back to wall construction if the user was not asked about neither their roof or floor",
            new Input(
                QuestionFlowPage.GlazingType,
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                reference: "ABCDEFGH"
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Changing glazing type goes back to summary",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.GlazingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Outdoor space goes back to glazing type",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH"
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Changing outdoor space goes back to summary",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OutdoorSpace
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Heating type goes back to outdoor space",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.OutdoorSpace),
        new(
            "Changing heating type goes back to summary",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Other heating type goes back to outdoor space",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HeatingType),
        new(
            "Changing other heating type goes back to summary",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OtherHeatingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Hot water cylinder goes back to heating type",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HeatingType),
        new(
            "Changing hot water cylinder goes back to summary",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HotWaterCylinder
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Number of occupants goes back to hot water cylinder if user has boiler",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.GasBoiler
            ),
            QuestionFlowPage.HotWaterCylinder),
        new(
            "Number of occupants goes back to other heating type if the user selected so",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.Other
            ),
            QuestionFlowPage.OtherHeatingType),
        new(
            "Number of occupants goes back to heating type if the user does not have a boiler",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump
            ),
            QuestionFlowPage.HeatingType),
        new(
            "Changing number of occupants goes back to summary",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.NumberOfOccupants
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Heating pattern goes back to number of occupants",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH"
            ),
            QuestionFlowPage.NumberOfOccupants),
        new(
            "Changing heating pattern goes back to summary",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingPattern
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Temperature goes back to heating pattern",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HeatingPattern),
        new(
            "Changing temperature goes back to summary",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.Temperature
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Answer summary goes back to temperature",
            new Input(
                QuestionFlowPage.AnswerSummary,
                "ABCDEFGH"
            ),
            QuestionFlowPage.Temperature),
        new(
            "No recommendations goes back to summary",
            new Input(
                QuestionFlowPage.NoRecommendations,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Your recommendations goes back to summary",
            new Input(
                QuestionFlowPage.YourRecommendations,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AnswerSummary)
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
            QuestionFlowPage.ServiceUnsuitable),
        new(
            "Country continues to ownership status",
            new Input(
                QuestionFlowPage.Country,
                "ABCDEFGH",
                country: Country.England
            ),
            QuestionFlowPage.OwnershipStatus),
        new(
            "Ownership status continues to service unsuitable if user is a private tenant",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                OwnershipStatus.PrivateTenancy
            ),
            QuestionFlowPage.ServiceUnsuitable),
        new(
            "Ownership status continues to find EPC",
            new Input(
                QuestionFlowPage.OwnershipStatus,
                "ABCDEFGH",
                OwnershipStatus.OwnerOccupancy
            ),
            QuestionFlowPage.FindEpc),
        new(
            "Find EPC continues to postcode if yes selected",
            new Input(
                QuestionFlowPage.FindEpc,
                "ABCDEFGH",
                searchForEpc: SearchForEpc.Yes
            ),
            QuestionFlowPage.AskForPostcode),
        new(
            "Find EPC continues to property type if no selected",
            new Input(
                QuestionFlowPage.FindEpc,
                "ABCDEFGH",
                searchForEpc: SearchForEpc.No
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Postcode continues to confirm address",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH",
                searchForEpc: SearchForEpc.Yes
            ),
            QuestionFlowPage.ConfirmAddress),
        new(
            "Postcode continues to property type on cancel",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH",
                searchForEpc: SearchForEpc.No
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Confirm address continues to no EPC found if no EPC set",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH",
                epc: null
            ),
            QuestionFlowPage.NoEpcFound),
        new(
            "Confirm address continues to confirm EPC details if EPC added contains property type and age",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH",
                epc: new Epc
                {
                    PropertyType = PropertyType.House,
                    HouseType = HouseType.Detached,
                    ConstructionAgeBand = HomeAge.From1950To1966
                }
            ),
            QuestionFlowPage.ConfirmEpcDetails),
        new(
            "Confirm address continues to property type if EPC exists but property type is missing",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH",
                epc: new Epc
                {
                    ConstructionAgeBand = HomeAge.From1950To1966
                }
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Confirm address continues to property type if EPC exists but age is missing",
            new Input(
                QuestionFlowPage.ConfirmAddress,
                "ABCDEFGH",
                epc: new Epc()
                {
                    PropertyType = PropertyType.House,
                    HouseType = HouseType.Detached
                }
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Confirm EPC details continues to wall construction if epc details confirmed",
            new Input(
                QuestionFlowPage.ConfirmEpcDetails,
                "ABCDEFGH",
                epcDetailsConfirmed: EpcDetailsConfirmed.Yes
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Confirm EPC details continues to property type if epc details not confirmed",
            new Input(
                QuestionFlowPage.ConfirmEpcDetails,
                "ABCDEFGH",
                epcDetailsConfirmed: EpcDetailsConfirmed.No
            ),
            QuestionFlowPage.PropertyType),
        new(
            "No EPC found continues to property type",
            new Input(
                QuestionFlowPage.NoEpcFound,
                "ABCDEFGH"
            ),
            QuestionFlowPage.PropertyType),
        new(
            "Property type continues to the relevant specific type of property (house)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            QuestionFlowPage.HouseType),
        new(
            "Property type continues to the relevant specific type of property (bungalow)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.Bungalow
            ),
            QuestionFlowPage.BungalowType),
        new(
            "Property type continues to the relevant specific type of property (flat)",
            new Input(
                QuestionFlowPage.PropertyType,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette
            ),
            QuestionFlowPage.FlatType),
        new(
            "House type continues to home age",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HomeAge),
        new(
            "Changing house type continues to check your unchangeable answers",
            new Input(
                QuestionFlowPage.HouseType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Bungalow type continues to home age",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HomeAge),
        new(
            "Changing bungalow type continues to check your unchangeable answers",
            new Input(
                QuestionFlowPage.BungalowType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Flat type continues to home age",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HomeAge),
        new(
            "Changing flat type continues to check your unchangeable answers",
            new Input(
                QuestionFlowPage.FlatType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.PropertyType
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Home age continues to check your unchangeable answers",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH"
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Changing home age continues to check your unchangeable answers",
            new Input(
                QuestionFlowPage.HomeAge,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HomeAge
            ),
            QuestionFlowPage.CheckYourUnchangeableAnswers),
        new(
            "Check your unchangeable answers continues to wall consutruction",
            new Input(
                QuestionFlowPage.CheckYourUnchangeableAnswers,
                "ABCDEFGH"
            ),
            QuestionFlowPage.WallConstruction),
        new(
            "Wall construction continues to the respective type of wall insulation",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Solid
            ),
            QuestionFlowPage.SolidWallsInsulated),
        new(
            "Wall construction continues to floor construction if user's walls are not known and the user can answer floor questions",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                wallConstruction: WallConstruction.Other
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Wall construction continues to roof construction if user's walls are not known, can not answer floor questions but can answer roof ones",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Other
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Wall construction continues to glazing type if user's walls are not known and can not answer neither floor questions nor roof ones",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                wallConstruction: WallConstruction.Other
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Changing wall construction continues to summary if walls are not known",
            new Input(
                QuestionFlowPage.WallConstruction,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Other,
                entryPoint: QuestionFlowPage.WallConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Changing cavity walls insulated continues to summary",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.CavityWallsInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Cavity walls insulated continue to solid walls insulated if user has mixed walls",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                wallConstruction: WallConstruction.Mixed
            ),
            QuestionFlowPage.SolidWallsInsulated),
        new(
            "Cavity walls insulated continue to floor construction if user only does not have solid walls and can answer floor questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                wallConstruction: WallConstruction.Cavity
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Cavity walls insulated continue to roof construction if user only does not have solid walls, can not answer floor questions but can answer roof questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor,
                wallConstruction: WallConstruction.Cavity
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Cavity walls insulated continue to glazing type if user only does not have solid walls and can not answer neither floor questions nor roof questions",
            new Input(
                QuestionFlowPage.CavityWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                wallConstruction: WallConstruction.Cavity
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Changing solid walls insulated continues to summary",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.SolidWallsInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Solid walls insulated continue to floor construction if user can answer floor questions",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            QuestionFlowPage.FloorConstruction),
        new(
            "Solid walls insulated continue to roof construction if user can not answer floor questions but can answer roof ones",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.TopFloor
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Solid walls insulated continue to glazing type if user can not answer neither floor questions nor roof ones",
            new Input(
                QuestionFlowPage.SolidWallsInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Floor construction continues to floor insulated if the floors are known",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                floorConstruction: FloorConstruction.Mix
            ),
            QuestionFlowPage.FloorInsulated),
        new(
            "Changing floor construction continues to summary if floor are not known",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                floorConstruction: FloorConstruction.Other,
                entryPoint: QuestionFlowPage.FloorConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Floor construction continues to roof construction if floors are unknown and the user can answer roof questions",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.House,
                floorConstruction: FloorConstruction.Other
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Floor construction continues to glazing type if floor are unknown and the user can not answer roof questions",
            new Input(
                QuestionFlowPage.FloorConstruction,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor,
                floorConstruction: FloorConstruction.Other
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Changing floor insulated continues to summary",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.FloorInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Floor insulated continues to roof construction if user can answer roof questions",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.House
            ),
            QuestionFlowPage.RoofConstruction),
        new(
            "Floor insulated continues to glazing type if user can not answer roof questions",
            new Input(
                QuestionFlowPage.FloorInsulated,
                "ABCDEFGH",
                propertyType: PropertyType.ApartmentFlatOrMaisonette,
                flatType: FlatType.MiddleFloor
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Roof construction continues to loft space if roof is pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Pitched
            ),
            QuestionFlowPage.LoftSpace),
        new(
            "Roof construction continues to loft space if roof is mixed",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Mixed
            ),
            QuestionFlowPage.LoftSpace),
        new(
            "Changing roof construction continues to summary if roof is not pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Flat,
                entryPoint: QuestionFlowPage.RoofConstruction
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Roof construction continues to glazing type space if roof is not pitched",
            new Input(
                QuestionFlowPage.RoofConstruction,
                "ABCDEFGH",
                roofConstruction: RoofConstruction.Flat
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Loft space continues to loft access if user has it",
            new Input(
                QuestionFlowPage.LoftSpace,
                "ABCDEFGH",
                loftSpace: LoftSpace.Yes
            ),
            QuestionFlowPage.LoftAccess),
        new(
            "Changing loft space continues to answer summary if user does not have loft space",
            new Input(
                QuestionFlowPage.LoftSpace,
                "ABCDEFGH",
                loftSpace: LoftSpace.No,
                entryPoint: QuestionFlowPage.LoftSpace
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Loft space continues to glazing type if user does not have it",
            new Input(
                QuestionFlowPage.LoftSpace,
                "ABCDEFGH",
                loftSpace: LoftSpace.No
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Loft access continues to roof insulation if user has it",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH",
                accessibleLoft: LoftAccess.Yes
            ),
            QuestionFlowPage.RoofInsulated),
        new(
            "Changing loft access continues to roof insulation if user has it",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH",
                accessibleLoft: LoftAccess.Yes,
                entryPoint: QuestionFlowPage.LoftAccess
            ),
            QuestionFlowPage.RoofInsulated),
        new(
            "Changing loft access continues to answer summary if user does not have it",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH",
                accessibleLoft: LoftAccess.No,
                entryPoint: QuestionFlowPage.LoftAccess
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Loft access continues to glazing type if user does not have it",
            new Input(
                QuestionFlowPage.LoftAccess,
                "ABCDEFGH",
                loftSpace: LoftSpace.No
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Roof insulated continues to glazing type",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH"
            ),
            QuestionFlowPage.GlazingType),
        new(
            "Changing roof insulated continues to summary",
            new Input(
                QuestionFlowPage.RoofInsulated,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.RoofInsulated
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Glazing type continues to outdoor space",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.OutdoorSpace),
        new(
            "Changing glazing type continues to summary",
            new Input(
                QuestionFlowPage.GlazingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.GlazingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Outdoor space continues to heating type",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HeatingType),
        new(
            "Changing outdoor space continues to summary",
            new Input(
                QuestionFlowPage.OutdoorSpace,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OutdoorSpace
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Heating type continues to other heating type in that scenario",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.Other
            ),
            QuestionFlowPage.OtherHeatingType),
        new(
            "Heating type continues to hot water cylinder if user has a boiler",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.GasBoiler
            ),
            QuestionFlowPage.HotWaterCylinder),
        new(
            "Changing heating type continues to summary if user doesn't have a boiler or other heating type",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump,
                entryPoint: QuestionFlowPage.HeatingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Heating type continues to number of occupants if user doesn't have a boiler or other heating",
            new Input(
                QuestionFlowPage.HeatingType,
                "ABCDEFGH",
                heatingType: HeatingType.HeatPump
            ),
            QuestionFlowPage.NumberOfOccupants),
        new(
            "Other heating type continues to number of occupants",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH"
            ),
            QuestionFlowPage.NumberOfOccupants),
        new(
            "Changing other heating type continues to summary",
            new Input(
                QuestionFlowPage.OtherHeatingType,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.OtherHeatingType
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Hot water cylinder continues to number of occupants",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH"
            ),
            QuestionFlowPage.NumberOfOccupants),
        new(
            "Changing hot water cylinder continues to summary",
            new Input(
                QuestionFlowPage.HotWaterCylinder,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HotWaterCylinder
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Number of occupants continues to heating pattern",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH"
            ),
            QuestionFlowPage.HeatingPattern),
        new(
            "Changing number of occupants continues to summary",
            new Input(
                QuestionFlowPage.NumberOfOccupants,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.NumberOfOccupants
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Heating pattern continues to temperature",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH"
            ),
            QuestionFlowPage.Temperature),
        new(
            "Changing heating pattern continues to summary",
            new Input(
                QuestionFlowPage.HeatingPattern,
                "ABCDEFGH",
                entryPoint: QuestionFlowPage.HeatingPattern
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Temperature continues to summary",
            new Input(
                QuestionFlowPage.Temperature,
                "ABCDEFGH"
            ),
            QuestionFlowPage.AnswerSummary),
        new(
            "Answer summary continues to your recommendations if user has at least one",
            new Input(
                QuestionFlowPage.AnswerSummary,
                "ABCDEFGH",
                propertyRecommendations: new List<PropertyRecommendation> {new()}
            ),
            QuestionFlowPage.YourRecommendations),
        new(
            "Answer summary continues to no recommendations if user has none",
            new Input(
                QuestionFlowPage.AnswerSummary,
                "ABCDEFGH",
                propertyRecommendations: new List<PropertyRecommendation>()),
            QuestionFlowPage.NoRecommendations)
    };

    private static QuestionFlowServiceTestCase[] SkipTestCases =
    {
        new(
            "Postcode skips to property type",
            new Input(
                QuestionFlowPage.AskForPostcode,
                "ABCDEFGH"
            ),
            QuestionFlowPage.PropertyType)
    };

    public class QuestionFlowServiceTestCase
    {
        public readonly string Description;
        public readonly QuestionFlowPage ExpectedOutput;
        public readonly Input Input;

        public QuestionFlowServiceTestCase(
            string description,
            Input input,
            QuestionFlowPage expectedOutput
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
            SearchForEpc? searchForEpc = null,
            EpcDetailsConfirmed? epcDetailsConfirmed = null,
            PropertyType? propertyType = null,
            HouseType? houseType = null,
            BungalowType? bungalowType = null,
            FlatType? flatType = null,
            YearBuilt? yearBuilt = null,
            WallConstruction? wallConstruction = null,
            CavityWallsInsulated? cavityWallsInsulated = null,
            SolidWallsInsulated? solidWallsInsulated = null,
            FloorConstruction? floorConstruction = null,
            FloorInsulated? floorInsulated = null,
            RoofConstruction? roofConstruction = null,
            LoftSpace? loftSpace = null,
            LoftAccess? accessibleLoft = null,
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
            QuestionFlowPage? entryPoint = null,
            List<PropertyRecommendation> propertyRecommendations = null)
        {
            Page = page;
            PropertyData = new PropertyData
            {
                Reference = reference,
                OwnershipStatus = ownershipStatus,
                Country = country,
                Epc = epc,
                SearchForEpc = searchForEpc,
                EpcDetailsConfirmed = epcDetailsConfirmed,
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
                LoftSpace = loftSpace,
                LoftAccess = accessibleLoft,
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
                PropertyRecommendations = propertyRecommendations
            };
            EntryPoint = entryPoint;
        }
    }
}