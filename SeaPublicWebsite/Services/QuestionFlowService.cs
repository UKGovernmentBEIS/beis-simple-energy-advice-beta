using System;
using System.Linq;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Controllers;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.Services
{
    public interface IQuestionFlowService
    { 
        public PathByActionArguments BeforeMainArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public PathByActionArguments SkipLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
    }

    public class QuestionFlowService: IQuestionFlowService
    {
        public PathByActionArguments BeforeMainArguments(
            QuestionFlowPage page, 
            PropertyData propertyData, 
            QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserBeforeMainArguments(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.Country => CountryBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.ServiceUnsuitable => ServiceUnsuitableBeforeMainArguments(propertyData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeBeforeMainArguments(propertyData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressBeforeMainArguments(propertyData),
                QuestionFlowPage.PropertyType => PropertyTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftSpace => LoftSpaceBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftAccess => LoftAccessBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureBeforeMainArguments(propertyData, entryPoint),
                QuestionFlowPage.AnswerSummary => AnswerSummaryBeforeMainArguments(propertyData),
                QuestionFlowPage.NoRecommendations => NoRecommendationsBeforeMainArguments(propertyData),
                QuestionFlowPage.YourRecommendations => YourRecommendationsBeforeMainArguments(propertyData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserForwardLinkArguments(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.Country => CountryForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.AskForPostcode => AskForPostcodeForwardLinkArguments(propertyData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressForwardLinkArguments(propertyData),
                QuestionFlowPage.PropertyType => PropertyTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftSpace => LoftSpaceForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftAccess => LoftAccessForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureForwardLinkArguments(propertyData),
                QuestionFlowPage.AnswerSummary => AnswerSummaryForwardLinkArguments(propertyData),
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }

        public PathByActionArguments SkipLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.AskForPostcode => AskForPostcodeSkipLinkArguments(propertyData),
                QuestionFlowPage.HomeAge => HomeAgeSkipLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsSkipLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureSkipLinkArguments(propertyData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private PathByActionArguments NewOrReturningUserBeforeMainArguments()
        {
            return new PathByActionArguments(nameof(EnergyEfficiencyController.Index), "EnergyEfficiency");
        }

        private PathByActionArguments CountryBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.Country
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NewOrReturningUser_Get), "EnergyEfficiency");
        }

        private PathByActionArguments OwnershipStatusBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OwnershipStatus
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.Country_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments ServiceUnsuitableBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return propertyData switch
            {
                { Country: not Country.England and not Country.Wales }
                    => new PathByActionArguments(nameof(EnergyEfficiencyController.Country_Get), "EnergyEfficiency", new { reference }),
                { OwnershipStatus: OwnershipStatus.PrivateTenancy }
                    => new PathByActionArguments(nameof(EnergyEfficiencyController.OwnershipStatus_Get), "EnergyEfficiency", new { reference }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private PathByActionArguments AskForPostcodeBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.OwnershipStatus_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments ConfirmAddressBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments PropertyTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.PropertyType
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HouseTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments BungalowTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments FlatTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HomeAgeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HomeAge
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : propertyData.PropertyType switch
                {
                    PropertyType.House => 
                        new PathByActionArguments(nameof(EnergyEfficiencyController.HouseType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.Bungalow => 
                        new PathByActionArguments(nameof(EnergyEfficiencyController.BungalowType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.ApartmentFlatOrMaisonette => 
                        new PathByActionArguments(nameof(EnergyEfficiencyController.FlatType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments WallConstructionBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.WallConstruction
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HomeAge_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments CavityWallsInsulatedBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.CavityWallsInsulated
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments SolidWallsInsulatedBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.SolidWallsInsulated
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : propertyData.WallConstruction switch
                {
                    WallConstruction.Mixed => 
                        new PathByActionArguments(nameof(EnergyEfficiencyController.CavityWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Solid => 
                        new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments FloorConstructionBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.FloorConstruction
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : propertyData.WallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        new PathByActionArguments(nameof(EnergyEfficiencyController.SolidWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Cavity =>
                        new PathByActionArguments(nameof(EnergyEfficiencyController.CavityWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
                };
        }

        private PathByActionArguments FloorInsulatedBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.FloorInsulated
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofConstructionBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (entryPoint is QuestionFlowPage.RoofConstruction)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasFloor(propertyData))
            {
                return propertyData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        new PathByActionArguments(nameof(EnergyEfficiencyController.FloorInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return propertyData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.SolidWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.CavityWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                _ => new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private PathByActionArguments LoftSpaceBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.LoftSpace
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }
        
        private PathByActionArguments LoftAccessBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.LoftAccess
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.LoftSpace_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofInsulatedBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.RoofInsulated 
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.LoftAccess_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments GlazingTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (entryPoint is QuestionFlowPage.GlazingType)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return propertyData switch
                {
                    { RoofConstruction: RoofConstruction.Flat }
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    { LoftSpace: not LoftSpace.Yes }
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.LoftSpace_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    { LoftAccess: not LoftAccess.Yes }
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.LoftAccess_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments(nameof(EnergyEfficiencyController.RoofInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                };
            }

            if (PropertyDataHelper.HasFloor(propertyData))
            {
                return propertyData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        new PathByActionArguments(nameof(EnergyEfficiencyController.FloorInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return propertyData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.SolidWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.CavityWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint }),
                _ => new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private PathByActionArguments OutdoorSpaceBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OutdoorSpace
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HeatingTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HeatingType
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.OutdoorSpace_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments OtherHeatingTypeBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OtherHeatingType
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HotWaterCylinderBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HotWaterCylinder
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments NumberOfOccupantsBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.NumberOfOccupants
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : propertyData.HeatingType switch
                {
                    HeatingType.Storage or HeatingType.DirectActionElectric or HeatingType.HeatPump
                        or HeatingType.DoNotKnow
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.HotWaterCylinder_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.Other
                        => new PathByActionArguments(nameof(EnergyEfficiencyController.OtherHeatingType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments HeatingPatternBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HeatingPattern
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NumberOfOccupants_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments TemperatureBeforeMainArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.Temperature
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingPattern_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments AnswerSummaryBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.Temperature_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NoRecommendationsBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments YourRecommendationsBeforeMainArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NewOrReturningUserForwardLinkArguments()
        {
            // TODO: Routing for the first step?
            throw new InvalidOperationException();
        }
        
        private PathByActionArguments CountryForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.Country is not Country.England and not Country.Wales)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.ServiceUnsuitable), "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.OwnershipStatus_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments OwnershipStatusForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.OwnershipStatus is OwnershipStatus.PrivateTenancy)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.ServiceUnsuitable), "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments AskForPostcodeForwardLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.ConfirmAddress_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments ConfirmAddressForwardLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments PropertyTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return propertyData.PropertyType switch
            {
                PropertyType.House =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.HouseType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.Bungalow =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.BungalowType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.ApartmentFlatOrMaisonette =>
                    new PathByActionArguments(nameof(EnergyEfficiencyController.FlatType_Get), "EnergyEfficiency", new { reference, entryPoint }),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private PathByActionArguments HouseTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HomeAge_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments BungalowTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HomeAge_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FlatTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HomeAge_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments HomeAgeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments WallConstructionForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;

            if (propertyData.WallConstruction is WallConstruction.Cavity or WallConstruction.Mixed)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.CavityWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint });
            }

            if (propertyData.WallConstruction == WallConstruction.Solid)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.SolidWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
            if (PropertyDataHelper.HasFloor(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments CavityWallsInsulatedForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            
            if (entryPoint is QuestionFlowPage.CavityWallsInsulated)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (propertyData.WallConstruction is WallConstruction.Mixed)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.SolidWallsInsulated_Get), "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
            if (PropertyDataHelper.HasFloor(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency",new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments SolidWallsInsulatedForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasFloor(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency",new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FloorConstructionForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;

            if (propertyData.FloorConstruction is FloorConstruction.SolidConcrete or FloorConstruction.SuspendedTimber or FloorConstruction.Mix ) 
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.FloorInsulated_Get), "EnergyEfficiency",new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FloorInsulatedForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            if (PropertyDataHelper.HasRoof(propertyData))
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments RoofConstructionForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.RoofConstruction is RoofConstruction.Mixed or RoofConstruction.Pitched)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.LoftSpace_Get), "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments LoftSpaceForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.LoftSpace is LoftSpace.Yes)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.LoftAccess_Get), "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference });
        }
        
        private PathByActionArguments LoftAccessForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.LoftAccess is LoftAccess.Yes)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.RoofInsulated_Get), "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments RoofInsulatedForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments GlazingTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.OutdoorSpace_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments OutdoorSpaceForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments HeatingTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.HeatingType == HeatingType.Other)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.OtherHeatingType_Get), "EnergyEfficiency", new { reference, entryPoint});
            }

            if (propertyData.HeatingType is HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.HotWaterCylinder_Get), "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference});
            }

            return new PathByActionArguments(nameof(EnergyEfficiencyController.NumberOfOccupants_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments OtherHeatingTypeForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NumberOfOccupants_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments HotWaterCylinderForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NumberOfOccupants_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments NumberOfOccupantsForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingPattern_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments HeatingPatternForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference})
                : new PathByActionArguments(nameof(EnergyEfficiencyController.Temperature_Get), "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments TemperatureForwardLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments AnswerSummaryForwardLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return propertyData.PropertyRecommendations.Any()
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.YourRecommendations_Get),
                    "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NoRecommendations_Get),
                    "EnergyEfficiency", new { reference });
        }
        
        private PathByActionArguments AskForPostcodeSkipLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference  });
        }

        private PathByActionArguments HomeAgeSkipLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NumberOfOccupantsSkipLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingPattern_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments TemperatureSkipLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference  });
        }
    }

    public enum QuestionFlowPage
    {
        NewOrReturningUser,
        Country,
        OwnershipStatus,
        ServiceUnsuitable,
        AskForPostcode,
        ConfirmAddress,
        PropertyType,
        HouseType,
        BungalowType,
        FlatType,
        HomeAge,
        WallConstruction,
        CavityWallsInsulated,
        SolidWallsInsulated,
        FloorConstruction,
        FloorInsulated,
        RoofConstruction,
        LoftSpace,
        LoftAccess,
        RoofInsulated,
        GlazingType,
        OutdoorSpace,
        HeatingType,
        OtherHeatingType,
        HotWaterCylinder,
        NumberOfOccupants,
        HeatingPattern,
        Temperature,
        AnswerSummary,
        NoRecommendations,
        YourRecommendations
    }

    public class PathByActionArguments
    {
        public readonly string Action;
        public readonly string Controller;
        public readonly object Values;
        
        public PathByActionArguments(string action, string controller, object values = null)
        {
            Action = action;
            Controller = controller;
            Values = values;
        }
    }
}