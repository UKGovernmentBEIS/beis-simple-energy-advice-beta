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
        new (string Descrption, string inputAssessmentType, Epc outputEpc)[]
        {
            ("Can parse an RdSAP assessment", "RdSAP", new Epc()),
            ("Does not parse a SAP assessment", "SAP", null)
        }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = p.inputAssessmentType 
                }, 
                p.outputEpc))
            .ToArray();
    
    private static readonly EpcTestCase[] LodgementDateTestCases =
        new (string Descrption, string inputLodgementDate, int? outputLodgementYear)[]
            {
                ("Can handle null lodgement year", null, null),
                ("Can parse lodgement year", "2012-12-22", 2012),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    LodgementDate = p.inputLodgementDate
                }, 
                new Epc
                {
                    LodgementYear = p.outputLodgementYear
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] ConstructionAgeBandTestCases =
        new (string Descrption, string inputPropertyAgeBand, HomeAge? outputConstructionAgeBand)[]
            {
                ("Can handle null home age", null, null),
                ("Can parse home age before 1900", "before 1900", HomeAge.Pre1900),
                ("Can parse home age band A", "A", HomeAge.Pre1900),
                ("Can parse home age 1900-1929", "1900-1929", HomeAge.From1900To1929),
                ("Can parse home age band B", "B", HomeAge.From1900To1929),
                ("Can parse home age 1930-1949", "1930-1949", HomeAge.From1930To1949),
                ("Can parse home age band C", "C", HomeAge.From1930To1949),
                ("Can parse home age 1950-1966", "1950-1966", HomeAge.From1950To1966),
                ("Can parse home age band D", "D", HomeAge.From1950To1966),
                ("Can parse home age 1967-1975", "1967-1975", HomeAge.From1967To1975),
                ("Can parse home age band E", "E", HomeAge.From1967To1975),
                ("Can parse home age 1976-1982", "1976-1982", HomeAge.From1976To1982),
                ("Can parse home age band F", "F", HomeAge.From1976To1982),
                ("Can parse home age 1983-1990", "1983-1990", HomeAge.From1983To1990),
                ("Can parse home age band G", "G", HomeAge.From1983To1990),
                ("Can parse home age 1991-1995", "1991-1995", HomeAge.From1991To1995),
                ("Can parse home age band H", "H", HomeAge.From1991To1995),
                ("Can parse home age 1996-2002", "1996-2002", HomeAge.From1996To2002),
                ("Can parse home age band I", "I", HomeAge.From1996To2002),
                ("Can parse home age 2003-2006", "2003-2006", HomeAge.From2003To2006),
                ("Can parse home age band J", "J", HomeAge.From2003To2006),
                ("Can parse home age 2007-2011", "2007-2011", HomeAge.From2007To2011),
                ("Can parse home age band K", "K", HomeAge.From2007To2011),
                ("Can parse home age 2012 onwards", "2012 onwards", HomeAge.From2012ToPresent),
                ("Can parse home age band L", "L", HomeAge.From2012ToPresent),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    PropertyAgeBand = p.inputPropertyAgeBand
                }, 
                new Epc
                {
                    ConstructionAgeBand = p.outputConstructionAgeBand
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] PropertyTypeTestCases =
        new (string Descrption, string inputPropertyType, PropertyType? outputPropertyType)[]
            {
                ("Can handle null property type", null, null),
                ("Can parse property type house", "house", PropertyType.House),
                ("Can parse property type bungalow", "bungalow", PropertyType.Bungalow),
                ("Can parse property type flat", "flat", PropertyType.ApartmentFlatOrMaisonette),
                ("Can parse property type maisonette", "maisonette", PropertyType.ApartmentFlatOrMaisonette),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    PropertyType = p.inputPropertyType
                }, 
                new Epc
                {
                    PropertyType = p.outputPropertyType
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] HouseTypeTestCases =
        new (string Descrption, string inputPropertyType, HouseType? outputHouseType)[]
            {
                ("Can parse house type detached", "detached house", HouseType.Detached),
                ("Can parse house type semi-detached", "semi-detached house", HouseType.SemiDetached),
                ("Can parse house type mid-terrace", "mid-terrace house", HouseType.Terraced),
                ("Can parse house type end terrace", "end-terrace house", HouseType.EndTerrace),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    PropertyType = p.inputPropertyType
                }, 
                new Epc
                {
                    PropertyType = PropertyType.House,
                    HouseType = p.outputHouseType
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] BungalowTypeTestCases =
        new (string Descrption, string inputPropertyType, BungalowType? outputBungalowType)[]
            {
                ("Can parse bungalow type detached", "detached bungalow", BungalowType.Detached),
                ("Can parse bungalow type semi-detached", "semi-detached bungalow", BungalowType.SemiDetached),
                ("Can parse bungalow type mid-terrace", "mid-terrace bungalow", BungalowType.Terraced),
                ("Can parse bungalow type end terrace", "end-terrace bungalow", BungalowType.EndTerrace),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    PropertyType = p.inputPropertyType
                }, 
                new Epc
                {
                    PropertyType = PropertyType.Bungalow,
                    BungalowType = p.outputBungalowType
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] FlatTypeTestCases =
        new (string Descrption, string inputPropertyType, FlatType? outputFlatType)[]
            {
                ("Can parse flat type ground flat (basement)", "basement flat", FlatType.GroundFloor),
                ("Can parse flat type ground flat", "ground flat", FlatType.GroundFloor),
                ("Can parse flat type middle flat", "mid flat", FlatType.MiddleFloor),
                ("Can parse flat type top flat", "top flat", FlatType.TopFloor),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    PropertyType = p.inputPropertyType
                }, 
                new Epc
                {
                    PropertyType = PropertyType.ApartmentFlatOrMaisonette,
                    FlatType = p.outputFlatType
                }))
            .ToArray();
    
    private static readonly EpcTestCase[] WallConstructionTestCases =
        new (string Descrption, string[] inputWallsDescription, WallConstruction? outputWallConstruction)[]
            {
                ("Can handle null wall construction", null, null),
                ("Can parse wall construction solid", new[] {"solid brick"}, WallConstruction.Solid),
                ("Can parse wall construction solid (sandstone or limestone)", new[] {"Sandstone or limestone"}, WallConstruction.Solid),
                ("Can parse wall construction solid (granite or whinstone)", new[] {"Granite or whinstone"}, WallConstruction.Solid),
                ("Can parse wall construction mixed walls", new[] {"solid brick", "cavity wall"}, WallConstruction.Mixed),
            }
            .Select(p => new EpcTestCase(
                p.Descrption, 
                new EpbEpcAssessmentDto 
                { 
                    AssessmentType = "RdSAP",
                    WallsDescription = p.inputWallsDescription?.ToList()
                }, 
                new Epc
                {
                    WallConstruction = p.outputWallConstruction
                }))
            .ToArray();

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