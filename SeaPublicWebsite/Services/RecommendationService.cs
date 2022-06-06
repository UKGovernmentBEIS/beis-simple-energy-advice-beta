using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices.Bre;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Models.EnergyEfficiency;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Services
{
    public class RecommendationService
    {
        private readonly BreApi breApi;

        public RecommendationService(BreApi breApi)
        {
            this.breApi = breApi;
        }

        public static readonly Dictionary<string, BreRecommendation> RecommendationDictionary =
            new()
            {
                {
                    "A", new BreRecommendation
                    {
                        Key = RecommendationKey.AddLoftInsulation,
                        Title = "Add some loft insulation",
                        Summary = "Increase the level of insulation in your loft to the recommended level of 300mm"
                    }
                },
                {
                    "A2", new BreRecommendation
                    {
                        Key = RecommendationKey.FlatRoofInsulation,
                        Title = "Flat roof insulation",
                        Summary = "Under Development"
                    }
                },
                {
                    "B", new BreRecommendation
                    {
                        Key = RecommendationKey.InsulateCavityWalls,
                        Title = "Insulate your cavity walls",
                        Summary = "Inject insulation into the cavity in your external walls"
                    }
                },
                {
                    "Q", new BreRecommendation
                    {
                        Key = RecommendationKey.WallInsulationBrickAgeAToD,
                        Title = "Wall insulation Brick age A-D",
                        Summary = "Under Development"
                    }
                },
                {
                    "Q1", new BreRecommendation
                    {
                        Key = RecommendationKey.WallInsulationOther,
                        Title = "Wall insulation Other",
                        Summary = "Under Development"
                    }
                },
                {
                    "W1", new BreRecommendation
                    {
                        Key = RecommendationKey.FloorInsulationSuspendedFloor,
                        Title = "Floor insulation suspended floor",
                        Summary = "Under Development"
                    }
                },
                {
                    "W2", new BreRecommendation
                    {
                        Key = RecommendationKey.FloorInsulationSolidFloor,
                        Title = "Floor insulation solid floor",
                        Summary = "Under Development"
                    }
                },
                {
                    "D", new BreRecommendation
                    {
                        Key = RecommendationKey.DraughtproofWindowsAndDoors,
                        Title = "Draughtproof 100% of windows and doors",
                        Summary = "Under Development"
                    }
                },
                {
                    "C", new BreRecommendation
                    {
                        Key = RecommendationKey.HotWaterCylinderInsulation,
                        Title = "Hot water cylinder insulation",
                        Summary = "Under Development"
                    }
                },
                {
                    "F", new BreRecommendation
                    {
                        Key = RecommendationKey.HotWaterCylinderThermostat,
                        Title = "Hot water cylinder thermostat",
                        Summary = "Under Development"
                    }
                },
                {
                    "G", new BreRecommendation
                    {
                        Key = RecommendationKey.UpgradeHeatingControls,
                        Title = "Upgrade your heating controls",
                        Summary = "Fit a programmer, thermostat and thermostatic radiator valves"
                    }
                },
                {
                    "I", new BreRecommendation
                    {
                        Key = RecommendationKey.ReplaceCondensingBoiler,
                        Title = "Replacement condensing boiler",
                        Summary = "Under Development"
                    }
                },
                {
                    "T", new BreRecommendation
                    {
                        Key = RecommendationKey.CondensingGasBoiler,
                        Title = "Condensing gas boiler (fuel switch)",
                        Summary = "Under Development"
                    }
                },
                {
                    "L2", new BreRecommendation
                    {
                        Key = RecommendationKey.HighHeatRetentionStorageHeaters,
                        Title = "High heat retention storage heaters",
                        Summary = "Under Development"
                    }
                },
                {
                    "N", new BreRecommendation
                    {
                        Key = RecommendationKey.SolarWaterHeating,
                        Title = "Solar water heating",
                        Summary = "Under Development"
                    }
                },
                {
                    "Y", new BreRecommendation
                    {
                        Key = RecommendationKey.MixerShowerHeatRecoverySystem,
                        Title = "Heat recovery system for mixer showers",
                        Summary = "Under Development"
                    }
                },
                {
                    "O", new BreRecommendation
                    {
                        Key = RecommendationKey.ReplaceSingleGlazedWindowsWithLowEDoubleGlazing,
                        Title = "Replace single glazed windows with low-E double glazing",
                        Summary = "Under Development"
                    }
                },
                {
                    "O3", new BreRecommendation
                    {
                        Key = RecommendationKey.ReplaceSingleGlazedWindowsWithDoubleOrTripleGlazing,
                        Title = "Fit new windows",
                        Summary = "Replace old single glazed windows with new double or triple glazing"
                    }
                },
                {
                    "X", new BreRecommendation
                    {
                        Key = RecommendationKey.HighPerformanceExternalDoors,
                        Title = "High performance external doors",
                        Summary = "Under Development"
                    }
                },
                {
                    "U", new BreRecommendation
                    {
                        Key = RecommendationKey.SolarElectricPanels,
                        Title = "Fit solar electric panels",
                        Summary = "Install PV panels on your roof to generate electricity"
                    }
                }
            };

        public async Task<List<BreRecommendation>> GetRecommendationsForUserAsync(UserDataModel userData)
        {
            BreRequest request = CreateRequest(userData);

            return await breApi.GetRecommendationsForUserRequestAsync(request);
        }

        private static BreRequest CreateRequest(UserDataModel userData)
        {
            BrePropertyType brePropertyType = GetBrePropertyType(userData.PropertyType.Value);

            BreBuiltForm breBuiltForm =
                GetBreBuiltForm(userData.PropertyType.Value, userData.HouseType, userData.BungalowType);

            BreFlatLevel? breFlatLevel = GetBreFlatLevel(userData.PropertyType.Value, userData.FlatType);

            string breConstructionDate = GetBreConstructionDate(userData.YearBuilt);

            BreWallType breWallType = GetBreWallType(userData.WallConstruction.Value, userData.SolidWallsInsulated,
                userData.CavityWallsInsulated);

            BreRoofType? breRoofType = GetBreRoofType(userData.RoofConstruction, userData.AccessibleLoftSpace,
                userData.RoofInsulated);

            BreGlazingType breGlazingType = GetBreGlazingType(userData.GlazingType.Value);

            BreHeatingFuel breHeatingFuel = GetBreHeatingFuel(userData.HeatingType.Value, userData.OtherHeatingType);

            bool? breHotWaterCylinder = GetBreHotWaterCylinder(userData.HasHotWaterCylinder);

            BreHeatingPatternType breHeatingPatternType = GetBreHeatingPatternType(userData.HeatingPattern.Value);

            decimal[] breNormalDaysOffHours =
                GetBreNormalDaysOffHours(userData.HoursOfHeatingMorning, userData.HoursOfHeatingEvening);

            BreRequest request = new(
                brePostcode: userData.Postcode,
                brePropertyType: brePropertyType,
                breBuiltForm: breBuiltForm,
                breFlatLevel: breFlatLevel,
                breConstructionDate: breConstructionDate,
                breWallType: breWallType,
                breRoofType: breRoofType,
                breGlazingType: breGlazingType,
                breHeatingFuel: breHeatingFuel,
                breHotWaterCylinder: breHotWaterCylinder,
                breOccupants: userData.NumberOfOccupants,
                breHeatingPatternType: breHeatingPatternType,
                breNormalDaysOffHours: breNormalDaysOffHours,
                breTemperature: userData.Temperature
            );

            return request;
        }

        private static BrePropertyType GetBrePropertyType(PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.House => BrePropertyType.House,
                PropertyType.Bungalow => BrePropertyType.Bungalow,
                // assumption:
                PropertyType.ApartmentFlatOrMaisonette => BrePropertyType.Flat,
                _ => throw new ArgumentNullException()
            };
        }

        private static BreBuiltForm GetBreBuiltForm(PropertyType propertyType, HouseType? houseType,
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
                    //the BreBuiltForm values don't make sense for flats, but built_form is a required input to the
                    //BRE API even when property_type is Flat  so we set EnclosedEndTerrace as a default value
                    //(this value indicates two adjacent exposed walls which seems a good average for a flat):
                    BreBuiltForm.EnclosedEndTerrace,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreFlatLevel? GetBreFlatLevel(PropertyType propertyType, FlatType? flatType)
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
                //assumption:
                _ => "D"
            };
        }

        private static BreWallType GetBreWallType(WallConstruction wallConstruction,
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
                    //assumption:
                    SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.All => BreWallType.SolidWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Cavity => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    //assumption:
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => BreWallType.CavityWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Mixed => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => solidWallsInsulated switch
                    {
                        //assumption:
                        SolidWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                        //assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.No => BreWallType.SolidWallsWithoutInsulation,
                        //assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                        //assumption:
                        SolidWallsInsulated.All => BreWallType.DontKnow,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    //assumption:
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    //assumption:
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => solidWallsInsulated switch
                    {
                        //assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.DoNotKnow => BreWallType.SolidWallsWithoutInsulation,
                        //assumption:
                        SolidWallsInsulated.No => BreWallType.SolidWallsWithoutInsulation,
                        //assumption:
                        SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                        //assumption:
                        SolidWallsInsulated.All => BreWallType.SolidWallsWithInsulation,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Other => BreWallType.DontKnow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreRoofType? GetBreRoofType(RoofConstruction? roofConstruction,
            AccessibleLoftSpace? accessibleLoftSpace, RoofInsulated? roofInsulated)
        {
            return roofConstruction switch
            {
                RoofConstruction.Flat =>
                    //assumption:
                    BreRoofType.FlatRoofWithInsulation,
                RoofConstruction.Pitched or RoofConstruction.Mixed => accessibleLoftSpace switch
                {
                    AccessibleLoftSpace.No or AccessibleLoftSpace.DoNotKnow => BreRoofType.DontKnow,
                    AccessibleLoftSpace.Yes => roofInsulated switch
                    {
                        RoofInsulated.DoNotKnow => BreRoofType.DontKnow,
                        //assumption in case RoofConstruction.Mixed:
                        RoofInsulated.Yes => BreRoofType.PitchedRoofWithInsulation,
                        //assumption in case RoofConstruction.Mixed:
                        RoofInsulated.No => BreRoofType.PitchedRoofWithoutInsulation,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                },
                //assumption for ground and middle floor flats
                _ => null
            };
        }

        private static BreGlazingType GetBreGlazingType(GlazingType glazingType)
        {
            return glazingType switch
            {
                GlazingType.DoNotKnow => BreGlazingType.DontKnow,
                GlazingType.SingleGlazed => BreGlazingType.SingleGlazed,
                //assumption:
                GlazingType.DoubleOrTripleGlazed => BreGlazingType.DoubleGlazed,
                //assumption:
                GlazingType.Both => BreGlazingType.SingleGlazed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreHeatingFuel GetBreHeatingFuel(HeatingType heatingType, OtherHeatingType? otherHeatingType)
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
                    OtherHeatingType.Biomass => BreHeatingFuel.MainsGas,
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
                HeatingPattern.Other => BreHeatingPatternType.NoneOfTheAbove,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private static decimal[] GetBreNormalDaysOffHours(decimal? hoursOfHeatingMorning, decimal? hoursOfHeatingEvening)
        {
            if (hoursOfHeatingMorning != null && hoursOfHeatingEvening != null)
            {
                //assumption: time heating is turned on is not collected so this is a simplification of the BRE input complexity available
                decimal averageOffPeriod = (24 - ((decimal) hoursOfHeatingMorning + (decimal) hoursOfHeatingEvening)) / 2;
                return new [] { averageOffPeriod, averageOffPeriod };
            }
            return null;
        }
    }
}