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

        public static BreRequest GenerateRequest(UserDataModel userData)
        {
            Console.WriteLine(JsonConvert.SerializeObject(userData));

            //ApartmentFlatOrMaisonette is assumed to be Flat
            int propertyType = (int) (PropertyTypeEnum) userData.PropertyType; 
            int? builtForm = null;
            int? flatLevel = null;
            switch (userData.PropertyType)
            {
                case PropertyType.House:
                    builtForm = userData.HouseType switch
                    {
                        HouseType.Detached => (int) BuiltFormEnum.Detached,
                        HouseType.SemiDetached => (int) BuiltFormEnum.SemiDetached,
                        HouseType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
                        HouseType.Terraced => (int) BuiltFormEnum.MidTerrace,
                        _ => null,
                    };
                    break;
                case PropertyType.Bungalow:
                    builtForm = userData.BungalowType switch
                    {
                        BungalowType.Detached => (int) BuiltFormEnum.Detached,
                        BungalowType.SemiDetached => (int) BuiltFormEnum.SemiDetached,
                        BungalowType.EndTerrace => (int) BuiltFormEnum.EndTerrace,
                        BungalowType.Terraced => (int) BuiltFormEnum.MidTerrace,
                        _ => null
                    };
                    break;
                case PropertyType.ApartmentFlatOrMaisonette:
                    flatLevel = userData.FlatType switch
                    {
                        FlatType.TopFloor => (int) FlatLevelEnum.TopFloor,
                        FlatType.MiddleFloor => (int) FlatLevelEnum.MidFloor,
                        FlatType.GroundFloor => (int) FlatLevelEnum.GroundFloor,
                        _ => null
                    };
                    builtForm = (int) BuiltFormEnum.MidTerrace;
                    break;
            }
            BreRequest request = new()
            {
                property_type = propertyType.ToString(),
                built_form = builtForm.ToString(),
                flat_level = flatLevel.ToString(),
                construction_date = "E",
                wall_type = 1,
                //no input for floor_type
                roof_type = 1,
                glazing_type = 1,
                //no input for outdoor heater space
                heating_fuel = "26",
                hot_water_cylinder = false,
                occupants = 2,
                heating_pattern_type = 1,
                living_room_temperature = 22,
                num_storeys = 1,
                num_bedrooms = 1,
                measures = true,
                measures_package = new[] { "A", "B", "G", "O3", "U" }
            };
            Console.WriteLine(request.property_type);
            Console.WriteLine(request.built_form);
            Console.WriteLine(request.flat_level);
            
            BreRequest exampleCase3Request = new()
            {
                epc = new RequestEpc
                {
                    postcode = "A12 3BC",
                    isConnectedtoMainsGas = true,
                    propertyType = "house",
                    builtForm = "mid-terrace",
                    floorLevel = null,
                    heatLossCorridor = "no corridor",
                    numberHabitableRooms = 7,
                    totalFloorArea = "151.26",
                    wallsDescription = "Solid brick, as built, no insulation (assumed)",
                    roofDescription = "Pitched, no insulation (assumed)",
                    multiGlazeProportion = "30",
                    glazedArea = "Normal",
                    mainheatDescription = "boiler and radiators, mains gas",
                    mainheatcontDescription = "Programmer, no room thermostat",
                    numberOpenFireplaces = "1",
                    lowEnergyLighting = "80",
                    solarWaterHeatingFlag = "N",
                    photoSupply = "0",
                    windTurbineCount = "0"
                },
                property_type = "0",
                num_bedrooms = 1,
                built_form = "1",
                construction_date = "C",
                heating_fuel = "26",
                electricity_tariff = 1,
                num_storeys = 2,
                hot_water_cylinder = true,
                condensing_boiler = true,
                heating_pattern_type = 3,
                normal_days_off_hours = new[] { 7, 9 },
                living_room_temperature = 22,
                showers_per_week = 0,
                baths_per_week = 7,
                measures = true,
                measures_package = new[] { "A", "W1", "G", "U" }
            };

            return request;
        }
    }
}