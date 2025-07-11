﻿using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre.Enums;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.Bre
{
    public interface IRecommendationService
    {
        Task<BreRecommendationsWithPriceCap> GetRecommendationsWithPriceCapForPropertyAsync(PropertyData propertyData);
    }

    public class RecommendationService(BreApi breApi) : IRecommendationService
    {
        public static readonly Dictionary<string, BreRecommendation> RecommendationDictionary =
            new()
            {
                //The title and summary within the resource files have a key format of "Title/Summary" + name of key (+ any other identifier)
                {
                    "A", new BreRecommendation
                    {
                        Key = RecommendationKey.AddLoftInsulation,
                        Title = "Title" + RecommendationKey.AddLoftInsulation,
                        Summary = "Summary" + RecommendationKey.AddLoftInsulation
                    }
                },
                {
                    "A1", new BreRecommendation
                    {
                        Key = RecommendationKey.InsulateYourLoft,
                        Title = "Title" + RecommendationKey.InsulateYourLoft,
                        Summary = "Summary" + RecommendationKey.InsulateYourLoft
                    }
                },
                {
                    "B", new BreRecommendation
                    {
                        Key = RecommendationKey.InsulateCavityWalls,
                        Title = "Title" + RecommendationKey.InsulateCavityWalls,
                        Summary = "Summary" + RecommendationKey.InsulateCavityWalls
                    }
                },
                {
                    "Q", new BreRecommendation
                    {
                        Key = RecommendationKey.InsulateSolidWalls,
                        Title = "Title" + RecommendationKey.InsulateSolidWalls,
                        Summary = "Summary" + RecommendationKey.InsulateSolidWalls
                    }
                },
                {
                    "Q1", new BreRecommendation
                    {
                        Key = RecommendationKey.InsulateSolidWalls,
                        Title = "Title" + RecommendationKey.InsulateSolidWalls + "Other",
                        Summary = "Summary" + RecommendationKey.InsulateSolidWalls + "Other"
                    }
                },
                {
                    "W1", new BreRecommendation
                    {
                        Key = RecommendationKey.FloorInsulationSuspendedFloor,
                        Title = "Title" + RecommendationKey.FloorInsulationSuspendedFloor,
                        Summary = "Summary" + RecommendationKey.FloorInsulationSuspendedFloor
                    }
                },
                {
                    "C", new BreRecommendation
                    {
                        Key = RecommendationKey.HotWaterCylinderInsulation,
                        Title = "Title" + RecommendationKey.HotWaterCylinderInsulation,
                        Summary = "Summary" + RecommendationKey.HotWaterCylinderInsulation
                    }
                },
                {
                    "G", new BreRecommendation
                    {
                        Key = RecommendationKey.UpgradeHeatingControls,
                        Title = "Title" + RecommendationKey.UpgradeHeatingControls,
                        Summary = "Summary" + RecommendationKey.UpgradeHeatingControls
                    }
                },
                {
                    "L2", new BreRecommendation
                    {
                        Key = RecommendationKey.HighHeatRetentionStorageHeaters,
                        Title = "Title" + RecommendationKey.HighHeatRetentionStorageHeaters,
                        Summary = "Summary" + RecommendationKey.HighHeatRetentionStorageHeaters
                    }
                },
                {
                    "O", new BreRecommendation
                    {
                        Key = RecommendationKey.ReplaceSingleGlazedWindowsWithDoubleOrTripleGlazing,
                        Title = "Title" + RecommendationKey.ReplaceSingleGlazedWindowsWithDoubleOrTripleGlazing,
                        Summary = "Summary" + RecommendationKey.ReplaceSingleGlazedWindowsWithDoubleOrTripleGlazing
                    }
                },
                {
                    "U", new BreRecommendation
                    {
                        Key = RecommendationKey.SolarElectricPanels,
                        Title = "Title" + RecommendationKey.SolarElectricPanels,
                        Summary = "Summary" + RecommendationKey.SolarElectricPanels
                    }
                },
                {
                    "Z1", new BreRecommendation
                    {
                        Key = RecommendationKey.InstallHeatPump,
                        Title = "Title" + RecommendationKey.InstallHeatPump,
                        Summary = "Summary" + RecommendationKey.InstallHeatPump
                    }
                }
            };

        public async Task<BreRecommendationsWithPriceCap> GetRecommendationsWithPriceCapForPropertyAsync(PropertyData propertyData)
        {
            BreRequest request = CreateRequest(propertyData);

            return await breApi.GetRecommendationsWithPriceCapForPropertyRequestAsync(request);
        }

        private static BreRequest CreateRequest(PropertyData propertyData)
        {
            var brePropertyType = GetBrePropertyType(propertyData.PropertyType.Value);

            var breBuiltForm =
                GetBreBuiltForm(propertyData.PropertyType.Value, propertyData.HouseType, propertyData.BungalowType);

            var breFlatLevel = GetBreFlatLevel(propertyData.PropertyType.Value, propertyData.FlatType);

            var breConstructionDate = GetBreConstructionDate(propertyData.YearBuilt, propertyData.WallConstruction,
                propertyData.CavityWallsInsulated, propertyData.Epc?.ConstructionAgeBand);

            var breWallType = GetBreWallType(propertyData.WallConstruction.Value,
                propertyData.SolidWallsInsulated,
                propertyData.CavityWallsInsulated);

            var breRoofType = GetBreRoofType(propertyData.RoofConstruction, propertyData.LoftSpace,
                propertyData.LoftAccess, propertyData.RoofInsulated);

            var breGlazingType = GetBreGlazingType(propertyData.GlazingType.Value);

            var breHeatingSystem =
                GetBreHeatingSystem(propertyData.HeatingType.Value, propertyData.OtherHeatingType);

            var breHeatingControls = GetBreHeatingControls(propertyData.HeatingControls);

            var breHotWaterCylinder = GetBreHotWaterCylinder(propertyData.HasHotWaterCylinder);

            var breHeatingPatternType = GetBreHeatingPatternType(propertyData.HeatingPattern.Value,
                propertyData.HoursOfHeatingEvening, propertyData.HoursOfHeatingEvening);

            var breNormalDaysOffHours =
                GetBreNormalDaysOffHours(propertyData.HoursOfHeatingMorning, propertyData.HoursOfHeatingEvening);

            var breFloorType = GetBreFloorType(propertyData.FloorConstruction, propertyData.FloorInsulated);

            var breOutsideSpace = GetBreOutsideSpace(propertyData.HasOutdoorSpace);

            var brePvPanels = GetBrePvPanels(propertyData.SolarElectricPanels);

            BreRequest request = new(
                brePropertyType: brePropertyType,
                breBuiltForm: breBuiltForm,
                breFlatLevel: breFlatLevel,
                breConstructionDate: breConstructionDate,
                breWallType: breWallType,
                breRoofType: breRoofType,
                breGlazingType: breGlazingType,
                breHeatingSystem: breHeatingSystem,
                breHotWaterCylinder: breHotWaterCylinder,
                breOccupants: propertyData.NumberOfOccupants,
                breHeatingPatternType: breHeatingPatternType,
                breNormalDaysOffHours: breNormalDaysOffHours,
                breTemperature: propertyData.Temperature,
                breFloorType: breFloorType,
                breOutsideSpace: breOutsideSpace,
                brePvPanels: brePvPanels,
                breHeatingControls: breHeatingControls
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
                _ => throw new ArgumentNullException(nameof(propertyType))
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
                    _ => throw new ArgumentOutOfRangeException(nameof(houseType))
                },
                PropertyType.Bungalow => bungalowType switch
                {
                    BungalowType.Detached => BreBuiltForm.Detached,
                    BungalowType.SemiDetached => BreBuiltForm.SemiDetached,
                    //peer-reviewed assumption:
                    BungalowType.EndTerrace => BreBuiltForm.EndTerrace,
                    //peer-reviewed assumption:
                    BungalowType.Terraced => BreBuiltForm.MidTerrace,
                    _ => throw new ArgumentOutOfRangeException(nameof(bungalowType))
                },
                PropertyType.ApartmentFlatOrMaisonette =>
                    //the BreBuiltForm values don't make sense for flats, but built_form is a required input to the
                    //BRE API even when property_type is Flat  so we set EnclosedEndTerrace as a default value
                    //(this value indicates two adjacent exposed walls which seems a good average for a flat):
                    BreBuiltForm.EnclosedEndTerrace,
                _ => throw new ArgumentOutOfRangeException(nameof(propertyType))
            };
        }

        private static BreFlatLevel? GetBreFlatLevel(PropertyType propertyType, FlatType? flatType)
        {
            return propertyType switch
            {
                PropertyType.ApartmentFlatOrMaisonette => flatType switch
                {
                    FlatType.TopFloor => BreFlatLevel.TopFloor,
                    FlatType.MiddleFloor => BreFlatLevel.MidFloor,
                    FlatType.GroundFloor => BreFlatLevel.GroundFloor,
                    _ => throw new ArgumentOutOfRangeException(nameof(flatType))
                },
                _ => null
            };
        }

        private static string GetBreConstructionDate(YearBuilt? yearBuilt, WallConstruction? wallConstruction,
            CavityWallsInsulated? cavityWallsInsulated, HomeAge? epcConstructionAgeBand)
        {
            return yearBuilt switch
            {
                YearBuilt.Pre1900 => "A",
                YearBuilt.From1900To1929 or YearBuilt.Pre1930 => "B",
                YearBuilt.From1930To1949 => "C",
                YearBuilt.From1950To1966 or YearBuilt.From1930To1966 => "D",
                YearBuilt.From1967To1975 => "E",
                YearBuilt.From1976To1982 or YearBuilt.From1967To1982 => "F",
                YearBuilt.From1983To1990 => "G",
                YearBuilt.From1991To1995 or YearBuilt.From1983To1995 => "H",
                YearBuilt.From1996To2002 => "I",
                YearBuilt.From2003To2006 => "J",
                YearBuilt.From2007To2011 or YearBuilt.From1996To2011 => "K",
                YearBuilt.From2012ToPresent => "L",
                //peer-reviewed assumptions:
                _ => epcConstructionAgeBand switch
                {
                    HomeAge.Pre1900 => "A",
                    HomeAge.From1900To1929 => "B",
                    HomeAge.From1930To1949 => "C",
                    HomeAge.From1950To1966 => "D",
                    HomeAge.From1967To1975 => "E",
                    HomeAge.From1976To1982 => "F",
                    HomeAge.From1983To1990 => "G",
                    HomeAge.From1991To1995 => "H",
                    HomeAge.From1996To2002 => "I",
                    HomeAge.From2003To2006 => "J",
                    HomeAge.From2007To2011 => "K",
                    HomeAge.From2012ToPresent => "L",
                    _ => wallConstruction switch
                    {
                        WallConstruction.DoNotKnow => "D",
                        WallConstruction.Solid => "B",
                        WallConstruction.Cavity => cavityWallsInsulated switch
                        {
                            CavityWallsInsulated.DoNotKnow => "D",
                            CavityWallsInsulated.No => "D",
                            CavityWallsInsulated.Some => "D",
                            CavityWallsInsulated.All => "I",
                            _ => throw new ArgumentOutOfRangeException(nameof(cavityWallsInsulated))
                        },
                        WallConstruction.Mixed => "B",
                        WallConstruction.Other => "D",
                        _ => throw new ArgumentOutOfRangeException(nameof(wallConstruction))
                    },
                },
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
                    _ => throw new ArgumentOutOfRangeException(nameof(solidWallsInsulated))
                },
                WallConstruction.Cavity => cavityWallsInsulated switch
                {
                    CavityWallsInsulated.DoNotKnow => BreWallType.DontKnow,
                    CavityWallsInsulated.No => BreWallType.CavityWallsWithoutInsulation,
                    //peer-reviewed assumption:
                    CavityWallsInsulated.Some => BreWallType.CavityWallsWithoutInsulation,
                    CavityWallsInsulated.All => BreWallType.CavityWallsWithInsulation,
                    _ => throw new ArgumentOutOfRangeException(nameof(cavityWallsInsulated))
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
                        _ => throw new ArgumentOutOfRangeException(nameof(solidWallsInsulated))
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
                        _ => throw new ArgumentOutOfRangeException(nameof(solidWallsInsulated))
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(cavityWallsInsulated))
                },
                WallConstruction.Other => BreWallType.OtherWallType,
                _ => throw new ArgumentOutOfRangeException(nameof(wallConstruction))
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
                    //peer-reviewed assumption:
                    LoftSpace.No => BreRoofType.PitchedRoofWithInsulation,
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
                            _ => throw new ArgumentOutOfRangeException(nameof(roofInsulated))
                        },
                        _ => throw new ArgumentOutOfRangeException(nameof(loftAccess))
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(roofConstruction))
                },
                //peer-reviewed assumption for ground and middle floor flats
                _ => null
            };
        }

        private static BreGlazingType GetBreGlazingType(GlazingType glazingType)
        {
            return glazingType switch
            {
                //peer-reviewed assumption (BreGlazingType.DontKnow would return recommendation O3 rather than O, which we don't want):
                GlazingType.DoNotKnow => BreGlazingType.SingleGlazed,
                GlazingType.SingleGlazed => BreGlazingType.SingleGlazed,
                //peer-reviewed assumption, this will return recommendation O3, currently not whitelisted in BreRequest.cs:
                GlazingType.DoubleOrTripleGlazed => BreGlazingType.DoubleGlazed,
                //peer-reviewed assumption:
                GlazingType.Both => BreGlazingType.SingleGlazed,
                _ => throw new ArgumentOutOfRangeException(nameof(glazingType))
            };
        }

        private static BreHeatingSystem GetBreHeatingSystem(HeatingType heatingType, OtherHeatingType? otherHeatingType)
        {
            return heatingType switch
            {
                HeatingType.DoNotKnow => BreHeatingSystem.GasBoiler,
                HeatingType.GasBoiler => BreHeatingSystem.GasBoiler,
                HeatingType.OilBoiler => BreHeatingSystem.OilBoiler,
                HeatingType.LpgBoiler => BreHeatingSystem.LpgBoiler,
                HeatingType.Storage => BreHeatingSystem.StorageHeaters,
                HeatingType.DirectActionElectric => BreHeatingSystem.DirectActingElectric,
                HeatingType.HeatPump => BreHeatingSystem.HeatPump,
                HeatingType.Other => otherHeatingType switch
                {
                    OtherHeatingType.Biomass => BreHeatingSystem.BiomassBoiler,
                    OtherHeatingType.CoalOrSolidFuel => BreHeatingSystem.SolidFuelBoiler,
                    //peer-reviewed assumption:
                    OtherHeatingType.Other => BreHeatingSystem.GasBoiler,
                    _ => throw new ArgumentOutOfRangeException(nameof(otherHeatingType))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(heatingType))
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

        private static BreHeatingPatternType GetBreHeatingPatternType(HeatingPattern? heatingPattern,
            int? hoursOfHeatingMorning, int? hoursOfHeatingEvening)
        {
            if (hoursOfHeatingMorning == 0 && hoursOfHeatingEvening == 0)
            {
                //User has input values that correspond to a pre-existing BreHeatingPatternType
                return BreHeatingPatternType.NoneOfTheAbove;
            }

            if (hoursOfHeatingMorning == 12 && hoursOfHeatingEvening == 12)
            {
                //User has input values that correspond to a pre-existing BreHeatingPatternType
                return BreHeatingPatternType.AllDayAndAllNight;
            }

            return heatingPattern switch
            {
                HeatingPattern.AllDayAndNight => BreHeatingPatternType.AllDayAndAllNight,
                HeatingPattern.AllDayNotNight => BreHeatingPatternType.AllDayButOffAtNight,
                HeatingPattern.Other => BreHeatingPatternType.NoneOfTheAbove,
                _ => throw new ArgumentOutOfRangeException(nameof(heatingPattern))
            };
        }

        private static int[] GetBreNormalDaysOffHours(int? hoursOfHeatingMorning, int? hoursOfHeatingEvening)
        {
            if (hoursOfHeatingMorning != null && hoursOfHeatingEvening != null)
            {
                //This condition has the secondary purpose of guaranteeing that the sum of the two off periods returned
                //later is no more than 23 as is required by BRE 
                if ((hoursOfHeatingMorning == 0 && hoursOfHeatingEvening == 0)
                    || (hoursOfHeatingMorning == 12 && hoursOfHeatingEvening == 12))
                {
                    //User has input values that correspond to a pre-existing BreHeatingPatternType
                    return null;
                }

                //Peer-reviewed assumptions: time heating is turned on is not collected so we assume morning heating
                //starts at 7am, evening heating starts at 6pm. If morning/evening hours is greater than 5/6
                //(hence reaching 12 pm/am) then the extra hours are added on before 7am/6pm.
                var hoursOnFrom12amTo7am = Math.Max(0, hoursOfHeatingMorning.Value - 5);
                var hoursOnFrom7amTo12pm = Math.Min(5, hoursOfHeatingMorning.Value);
                var hoursOnFrom12pmTo6pm = Math.Max(0, hoursOfHeatingEvening.Value - 6);
                var hoursOnFrom6pmTo12am = Math.Min(6, hoursOfHeatingEvening.Value);
                //Hours between heating turning off in the morning and turning on in the evening
                var firstOffPeriod = (18 - hoursOnFrom12pmTo6pm) - (7 + hoursOnFrom7amTo12pm);
                //Hours between midnight and heating turning on in the morning, plus hours between heating turning off
                //in the evening and midnight 
                var secondOffPeriod = (7 - hoursOnFrom12amTo7am) + (6 - hoursOnFrom6pmTo12am);
                return [firstOffPeriod, secondOffPeriod];
            }

            return null;
        }

        private static BreFloorType? GetBreFloorType(FloorConstruction? floorConstruction,
            FloorInsulated? floorInsulated)
        {
            return floorConstruction switch
            {
                FloorConstruction.SuspendedTimber => floorInsulated switch
                {
                    FloorInsulated.Yes => BreFloorType.SuspendedFloorWithInsulation,
                    FloorInsulated.No => BreFloorType.SuspendedFloorWithoutInsulation,
                    //peer-reviewed assumption:
                    FloorInsulated.DoNotKnow => BreFloorType.SuspendedFloorWithoutInsulation,
                    _ => throw new ArgumentOutOfRangeException(nameof(floorInsulated))
                },
                FloorConstruction.SolidConcrete => floorInsulated switch
                {
                    FloorInsulated.Yes => BreFloorType.SolidFloorWithInsulation,
                    FloorInsulated.No => BreFloorType.SolidFloorWithoutInsulation,
                    //peer-reviewed assumption:
                    FloorInsulated.DoNotKnow => BreFloorType.SolidFloorWithoutInsulation,
                    _ => throw new ArgumentOutOfRangeException(nameof(floorInsulated))
                },
                FloorConstruction.Mix => floorInsulated switch
                {
                    //peer-reviewed assumptions:
                    FloorInsulated.Yes => BreFloorType.SuspendedFloorWithInsulation,
                    FloorInsulated.No => BreFloorType.SuspendedFloorWithoutInsulation,
                    FloorInsulated.DoNotKnow => BreFloorType.SuspendedFloorWithoutInsulation,
                    _ => throw new ArgumentOutOfRangeException(nameof(floorInsulated))
                },
                FloorConstruction.Other => BreFloorType.DontKnow,
                FloorConstruction.DoNotKnow => BreFloorType.DontKnow,
                _ => null
            };
        }

        private static bool GetBreOutsideSpace(HasOutdoorSpace? hasOutdoorSpace)
        {
            return hasOutdoorSpace switch
            {
                HasOutdoorSpace.Yes => true,
                HasOutdoorSpace.No => false,
                HasOutdoorSpace.DoNotKnow => true,
                _ => throw new ArgumentOutOfRangeException(nameof(hasOutdoorSpace))
            };
        }

        private static bool? GetBrePvPanels(SolarElectricPanels? solarElectricPanels)
        {
            return solarElectricPanels switch
            {
                SolarElectricPanels.Yes => true,
                SolarElectricPanels.No => false,
                SolarElectricPanels.DoNotKnow => false,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(solarElectricPanels))
            };
        }

        /// <returns>Mappings for the returned integer can be found in the BRE API Documentation</returns>
        private static int? GetBreHeatingControls(List<HeatingControls> heatingControls)
        {
            heatingControls.Sort();
            return heatingControls switch
            {
                [
                    HeatingControls.Programmer, HeatingControls.RoomThermostats,
                    HeatingControls.ThermostaticRadiatorValves
                ] => 1,
                [HeatingControls.RoomThermostats, HeatingControls.ThermostaticRadiatorValves] => 2,
                [HeatingControls.Programmer, HeatingControls.ThermostaticRadiatorValves] => 3,
                [HeatingControls.Programmer, HeatingControls.RoomThermostats] => 4,
                [HeatingControls.Programmer] => 5,
                [HeatingControls.RoomThermostats] => 6,
                [HeatingControls.ThermostaticRadiatorValves] => 7,
                [HeatingControls.None] => 8,
                [HeatingControls.DoNotKnow] => 9,
                [] => null,
                _ => throw new ArgumentOutOfRangeException("heatingControls value: [" +
                                                           string.Join(", ", heatingControls) + "]")
            };
        }
    }
}