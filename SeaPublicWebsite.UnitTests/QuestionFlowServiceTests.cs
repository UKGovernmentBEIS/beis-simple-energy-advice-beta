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

    public QuestionFlowServiceTests()
    { }

    [SetUp]
    public void Setup()
    {
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
    public QuestionFlowServiceTestCase NewOrReturningUserReturnsToIndex = new QuestionFlowServiceTestCase(
        "Description",
        TestType.Back,
        new Input(
            QuestionFlowPage.NewOrReturningUser
        ),
        new PathByActionArguments(
            nameof(EnergyEfficiencyController.Index),
            "EnergyEfficiency"
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