using System;
using System.Collections.Generic;
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

    private static EpcTestCase[] AssessmentTypeTestCases =
    {
        new(
            "Can parse an RdSAP assessment",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP"
            },
            new Epc()),
        new(
            "Does not parse a SAP assessment",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "SAP"
            },
            null),
    };

    private static EpcTestCase[] LodgementDateTestCases =
    {
        
        new(
            "Can handle null lodgement year",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                LodgementDate = null
            },
            new Epc
            {
                LodgementYear = null
            }),
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
            "Can handle null home age",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = null
            },
            new Epc
            {
                ConstructionAgeBand = null
            }),
        new(
            "Can parse home age before 1900",
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
            "Can parse home age band A",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "A"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.Pre1900
            }),
        
        new(
            "Can parse home age 1900-1929",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1900-1929"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1900To1929
            }),
        new(
            "Can parse home age band B",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "B"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1900To1929
            }),
        new(
            "Can parse home age 1930-1949",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1930-1949"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1930To1949
            }),
        new(
            "Can parse home age band C",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "C"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1930To1949
            }),
        new(
            "Can parse home age 1950-1966",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1950-1966"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1950To1966
            }),
        new(
            "Can parse home age band D",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "D"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1950To1966
            }),
        new(
            "Can parse home age 1967-1975",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1967-1975"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1967To1975
            }),
        new(
            "Can parse home age band E",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "E"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1967To1975
            }),
        new(
            "Can parse home age 1976-1982",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1976-1982"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1976To1982
            }),
        new(
            "Can parse home age band F",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "F"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1976To1982
            }),
        new(
            "Can parse home age 1983-1990",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1983-1990"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1983To1990
            }),
        new(
            "Can parse home age band G",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "G"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1983To1990
            }),
        new(
            "Can parse home age 1991-1995",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1991-1995"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1991To1995
            }),
        new(
            "Can parse home age band H",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "H"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1991To1995
            }),
        new(
            "Can parse home age 1996-2002",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "1996-2002"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1996To2002
            }),
        new(
            "Can parse home age band I",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "I"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From1996To2002
            }),
        new(
            "Can parse home age 2003-2006",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "2003-2006"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2003To2006
            }),
        new(
            "Can parse home age band J",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "J"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2003To2006
            }),
        new(
            "Can parse home age 2007-2011",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "2007-2011"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2007To2011
            }),
        new(
            "Can parse home age band K",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "K"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2007To2011
            }),
        new(
            "Can parse home age 2012 onwards",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "2012 onwards"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2012ToPresent
            }),
        new(
            "Can parse home age band L",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyAgeBand = "L"
            },
            new Epc
            {
                ConstructionAgeBand = HomeAge.From2012ToPresent
            }),
    };

    private static EpcTestCase[] PropertyTypeTestCases =
    {
        new(
            "Can handle null property type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = null
            },
            new Epc
            {
                PropertyType = null
            }),
        new(
            "Can parse property type house",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "house"
            },
            new Epc
            {
                PropertyType = PropertyType.House
            }),
        new(
            "Can parse property type bungalow",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "bungalow"
            },
            new Epc
            {
                PropertyType = PropertyType.Bungalow
            }),
        new(
            "Can parse property type flat",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "flat"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette
            }),
        new(
            "Can parse property type maisonette",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "maisonette"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette
            }),
    };

    private static EpcTestCase[] HouseTypeTestCases =
    {
        new(
            "Can handle null house type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = null
            },
            new Epc
            {
                HouseType = null
            }),
        new(
            "Can parse house type detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "detached house"
            },
            new Epc
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.Detached
            }),
        new(
            "Can parse house type semi-detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "semi-detached house"
            },
            new Epc
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.SemiDetached
            }),
        new(
            "Can parse house type mid-terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "mid-terrace house"
            },
            new Epc
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.Terraced
            }),
        new(
            "Can parse house type end terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "end-terrace house"
            },
            new Epc
            {
                PropertyType = PropertyType.House,
                HouseType = HouseType.EndTerrace
            }),
    };
    
    private static EpcTestCase[] BungalowTypeTestCases =
    {
        new(
            "Can handle null bungalow type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = null
            },
            new Epc
            {
                BungalowType = null
            }),
        new(
            "Can parse bungalow type detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "detached bungalow"
            },
            new Epc
            {
                PropertyType = PropertyType.Bungalow,
                BungalowType = BungalowType.Detached
            }),
        new(
            "Can parse bungalow type semi-detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "semi-detached bungalow"
            },
            new Epc
            {
                PropertyType = PropertyType.Bungalow,
                BungalowType = BungalowType.SemiDetached
            }),
        new(
            "Can parse bungalow type mid-terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "mid-terrace bungalow"
            },
            new Epc
            {
                PropertyType = PropertyType.Bungalow,
                BungalowType = BungalowType.Terraced
            }),
        new(
            "Can parse bungalow type end terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "end-terrace bungalow"
            },
            new Epc
            {
                PropertyType = PropertyType.Bungalow,
                BungalowType = BungalowType.EndTerrace
            }),
    };
    
    private static EpcTestCase[] FlatTypeTestCases =
    {
        new(
            "Can handle null flat type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = null
            },
            new Epc
            {
                FlatType = null
            }),
        new(
            "Can parse flat type detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "basement flat"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.GroundFloor
            }),
        new(
            "Can parse flat type semi-detached",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "ground flat"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.GroundFloor
            }),
        new(
            "Can parse flat type mid-terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "mid flat"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.MiddleFloor
            }),
        new(
            "Can parse flat type end terrace",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                PropertyType = "top flat"
            },
            new Epc
            {
                PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                FlatType = FlatType.TopFloor
            }),
    };
    
    private static EpcTestCase[] WallConstructionTestCases =
    {
        new(
            "Can handle null wall construction",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = null
            },
            new Epc
            {
                WallConstruction = null
            }),
        new(
            "Can parse wall construction solid",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "solid brick"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Solid
            }),
        new(
            "Can parse wall construction cavity",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "cavity wall"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Cavity
            }),
        new(
            "Can parse mixed walls",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "solid brick",
                    "cavity wall"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Mixed
            }),
    };
    
    // TODO: Investigate solid brick outcomes. Sharepoint document only specifies this option.
    private static EpcTestCase[] SolidWallsInsulatedTestCases =
    {
        new(
            "Can handle null solid walls insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = null
            },
            new Epc
            {
                SolidWallsInsulated = null
            }),
        new(
            "Can parse solid walls insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "solid brick, as built, insulated (assumed)"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Solid,
                SolidWallsInsulated = SolidWallsInsulated.All
            })
    };
    
    private static EpcTestCase[] CavityWallsInsulatedTestCases =
    {
        new(
            "Can handle null cavity walls insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = null
            },
            new Epc
            {
                CavityWallsInsulated = null
            }),
        new(
            "Can parse cavity walls insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "cavity wall, as built, insulated (assumed)"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Cavity,
                CavityWallsInsulated = CavityWallsInsulated.All
            }),
        new(
            "Can parse cavity walls partially insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "cavity wall, as built, partial insulation (assumed)"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Cavity,
                CavityWallsInsulated = CavityWallsInsulated.Some
            }),
        new(
            "Can parse cavity walls no insulation",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WallsDescription = new List<string>
                {
                    "cavity wall, as built, no insulation (assumed)"
                }
            },
            new Epc
            {
                WallConstruction = WallConstruction.Cavity,
                CavityWallsInsulated = CavityWallsInsulated.No
            })
    };
    
    // private static EpcTestCase[] TestCases =
    // {
    //     new(
    //         "",
    //         new EpbEpcAssessmentDto
    //         {
    //             AssessmentType = "RdSAP"
    //         },
    //         new Epc
    //         {
    //         }),
    // };

    private static EpcTestCase[] EpcParseTestCases =
        Array.Empty<EpcTestCase>()
            .Concat(AssessmentTypeTestCases)
            .Concat(LodgementDateTestCases)
            .Concat(ConstructionAgeBandTestCases)
            .Concat(PropertyTypeTestCases)
            .Concat(HouseTypeTestCases)
            .Concat(BungalowTypeTestCases)
            .Concat(FlatTypeTestCases)
            .Concat(WallConstructionTestCases)
            .Concat(SolidWallsInsulatedTestCases)
            .Concat(CavityWallsInsulatedTestCases)
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