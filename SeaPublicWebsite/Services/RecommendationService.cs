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
                //Not all possible measures from the bre api are here yet
                {
                    "A", new Recommendation
                    {
                        Key = RecommendationKey.AddLoftInsulation,
                        Title = "Add some loft insulation",
                        Summary = "Increase the level of insulation in your loft to the recommended level of 300mm"
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
                    "G", new Recommendation
                    {
                        Key = RecommendationKey.UpgradeHeatingControls,
                        Title = "Upgrade your heating controls",
                        Summary = "Fit a programmer, thermostat and thermostatic radiator valves"
                    }
                },
                {
                    "O3", new Recommendation
                    {
                        Key = RecommendationKey.FitNewWindows,
                        Title = "Fit new windows",
                        Summary = "Replace old single glazed windows with new double or triple glazing"
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
            BreRequest request = GenerateRequest(userData);

            string requestString = JsonConvert.SerializeObject(request);
            Console.WriteLine(requestString);
            return BreApi.GetRecommendationsForUserRequest(requestString);
        }

        private static BreRequest GenerateRequest(UserDataModel userData)
        {
            Console.WriteLine(JsonConvert.SerializeObject(userData));

            //ApartmentFlatOrMaisonette is assumed to be Flat
            (int builtForm, int? flatLevel) = GetBuiltFormAndFlatLevel(userData.PropertyType, userData.HouseType,
                userData.BungalowType, userData.FlatType);

            string constructionDate = GetConstructionDate(userData.YearBuilt);

            int wallType = GetWallType(userData.WallConstruction, userData.SolidWallsInsulated,
                userData.CavityWallsInsulated);

            int roofType = GetRoofType(userData.RoofConstruction, userData.RoofInsulated);

            int glazingType = GetGlazingType(userData.GlazingType);

            int heatingFuel = GetHeatingFuel(userData.HeatingType, userData.OtherHeatingType);

            bool? hotWaterCylinder = GetHotWaterCylinder(userData.HasHotWaterCylinder);

            int heatingPatternType = GetHeatingPatternType(userData.HeatingPattern);

            BreRequest request = new()
            {
                postcode = userData.Postcode,
                property_type = ((int) (PropertyTypeEnum) userData.PropertyType).ToString(),
                built_form = builtForm.ToString(),
                flat_level = flatLevel.ToString(),
                construction_date = constructionDate,
                wall_type = wallType,
                //no input for floor_type
                roof_type = roofType,
                glazing_type = glazingType,
                //no input for outdoor heater space
                heating_fuel = heatingFuel.ToString(),
                hot_water_cylinder = hotWaterCylinder,
                occupants = userData.NumberOfOccupants,
                heating_pattern_type = heatingPatternType,
                living_room_temperature = userData.Temperature,

                //set as defaults?
                num_storeys = 1,
                num_bedrooms = 1,
                measures = true,
                measures_package = new[] { "A", "B", "G", "O3", "U" }
            };

            return request;
        }

        private static (int builtForm, int? flatLevel) GetBuiltFormAndFlatLevel(PropertyType? propertyType,
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
                        HouseType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
                        HouseType.Terraced => (int) BuiltFormEnum.MidTerrace,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case PropertyType.Bungalow:
                    builtForm = bungalowType switch
                    {
                        BungalowType.Detached => (int) BuiltFormEnum.Detached,
                        BungalowType.SemiDetached => (int) BuiltFormEnum.SemiDetached,
                        BungalowType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
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
                    builtForm = (int) BuiltFormEnum.MidTerrace;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (builtForm, flatLevel);
        }

        private static string GetConstructionDate(int? yearBuilt)
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

        private static int GetWallType(WallConstruction? wallConstruction, SolidWallsInsulated? solidWallsInsulated,
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
                    //should we prioritise one wall type here?
                    (int) WallTypeEnum.DontKnow,
                WallConstruction.Other => (int) WallTypeEnum.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetRoofType(RoofConstruction? roofConstruction, RoofInsulated? roofInsulated)
        {
            return roofConstruction switch
            {
                RoofConstruction.Flat =>
                    //should we ask about flat roof insulation?
                    (int) RoofTypeEnum.DontKnow,
                RoofConstruction.Mixed =>
                    //should we prioritise one roof type here?
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

        private static int GetGlazingType(GlazingType? glazingType)
        {
            return glazingType switch
            {
                GlazingType.DoNotKnow => (int) GlazingTypeEnum.DontKnow,
                GlazingType.SingleGlazed => (int) GlazingTypeEnum.SingleGlazed,
                //or go triple?
                GlazingType.DoubleOrTripleGlazed => (int) GlazingTypeEnum.DoubleGlazed,
                GlazingType.Both => (int) GlazingTypeEnum.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetHeatingFuel(HeatingType? heatingType, OtherHeatingType? otherHeatingType)
        {
            return heatingType switch
            {
                //check these assignments are correct
                HeatingType.DoNotKnow => (int) HeatingFuelEnum.MainsGas,
                HeatingType.GasBoiler => (int) HeatingFuelEnum.MainsGas,
                HeatingType.OilBoiler => (int) HeatingFuelEnum.HeatingOil,
                HeatingType.LpgBoiler => (int) HeatingFuelEnum.Lpg,
                HeatingType.Storage => (int) HeatingFuelEnum.Electricity,
                HeatingType.DirectActionElectric => (int) HeatingFuelEnum.Electricity,
                HeatingType.HeatPump => (int) HeatingFuelEnum.Electricity,
                HeatingType.Other => otherHeatingType switch
                {
                    OtherHeatingType.Biomass => (int) HeatingFuelEnum.MainsGas,
                    OtherHeatingType.CoalOrSolidFuel => (int) HeatingFuelEnum.SolidFuel,
                    OtherHeatingType.Other => (int) HeatingFuelEnum.MainsGas,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool? GetHotWaterCylinder(HasHotWaterCylinder? hasHotWaterCylinder)
        {
            return hasHotWaterCylinder switch
            {
                HasHotWaterCylinder.DoNotKnow => null,
                HasHotWaterCylinder.Yes => true,
                HasHotWaterCylinder.No => false,
                _ => null
            };
        }

        private static int GetHeatingPatternType(HeatingPattern? heatingPattern)
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