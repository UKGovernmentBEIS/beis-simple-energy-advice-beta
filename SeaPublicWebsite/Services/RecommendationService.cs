using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Services
{
    public static class RecommendationService
    {
        public static readonly Dictionary<string, Recommendation> RecommendationDictionary =
            new()
            {
                {
                    "A", new Recommendation
                    {
                        Key = RecommendationKey.AddLoftInsulation,
                        Title = "Add some loft insulation",
                        Summary = "Increase the level of insulation in your loft to the recommended level of 300mm"
                    }
                },
                {
                    "A2", new Recommendation
                    {
                        Key = RecommendationKey.FlatRoofInsulation,
                        Title = "Flat roof insulation",
                        Summary = "Under Development"
                    }
                },
                {
                    "B", new Recommendation
                    {
                        Key = RecommendationKey.InsulateCavityWalls,
                        Title = "Insulate your cavity walls",
                        Summary = "Inject insulation into the cavity in your external walls"
                    }
                },
                {
                    "Q", new Recommendation
                    {
                        Key = RecommendationKey.WallInsulationBrickAgeAToD,
                        Title = "Wall insulation Brick age A-D",
                        Summary = "Under Development"
                    }
                },
                {
                    "Q1", new Recommendation
                    {
                        Key = RecommendationKey.WallInsulationOther,
                        Title = "Wall insulation Other",
                        Summary = "Under Development"
                    }
                },
                {
                    "W1", new Recommendation
                    {
                        Key = RecommendationKey.FloorInsulationSuspendedFloor,
                        Title = "Floor insulation suspended floor",
                        Summary = "Under Development"
                    }
                },
                {
                    "W2", new Recommendation
                    {
                        Key = RecommendationKey.FloorInsulationSolidFloor,
                        Title = "Floor insulation solid floor",
                        Summary = "Under Development"
                    }
                },
                {
                    "D", new Recommendation
                    {
                        Key = RecommendationKey.DraughtproofWindowsAndDoors,
                        Title = "Draughtproof 100% of windows and doors",
                        Summary = "Under Development"
                    }
                },
                {
                    "C", new Recommendation
                    {
                        Key = RecommendationKey.HotWaterCylinderInsulation,
                        Title = "Hot water cylinder insulation",
                        Summary = "Under Development"
                    }
                },
                {
                    "F", new Recommendation
                    {
                        Key = RecommendationKey.HotWaterCylinderThermostat,
                        Title = "Hot water cylinder thermostat",
                        Summary = "Under Development"
                    }
                },
                {
                    "G", new Recommendation
                    {
                        Key = RecommendationKey.UpgradeHeatingControls,
                        Title = "Upgrade your heating controls",
                        Summary = "Fit a programmer, thermostat and thermostatic radiator valves"
                    }
                },
                {
                    "I", new Recommendation
                    {
                        Key = RecommendationKey.ReplaceCondensingBoiler,
                        Title = "Replacement condensing boiler",
                        Summary = "Under Development"
                    }
                },
                {
                    "T", new Recommendation
                    {
                        Key = RecommendationKey.CondensingGasBoiler,
                        Title = "Condensing gas boiler (fuel switch)",
                        Summary = "Under Development"
                    }
                },
                {
                    "L2", new Recommendation
                    {
                        Key = RecommendationKey.HighHeatRetentionStorageHeaters,
                        Title = "High heat retention storage heaters",
                        Summary = "Under Development"
                    }
                },
                {
                    "N", new Recommendation
                    {
                        Key = RecommendationKey.SolarWaterHeating,
                        Title = "Solar water heating",
                        Summary = "Under Development"
                    }
                },
                {
                    "Y", new Recommendation
                    {
                        Key = RecommendationKey.MixerShowerHeatRecoverySystem,
                        Title = "Heat recovery system for mixer showers",
                        Summary = "Under Development"
                    }
                },
                {
                    "O", new Recommendation
                    {
                        Key = RecommendationKey.ReplaceSingleGlazedWindowsWithLowEDoubleGlazing,
                        Title = "Replace single glazed windows with low-E double glazing",
                        Summary = "Under Development"
                    }
                },
                {
                    "O3", new Recommendation
                    {
                        Key = RecommendationKey.ReplaceSingleGlazedWindowsWithDoubleOrTripleGlazing,
                        Title = "Fit new windows",
                        Summary = "Replace old single glazed windows with new double or triple glazing"
                    }
                },
                {
                    "X", new Recommendation
                    {
                        Key = RecommendationKey.HighPerformanceExternalDoors,
                        Title = "High performance external doors",
                        Summary = "Under Development"
                    }
                },
                {
                    "U", new Recommendation
                    {
                        Key = RecommendationKey.SolarElectricPanels,
                        Title = "Fit solar electric panels",
                        Summary = "Install PV panels on your roof to generate electricity"
                    }
                }
            };

        public static List<Recommendation> GetRecommendationsForUser(UserDataModel userData)
        {
            BreRequest request = CreateRequest(userData);

            return BreApi.GetRecommendationsForUserRequest(request).Result;
        }

        private static BreRequest CreateRequest(UserDataModel userData)
        {
            BrePropertyType brePropertyType = GetBrePropertyType(userData.PropertyType);

            BreBuiltForm breBuiltForm = GetBreBuiltForm(userData.PropertyType, userData.HouseType, userData.BungalowType);

            BreFlatLevel? breFlatLevel = GetBreFlatLevel(userData.PropertyType, userData.FlatType);

            string breConstructionDate = GetBreConstructionDate(userData.YearBuilt);

            BreWallType breWallType = GetBreWallType(userData.WallConstruction, userData.SolidWallsInsulated,
                userData.CavityWallsInsulated);

            BreRoofType breRoofType = GetBreRoofType(userData.RoofConstruction, userData.RoofInsulated);

            BreGlazingType breGlazingType = GetBreGlazingType(userData.GlazingType);

            BreHeatingFuel breHeatingFuel = GetBreHeatingFuel(userData.HeatingType, userData.OtherHeatingType);

            bool? breHotWaterCylinder = GetBreHotWaterCylinder(userData.HasHotWaterCylinder);

            BreHeatingPatternType breHeatingPatternType = GetBreHeatingPatternType(userData.HeatingPattern);

            BreRequest request = new()
            {
                postcode = userData.Postcode,
                property_type = ((int) brePropertyType).ToString(),
                built_form = ((int) breBuiltForm).ToString(),
                flat_level = ((int?) breFlatLevel).ToString(),
                construction_date = breConstructionDate,
                wall_type = (int) breWallType,
                //no input for floor_type in BRE API
                roof_type = (int) breRoofType,
                glazing_type = (int) breGlazingType,
                //no input for outdoor heater space in BRE API
                heating_fuel = ((int) breHeatingFuel).ToString(),
                hot_water_cylinder = breHotWaterCylinder,
                occupants = userData.NumberOfOccupants,
                heating_pattern_type = (int) breHeatingPatternType,
                living_room_temperature = userData.Temperature,
                //assumption:
                num_storeys = userData.PropertyType == PropertyType.House ? 2 : 1,
                //assumption:
                num_bedrooms = userData.NumberOfOccupants ?? 1,
                measures = true,
                //measures_package consists of all measures implemented in the BRE API as of May 2021
                measures_package = new[]
                {
                    "A", "A2", "B", "Q", "Q1", "W1", "W2", "D", "C", "F", "G", "I", "T", "L2", "N", "Y", "O", "O3", "X",
                    "U"
                }
            };

            return request;
        }

        private static BrePropertyType GetBrePropertyType(PropertyType? propertyType)
        {
            Console.WriteLine(BrePropertyType.Bungalow);
            return propertyType switch
            {
                PropertyType.House => BrePropertyType.House,
                PropertyType.Bungalow => BrePropertyType.Bungalow,
                // assumption:
                PropertyType.ApartmentFlatOrMaisonette => BrePropertyType.Flat,
                _ => throw new ArgumentNullException()
            };
        }

        private static BreBuiltForm GetBreBuiltForm(PropertyType? propertyType, HouseType? houseType,
            BungalowType? bungalowType)
        {
            return propertyType switch
            {
                PropertyType.House => houseType switch
                {
                    HouseType.Detached => BreBuiltForm.Detached,
                    HouseType.SemiDetached => BreBuiltForm.SemiDetached,
                    //assumption:
                    HouseType.EndTerrace => BreBuiltForm.EndTerrace,
                    //assumption:
                    HouseType.Terraced => BreBuiltForm.MidTerrace,
                    _ => throw new ArgumentOutOfRangeException()
                },
                PropertyType.Bungalow => bungalowType switch
                {
                    BungalowType.Detached => BreBuiltForm.Detached,
                    BungalowType.SemiDetached => BreBuiltForm.SemiDetached,
                    //assumption:
                    BungalowType.EndTerrace => BreBuiltForm.EndTerrace,
                    //assumption:
                    BungalowType.Terraced => BreBuiltForm.MidTerrace,
                    _ => throw new ArgumentOutOfRangeException()
                },
                PropertyType.ApartmentFlatOrMaisonette =>
                    //the BreBuiltForm values don't make sense for flats, but built_form is a required input to the BRE API even when property_type is Flat  so we set MidTerrace as a default value:
                    BreBuiltForm.MidTerrace,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreFlatLevel? GetBreFlatLevel(PropertyType? propertyType, FlatType? flatType)
        {
            switch (propertyType)
            {
                case PropertyType.ApartmentFlatOrMaisonette:
                    return flatType switch
                    {
                        FlatType.TopFloor => BreFlatLevel.TopFloor,
                        FlatType.MiddleFloor => BreFlatLevel.MidFloor,
                        FlatType.GroundFloor => BreFlatLevel.GroundFloor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                default:
                    return null;
            }
        }

        private static string GetBreConstructionDate(int? yearBuilt)
        {
            return yearBuilt switch
            {
                <= 1899 => "A",
                <= 1929 => "B",
                <= 1949 => "C",
                <= 1966 => "D",
                <= 1975 => "E",
                <= 1982 => "F",
                <= 1990 => "G",
                <= 1995 => "H",
                <= 2002 => "I",
                <= 2006 => "J",
                <= 2011 => "K",
                >= 2012 => "L",
                _ => "D"
            };
        }

        private static BreWallType GetBreWallType(WallConstruction? wallConstruction,
            SolidWallsInsulated? solidWallsInsulated,
            CavityWallsInsulated? cavityWallsInsulated)
        {
            return wallConstruction switch
            {
                WallConstruction.DoNotKnow => BreWallType.DontKnow,
                WallConstruction.Solid => solidWallsInsulated switch
                {
                    SolidWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                    SolidWallsInsulated.No => BreWallType.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.All => BreWallType.SolidWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Cavity => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => BreWallType.CavityWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Mixed =>
                    //assumption:
                    BreWallType.DontKnow,
                WallConstruction.Other => BreWallType.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreRoofType GetBreRoofType(RoofConstruction? roofConstruction, RoofInsulated? roofInsulated)
        {
            return roofConstruction switch
            {
                RoofConstruction.Flat =>
                    //assumption:
                    BreRoofType.DontKnow,
                RoofConstruction.Mixed =>
                    //assumption:
                    BreRoofType.DontKnow,
                RoofConstruction.Pitched => roofInsulated switch
                {
                    RoofInsulated.DoNotKnow => BreRoofType.DontKnow,
                    RoofInsulated.Yes => BreRoofType.PitchedRoofWithInsulation,
                    RoofInsulated.No => BreRoofType.PitchedRoofWithoutInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreGlazingType GetBreGlazingType(GlazingType? glazingType)
        {
            return glazingType switch
            {
                GlazingType.DoNotKnow => BreGlazingType.DontKnow,
                GlazingType.SingleGlazed => BreGlazingType.SingleGlazed,
                //assumption:
                GlazingType.DoubleOrTripleGlazed => BreGlazingType.DoubleGlazed,
                GlazingType.Both => BreGlazingType.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreHeatingFuel GetBreHeatingFuel(HeatingType? heatingType, OtherHeatingType? otherHeatingType)
        {
            return heatingType switch
            {
                //assumption:
                HeatingType.DoNotKnow => BreHeatingFuel.MainsGas,
                HeatingType.GasBoiler => BreHeatingFuel.MainsGas,
                HeatingType.OilBoiler => BreHeatingFuel.HeatingOil,
                HeatingType.LpgBoiler => BreHeatingFuel.Lpg,
                HeatingType.Storage => BreHeatingFuel.Electricity,
                HeatingType.DirectActionElectric => BreHeatingFuel.Electricity,
                HeatingType.HeatPump => BreHeatingFuel.Electricity,
                HeatingType.Other => otherHeatingType switch
                {
                    //assumption:
                    OtherHeatingType.Biomass => BreHeatingFuel.SolidFuel,
                    OtherHeatingType.CoalOrSolidFuel => BreHeatingFuel.SolidFuel,
                    //assumption:
                    OtherHeatingType.Other => BreHeatingFuel.MainsGas,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool? GetBreHotWaterCylinder(HasHotWaterCylinder? hasHotWaterCylinder)
        {
            return hasHotWaterCylinder switch
            {
                HasHotWaterCylinder.DoNotKnow => null,
                HasHotWaterCylinder.Yes => true,
                HasHotWaterCylinder.No => false,
                _ => null
            };
        }

        private static BreHeatingPatternType GetBreHeatingPatternType(HeatingPattern? heatingPattern)
        {
            return heatingPattern switch
            {
                HeatingPattern.AllDayAndNight => BreHeatingPatternType.AllDayAndAllNight,
                HeatingPattern.AllDayNotNight => BreHeatingPatternType.AllDayButOffAtNight,
                HeatingPattern.MorningAndEvening => BreHeatingPatternType.MorningAndEvening,
                HeatingPattern.OnceADay => BreHeatingPatternType.JustOnceADay,
                HeatingPattern.Other => BreHeatingPatternType.NoneOfTheAbove,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}