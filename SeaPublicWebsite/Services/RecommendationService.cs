﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.ExternalServices.Bre;
using SeaPublicWebsite.ExternalServices.Models;

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
                        Title = "Add some more loft insulation",
                        Summary = "Increase the level of insulation in your loft to the recommended level of 300mm"
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
                        Title = "Insulate the ground floor",
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
                    "L2", new BreRecommendation
                    {
                        Key = RecommendationKey.HighHeatRetentionStorageHeaters,
                        Title = "Upgrade to high heat retention storage heaters",
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

        public async Task<List<BreRecommendation>> GetRecommendationsForPropertyAsync(PropertyData propertyData)
        {
            BreRequest request = CreateRequest(propertyData);

            return await breApi.GetRecommendationsForPropertyRequestAsync(request);
        }

        private static BreRequest CreateRequest(PropertyData propertyData)
        {
            BrePropertyType brePropertyType = GetBrePropertyType(propertyData.PropertyType.Value);

            BreBuiltForm breBuiltForm =
                GetBreBuiltForm(propertyData.PropertyType.Value, propertyData.HouseType, propertyData.BungalowType);

            BreFlatLevel? breFlatLevel = GetBreFlatLevel(propertyData.PropertyType.Value, propertyData.FlatType);

            string breConstructionDate = GetBreConstructionDate(propertyData.YearBuilt);

            BreWallType breWallType = GetBreWallType(propertyData.WallConstruction.Value, propertyData.SolidWallsInsulated,
                propertyData.CavityWallsInsulated);

            BreRoofType? breRoofType = GetBreRoofType(propertyData.RoofConstruction, propertyData.LoftSpace, 
                propertyData.LoftAccess, propertyData.RoofInsulated);

            BreGlazingType breGlazingType = GetBreGlazingType(propertyData.GlazingType.Value);

            BreHeatingFuel breHeatingFuel = GetBreHeatingFuel(propertyData.HeatingType.Value, propertyData.OtherHeatingType);

            bool? breHotWaterCylinder = GetBreHotWaterCylinder(propertyData.HasHotWaterCylinder);

            BreHeatingPatternType breHeatingPatternType = GetBreHeatingPatternType(propertyData.HeatingPattern.Value);

            int[] breNormalDaysOffHours =
                GetBreNormalDaysOffHours(propertyData.HoursOfHeatingMorning, propertyData.HoursOfHeatingEvening);

            BreRequest request = new(
                brePostcode: propertyData.Postcode,
                brePropertyType: brePropertyType,
                breBuiltForm: breBuiltForm,
                breFlatLevel: breFlatLevel,
                breConstructionDate: breConstructionDate,
                breWallType: breWallType,
                breRoofType: breRoofType,
                breGlazingType: breGlazingType,
                breHeatingFuel: breHeatingFuel,
                breHotWaterCylinder: breHotWaterCylinder,
                breOccupants: propertyData.NumberOfOccupants,
                breHeatingPatternType: breHeatingPatternType,
                breNormalDaysOffHours: breNormalDaysOffHours,
                breTemperature: propertyData.Temperature
            );

            return request;
        }

        private static BrePropertyType GetBrePropertyType(PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.House => BrePropertyType.House,
                PropertyType.Bungalow => BrePropertyType.Bungalow,
                // peer-reviewed assumption:
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
                    //peer-reviewed assumption:
                    HouseType.EndTerrace => BreBuiltForm.EndTerrace,
                    //peer-reviewed assumption:
                    HouseType.Terraced => BreBuiltForm.MidTerrace,
                    _ => throw new ArgumentOutOfRangeException()
                },
                PropertyType.Bungalow => bungalowType switch
                {
                    BungalowType.Detached => BreBuiltForm.Detached,
                    BungalowType.SemiDetached => BreBuiltForm.SemiDetached,
                    //peer-reviewed assumption:
                    BungalowType.EndTerrace => BreBuiltForm.EndTerrace,
                    //peer-reviewed assumption:
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
                //peer-reviewed assumption:
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
                    //peer-reviewed assumption:
                    SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                    SolidWallsInsulated.All => BreWallType.SolidWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Cavity => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    //peer-reviewed assumption:
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => BreWallType.CavityWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException()
                },
                WallConstruction.Mixed => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => solidWallsInsulated switch
                    {
                        //peer-reviewed assumption:
                        SolidWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                        //peer-reviewed assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.No => BreWallType.SolidWallsWithoutInsulation,
                        //peer-reviewed assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                        //peer-reviewed assumption:
                        SolidWallsInsulated.All => BreWallType.DontKnow,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    //peer-reviewed assumption:
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    //peer-reviewed assumption:
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => solidWallsInsulated switch
                    {
                        //peer-reviewed assumption (may change to DontKnow if this can drive recommendations):
                        SolidWallsInsulated.DoNotKnow => BreWallType.SolidWallsWithoutInsulation,
                        //peer-reviewed assumption:
                        SolidWallsInsulated.No => BreWallType.SolidWallsWithoutInsulation,
                        //peer-reviewed assumption:
                        SolidWallsInsulated.Some => BreWallType.SolidWallsWithoutInsulation,
                        //peer-reviewed assumption:
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
            LoftSpace? loftSpace, LoftAccess? loftAccess, RoofInsulated? roofInsulated)
        {
            return roofConstruction switch
            {
                RoofConstruction.Flat =>
                    //peer-reviewed assumption:
                    BreRoofType.FlatRoofWithInsulation,
                RoofConstruction.Pitched or RoofConstruction.Mixed => loftSpace switch
                {
                    LoftSpace.No => BreRoofType.DontKnow,
                    LoftSpace.Yes => loftAccess switch
                    {
                        LoftAccess.No => BreRoofType.DontKnow,
                        LoftAccess.Yes => roofInsulated switch
                        {
                            RoofInsulated.DoNotKnow => BreRoofType.DontKnow,
                            //peer-reviewed assumption in case RoofConstruction.Mixed:
                            RoofInsulated.Yes => BreRoofType.PitchedRoofWithInsulation,
                            //peer-reviewed assumption in case RoofConstruction.Mixed:
                            RoofInsulated.No => BreRoofType.PitchedRoofWithoutInsulation,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                },
                //peer-reviewed assumption for ground and middle floor flats
                _ => null
            };
        }

        private static BreGlazingType GetBreGlazingType(GlazingType glazingType)
        {
            return glazingType switch
            {
                GlazingType.DoNotKnow => BreGlazingType.DontKnow,
                GlazingType.SingleGlazed => BreGlazingType.SingleGlazed,
                //peer-reviewed assumption:
                GlazingType.DoubleOrTripleGlazed => BreGlazingType.DoubleGlazed,
                //peer-reviewed assumption:
                GlazingType.Both => BreGlazingType.SingleGlazed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static BreHeatingFuel GetBreHeatingFuel(HeatingType heatingType, OtherHeatingType? otherHeatingType)
        {
            return heatingType switch
            {
                //peer-reviewed assumption:
                HeatingType.DoNotKnow => BreHeatingFuel.MainsGas,
                HeatingType.GasBoiler => BreHeatingFuel.MainsGas,
                HeatingType.OilBoiler => BreHeatingFuel.HeatingOil,
                HeatingType.LpgBoiler => BreHeatingFuel.Lpg,
                HeatingType.Storage => BreHeatingFuel.Electricity,
                HeatingType.DirectActionElectric => BreHeatingFuel.Electricity,
                HeatingType.HeatPump => BreHeatingFuel.Electricity,
                HeatingType.Other => otherHeatingType switch
                {
                    //peer-reviewed assumption:
                    OtherHeatingType.Biomass => BreHeatingFuel.MainsGas,
                    OtherHeatingType.CoalOrSolidFuel => BreHeatingFuel.SolidFuel,
                    //peer-reviewed assumption:
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
        
        private static int[] GetBreNormalDaysOffHours(decimal? hoursOfHeatingMorning, decimal? hoursOfHeatingEvening)
        {
            if (hoursOfHeatingMorning != null && hoursOfHeatingEvening != null)
            {
                //peer-reviewed assumption: time heating is turned on is not collected so this is a simplification of the BRE input complexity available
                int averageOffPeriod = (int) ((24 - (hoursOfHeatingMorning + hoursOfHeatingEvening)) / 2);
                return new [] { averageOffPeriod, averageOffPeriod };
            }
            return null;
        }
    }
}