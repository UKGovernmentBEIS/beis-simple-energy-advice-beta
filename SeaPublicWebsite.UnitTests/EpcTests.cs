using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.ExternalServices.EpbEpc;

namespace Tests;

public class EpcTests
{
    [TestCaseSource(nameof(EpcParseTestCases))]
    public void CanParseEpcDto(EpcTestCase testCase)
    {
        // Act
        var epc = testCase.Input.Parse();
        
        // Assert
        epc.Should().BeEquivalentTo(testCase.Output);
    }

    private static EpcTestCase[] LodgementDateTestCases =
    {
        new(
            "Can parse lodgement year",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                LodgementDate = "2012-12-22"
            },
            new Epc
            {
                LodgementYear = 2012
            })
    };
    
    private static EpcTestCase[] ConstructionAgeBandTestCases =
    {
        new(
            "Can parse before 1900",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "before 1900"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.Pre1900
            }),
        new(
            "Can parse band A",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "A"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.Pre1900
            })
    };

    private static EpcTestCase[] EpcParseTestCases =
        Array.Empty<EpcTestCase>()
            .Concat(LodgementDateTestCases)
            .Concat(ConstructionAgeBandTestCases)
            .ToArray();

    public class EpcTestCase
    {
        public readonly string Description;
        public readonly EpbEpcAssessmentDto Input;
        public readonly Epc Output;

        public EpcTestCase(
            string description, EpbEpcAssessmentDto input, Epc output)
        {
            Description = description;
            Input = input;
            Output = output;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}