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

    private static readonly EpcTestCase[] AssessmentTypeTestCases =
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

    private static readonly EpcTestCase[] LodgementDateTestCases =
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
    
    private static readonly EpcTestCase[] ConstructionAgeBandTestCases =
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

    private static readonly EpcTestCase[] PropertyTypeTestCases =
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

    private static readonly EpcTestCase[] HouseTypeTestCases =
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
    
    private static readonly EpcTestCase[] BungalowTypeTestCases =
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
    
    private static readonly EpcTestCase[] FlatTypeTestCases =
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
    
    private static readonly EpcTestCase[] WallConstructionTestCases =
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
            "Can parse wall construction mixed walls",
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
    private static readonly EpcTestCase[] SolidWallsInsulatedTestCases =
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
    
    private static readonly EpcTestCase[] CavityWallsInsulatedTestCases =
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
    
    private static readonly EpcTestCase[] FloorConstructionTestCases =
    {
        new(
            "Can handle null floor construction",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = null
            },
            new Epc
            {
                FloorConstruction = null
            }),
        new(
            "Can parse floor construction suspended timber",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = new List<string>
                {
                    "suspended"
                }
            },
            new Epc
            {
                FloorConstruction = FloorConstruction.SuspendedTimber
            }),
        new(
            "Can parse floor construction solid concrete",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = new List<string>
                {
                    "solid"
                }
            },
            new Epc
            {
                FloorConstruction = FloorConstruction.SolidConcrete
            }),
        new(
            "Can parse floor construction mixed",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = new List<string>
                {
                    "suspended",
                    "solid"
                }
            },
            new Epc
            {
                FloorConstruction = FloorConstruction.Mix
            })
    };
    
    private static readonly EpcTestCase[] FloorInsulationTestCases =
    {
        new(
            "Can handle null floor insulation",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = null
            },
            new Epc
            {
                FloorInsulated = null
            }),
        new(
            "Can parse floor insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = new List<string>
                {
                    "solid, insulated"
                }
            },
            new Epc
            {
                FloorConstruction = FloorConstruction.SolidConcrete,
                FloorInsulated = FloorInsulated.Yes
            }),
        new(
            "Can parse floor uninsulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                FloorDescription = new List<string>
                {
                    "solid, no insulation (assumed)"
                }
            },
            new Epc
            {
                FloorConstruction = FloorConstruction.SolidConcrete,
                FloorInsulated = FloorInsulated.No
            }),
    };
    
    private static readonly EpcTestCase[] RoofConstructionTestCases =
    {
        new(
            "Can handle null roof construction",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = null
            },
            new Epc
            {
                RoofConstruction = null
            }),
        new(
            "Can parse roof construction pitched",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "pitched"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Pitched
            }),
        new(
            "Can parse roof construction flat",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "flat"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Flat
            }),
        new(
            "Can parse roof construction mixed",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "pitched",
                    "flat"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Mixed
            }),
    };
    
    private static readonly EpcTestCase[] RoofInsulationTestCases =
    {
        new(
            "Can handle null roof insulation",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = null
            },
            new Epc
            {
                RoofInsulated = null
            }),
        new(
            "Can parse roof insulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "flat, insulated"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Flat,
                RoofInsulated = RoofInsulated.Yes
            }),
        new(
            "Can parse roof insulated >= 200mm",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "flat, 200 mm loft insulation"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Flat,
                RoofInsulated = RoofInsulated.Yes
            }),
        new(
            "Can parse roof uninsulated",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "flat, no insulation"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Flat,
                RoofInsulated = RoofInsulated.No
            }),
        new(
            "Can parse roof uninsulated < 200mm",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                RoofDescription = new List<string>
                {
                    "flat, 150 mm loft insulation"
                }
            },
            new Epc
            {
                RoofConstruction = RoofConstruction.Flat,
                RoofInsulated = RoofInsulated.No
            }),
    };
    
    private static readonly EpcTestCase[] GlazingTypeTestCases =
    {
        new(
            "Can handle null glazing type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = null
            },
            new Epc
            {
                GlazingType = null
            }),
        new(
            "Can parse glazing type single glazing",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "single glazed"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.SingleGlazed
            }),
        new(
            "Can parse glazing type both (some)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "some double glazing"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.Both
            }),
        new(
            "Can parse glazing type both (partial)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "partial triple glazing"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.Both
            }),
        new(
            "Can parse glazing type both (mostly)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "mostly multiple glazing"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.Both
            }),
        new(
            "Can parse glazing type both (throughout)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "multiple glazing throughout"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.Both
            }),
        new(
            "Can parse glazing type double or triple (full)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "fully double glazed"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.DoubleOrTripleGlazed
            }),
        new(
            "Can parse glazing type double or triple (high)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                WindowsDescription = new List<string>
                {
                    "High performance glazing"
                }
            },
            new Epc
            {
                GlazingType = GlazingType.DoubleOrTripleGlazed
            })
    };
    
    private static readonly EpcTestCase[] HeatingTypeTestCases =
    {
        new(
            "Can handle null heating type",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = null
            },
            new Epc
            {
                EpcHeatingType = null
            }),
        new(
            "Can parse heating type other (community)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "electricity (community)"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Other
            }),
        new(
            "Can parse heating type other (bioethanol)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "bioethanol"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Other
            }),
        new(
            "Can parse heating type other (biodiesel)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "biodiesel from any biomass source"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Other
            }),
        new(
            "Can parse heating type other (waste combustion)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "waste combustion"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Other
            }),
        new(
            "Can parse heating type other (wood pellets in bags for secondary heating)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "wood pellets in bags for secondary heating"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Other
            }),
        new(
            "Can parse heating type gas boiler (mains gas)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "mains gas (not community)"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.GasBoiler
            }),
        new(
            "Can parse heating type gas boiler (biogas)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "biogas - landfill"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.GasBoiler
            }),
        new(
            "Can parse heating type LPG boiler",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "LPG special condition"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.LpgBoiler
            }),
        new(
            "Can parse heating type oil boiler (oil)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "appliances able to use mineral oil or liquid biofuel"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.OilBoiler
            }),
        new(
            "Can parse heating type oil boiler (B30K)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "B30K (not community)"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.OilBoiler
            }),
        new(
            "Can parse heating type coal or solid fuel (coal)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "smokeless coal"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.CoalOrSolidFuel
            }),
        new(
            "Can parse heating type coal or solid fuel (anthracite)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "anthracite"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.CoalOrSolidFuel
            }),
        new(
            "Can parse heating type biomass boiler (wood)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "bulk wood pellets"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Biomass
            }),
        new(
            "Can parse heating type biomass boiler (biomass)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "biomass"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Biomass
            }),
        new(
            "Can parse heating type direct action electric",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "electricity",
                MainHeatingDescription = "electric underfloor heating"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.DirectActionElectric
            }),
        new(
            "Can parse heating type heat pump",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "electricity",
                MainHeatingDescription = "heat pump with warm air distribution"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.HeatPump
            }),
        new(
            "Can parse heating type storage heater",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                MainFuelType = "electricity",
                MainHeatingDescription = "electric storage heaters"
            },
            new Epc
            {
                EpcHeatingType = EpcHeatingType.Storage
            })
    };
    
    private static readonly EpcTestCase[] HasHotWaterCylinderTestCases =
    {
        new(
            "Can handle null has hot water cylinder",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                HasHotWaterCylinder = null
            },
            new Epc
            {
                HasHotWaterCylinder = null
            }),
        new(
            "Can parse has hot water cylinder (true)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                HasHotWaterCylinder = true
            },
            new Epc
            {
                HasHotWaterCylinder = HasHotWaterCylinder.Yes
            }),
        new(
            "Can parse has hot water cylinder (false)",
            new EpbEpcAssessmentDto
            {
                AssessmentType = "RdSAP",
                HasHotWaterCylinder = false
            },
            new Epc
            {
                HasHotWaterCylinder = HasHotWaterCylinder.No
            }),
    };

    private static readonly EpcTestCase[] EpcParseTestCases =
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
            .Concat(FloorConstructionTestCases)
            .Concat(FloorInsulationTestCases)
            .Concat(RoofConstructionTestCases)
            .Concat(RoofInsulationTestCases)
            .Concat(GlazingTypeTestCases)
            .Concat(HeatingTypeTestCases)
            .Concat(HasHotWaterCylinderTestCases)
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