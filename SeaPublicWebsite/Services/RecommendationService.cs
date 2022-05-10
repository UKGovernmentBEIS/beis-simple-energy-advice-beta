using System;
using System.Collections.Generic;
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

            string requestString = JsonConvert.SerializeObject(request);
            return BreApi.GetRecommendationsForUserRequest(requestString);
        }

        private static BreRequest CreateRequest(UserDataModel userData)
        {
            int propertyType = GetConvertedPropertyType(userData.PropertyType);

            (int builtForm, int? flatLevel) = GetConvertedBuiltFormAndFlatLevel(userData.PropertyType,
                userData.HouseType,
                userData.BungalowType, userData.FlatType);

            string constructionDate = GetConvertedConstructionDate(userData.YearBuilt);

            int wallType = GetConvertedWallType(userData.WallConstruction, userData.SolidWallsInsulated,
                userData.CavityWallsInsulated);

            int roofType = GetConvertedRoofType(userData.RoofConstruction, userData.RoofInsulated);

            int glazingType = GetConvertedGlazingType(userData.GlazingType);

            int heatingFuel = GetConvertedHeatingFuel(userData.HeatingType, userData.OtherHeatingType);

            bool? hotWaterCylinder = GetConvertedHotWaterCylinder(userData.HasHotWaterCylinder);

            int heatingPatternType = GetConvertedHeatingPatternType(userData.HeatingPattern);

            BreRequest request = new()
            {
                postcode = userData.Postcode,
                property_type = propertyType.ToString(),
                built_form = builtForm.ToString(),
                flat_level = flatLevel.ToString(),
                construction_date = constructionDate,
                wall_type = wallType,
                //no input for floor_type in BRE API
                roof_type = roofType,
                glazing_type = glazingType,
                //no input for outdoor heater space in BRE API
                heating_fuel = heatingFuel.ToString(),
                hot_water_cylinder = hotWaterCylinder,
                occupants = userData.NumberOfOccupants,
                heating_pattern_type = heatingPatternType,
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

        private static int GetConvertedPropertyType(PropertyType? propertyType)
        {
            //ApartmentFlatOrMaisonette is assumed to be a Flat
            if (propertyType != null) return (int) (PropertyTypeEnum) propertyType;
            throw new ArgumentNullException();
        }

        private static (int builtForm, int? flatLevel) GetConvertedBuiltFormAndFlatLevel(PropertyType? propertyType,
            HouseType? houseType, BungalowType? bungalowType, FlatType? flatType)
        {
            int builtForm;
            int? flatLevel = null;
            switch (propertyType)
            {
                case PropertyType.House:
                    builtForm = houseType switch
                    {
                        HouseType.Detached => (int) BuiltFormEnum.Detached,
                        HouseType.SemiDetached => (int) BuiltFormEnum.SemiDetached,
                        //assumption:
                        HouseType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
                        //assumption:
                        HouseType.Terraced => (int) BuiltFormEnum.MidTerrace,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case PropertyType.Bungalow:
                    builtForm = bungalowType switch
                    {
                        BungalowType.Detached => (int) BuiltFormEnum.Detached,
                        BungalowType.SemiDetached => (int) BuiltFormEnum.SemiDetached,
                        //assumption:
                        BungalowType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
                        //assumption:
                        BungalowType.Terraced => (int) BuiltFormEnum.MidTerrace,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case PropertyType.ApartmentFlatOrMaisonette:
                    flatLevel = flatType switch
                    {
                        FlatType.TopFloor => (int) FlatLevelEnum.TopFloor,
                        FlatType.MiddleFloor => (int) FlatLevelEnum.MidFloor,
                        FlatType.GroundFloor => (int) FlatLevelEnum.GroundFloor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    //assumption:
                    builtForm = (int) BuiltFormEnum.MidTerrace;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (builtForm, flatLevel);
        }

        private static string GetConvertedConstructionDate(int? yearBuilt)
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

        private static int GetConvertedWallType(WallConstruction? wallConstruction,
            SolidWallsInsulated? solidWallsInsulated,
            CavityWallsInsulated? cavityWallsInsulated)
        {
            return wallConstruction switch
            {
                WallConstruction.DoNotKnow => (int) WallTypeEnum.DontKnow,
                WallConstruction.Solid => solidWallsInsulated switch
                {
                    SolidWallsInsulated.DoNotKnow => (int) WallTypeEnum.DontKnow,
                    SolidWallsInsulated.No => (int) WallTypeEnum.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.Some => (int) WallTypeEnum.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.All => (int) WallTypeEnum.SolidWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Cavity => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => (int) WallTypeEnum.DontKnow,
                    CavityWallsInsulated.No => (int) WallTypeEnum.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.Some => (int) WallTypeEnum.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => (int) WallTypeEnum.CavityWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Mixed =>
                    //assumption:
                    (int) WallTypeEnum.DontKnow,
                WallConstruction.Other => (int) WallTypeEnum.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetConvertedRoofType(RoofConstruction? roofConstruction, RoofInsulated? roofInsulated)
        {
            return roofConstruction switch
            {
                RoofConstruction.Flat =>
                    //assumption:
                    (int) RoofTypeEnum.DontKnow,
                RoofConstruction.Mixed =>
                    //assumption:
                    (int) RoofTypeEnum.DontKnow,
                RoofConstruction.Pitched => roofInsulated switch
                {
                    RoofInsulated.DoNotKnow => (int) RoofTypeEnum.DontKnow,
                    RoofInsulated.Yes => (int) RoofTypeEnum.PitchedRoofWithInsulation,
                    RoofInsulated.No => (int) RoofTypeEnum.PitchedRoofWithoutInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetConvertedGlazingType(GlazingType? glazingType)
        {
            return glazingType switch
            {
                GlazingType.DoNotKnow => (int) GlazingTypeEnum.DontKnow,
                GlazingType.SingleGlazed => (int) GlazingTypeEnum.SingleGlazed,
                //assumption:
                GlazingType.DoubleOrTripleGlazed => (int) GlazingTypeEnum.DoubleGlazed,
                GlazingType.Both => (int) GlazingTypeEnum.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetConvertedHeatingFuel(HeatingType? heatingType, OtherHeatingType? otherHeatingType)
        {
            return heatingType switch
            {
                //assumption:
                HeatingType.DoNotKnow => (int) HeatingFuelEnum.MainsGas,
                HeatingType.GasBoiler => (int) HeatingFuelEnum.MainsGas,
                HeatingType.OilBoiler => (int) HeatingFuelEnum.HeatingOil,
                HeatingType.LpgBoiler => (int) HeatingFuelEnum.Lpg,
                HeatingType.Storage => (int) HeatingFuelEnum.Electricity,
                HeatingType.DirectActionElectric => (int) HeatingFuelEnum.Electricity,
                HeatingType.HeatPump => (int) HeatingFuelEnum.Electricity,
                HeatingType.Other => otherHeatingType switch
                {
                    //assumption:
                    OtherHeatingType.Biomass => (int) HeatingFuelEnum.SolidFuel,
                    OtherHeatingType.CoalOrSolidFuel => (int) HeatingFuelEnum.SolidFuel,
                    //assumption:
                    OtherHeatingType.Other => (int) HeatingFuelEnum.MainsGas,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool? GetConvertedHotWaterCylinder(HasHotWaterCylinder? hasHotWaterCylinder)
        {
            return hasHotWaterCylinder switch
            {
                HasHotWaterCylinder.DoNotKnow => null,
                HasHotWaterCylinder.Yes => true,
                HasHotWaterCylinder.No => false,
                _ => null
            };
        }

        private static int GetConvertedHeatingPatternType(HeatingPattern? heatingPattern)
        {
            return heatingPattern switch
            {
                HeatingPattern.AllDayAndNight => (int) HeatingPatternTypeEnum.AllDayAndAllNight,
                HeatingPattern.AllDayNotNight => (int) HeatingPatternTypeEnum.AllDayButOffAtNight,
                HeatingPattern.MorningAndEvening => (int) HeatingPatternTypeEnum.MorningAndEvening,
                HeatingPattern.OnceADay => (int) HeatingPatternTypeEnum.JustOnceADay,
                HeatingPattern.Other => (int) HeatingPatternTypeEnum.NoneOfTheAbove,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}