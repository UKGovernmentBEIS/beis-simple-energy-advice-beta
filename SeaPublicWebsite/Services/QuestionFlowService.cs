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
        public PathByActionArguments BackLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public PathByActionArguments SkipLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
    }

    public class QuestionFlowService: IQuestionFlowService
    {
        public PathByActionArguments BackLinkArguments(
            QuestionFlowPage page, 
            PropertyData propertyData, 
            QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserBackLinkArguments(),
                QuestionFlowPage.Country => CountryBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FindEpc => FindEpcBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.ServiceUnsuitable => ServiceUnsuitableBackLinkArguments(propertyData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeBackLinkArguments(propertyData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressBackLinkArguments(propertyData),
                QuestionFlowPage.PropertyType => PropertyTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftSpace => LoftSpaceBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.LoftAccess => LoftAccessBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureBackLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.AnswerSummary => AnswerSummaryBackLinkArguments(propertyData),
                QuestionFlowPage.NoRecommendations => NoRecommendationsBackLinkArguments(propertyData),
                QuestionFlowPage.YourRecommendations => YourRecommendationsBackLinkArguments(propertyData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserForwardLinkArguments(),
                QuestionFlowPage.Country => CountryForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusForwardLinkArguments(propertyData, entryPoint),
                QuestionFlowPage.FindEpc => FindEpcForwardLinkArguments(propertyData, entryPoint),
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

        private PathByActionArguments NewOrReturningUserBackLinkArguments()
        {
            return new PathByActionArguments(nameof(EnergyEfficiencyController.Index), "EnergyEfficiency");
        }

        private PathByActionArguments CountryBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.Country
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NewOrReturningUser_Get), "EnergyEfficiency");
        }

        private PathByActionArguments OwnershipStatusBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OwnershipStatus
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.Country_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments ServiceUnsuitableBackLinkArguments(PropertyData propertyData)
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

        private PathByActionArguments FindEpcBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.OwnershipStatus_Get), "EnergyEfficiency", new { reference, entryPoint });
        }
        
        private PathByActionArguments AskForPostcodeBackLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.FindEpc_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments ConfirmAddressBackLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments PropertyTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.FindEpc == FindEpc.Yes)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference, entryPoint });
            }
            return new PathByActionArguments(nameof(EnergyEfficiencyController.FindEpc_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HouseTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments BungalowTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments FlatTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HomeAgeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments WallConstructionBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.WallConstruction
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HomeAge_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments CavityWallsInsulatedBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.CavityWallsInsulated
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.WallConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments SolidWallsInsulatedBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments FloorConstructionBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments FloorInsulatedBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.FloorInsulated
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.FloorConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofConstructionBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments LoftSpaceBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.LoftSpace
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.RoofConstruction_Get), "EnergyEfficiency", new { reference, entryPoint });
        }
        
        private PathByActionArguments LoftAccessBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.LoftAccess
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.LoftSpace_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofInsulatedBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.RoofInsulated 
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.LoftAccess_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments GlazingTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments OutdoorSpaceBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OutdoorSpace
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.GlazingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HeatingTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HeatingType
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.OutdoorSpace_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments OtherHeatingTypeBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.OtherHeatingType
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HotWaterCylinderBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HotWaterCylinder
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingType_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments NumberOfOccupantsBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
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

        private PathByActionArguments HeatingPatternBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.HeatingPattern
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.NumberOfOccupants_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments TemperatureBackLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            return entryPoint is QuestionFlowPage.Temperature
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.HeatingPattern_Get), "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments AnswerSummaryBackLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.Temperature_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NoRecommendationsBackLinkArguments(PropertyData propertyData)
        {
            var reference = propertyData.Reference;
            return new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments YourRecommendationsBackLinkArguments(PropertyData propertyData)
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
                : new PathByActionArguments(nameof(EnergyEfficiencyController.FindEpc_Get), "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments FindEpcForwardLinkArguments(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            var reference = propertyData.Reference;
            if (propertyData.FindEpc is FindEpc.Yes)
            {
                return new PathByActionArguments(nameof(EnergyEfficiencyController.AskForPostcode_Get), "EnergyEfficiency", new { reference });
            }
            
            return entryPoint is not null
                ? new PathByActionArguments(nameof(EnergyEfficiencyController.AnswerSummary_Get), "EnergyEfficiency", new { reference })
                : new PathByActionArguments(nameof(EnergyEfficiencyController.PropertyType_Get), "EnergyEfficiency", new { reference });
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
        FindEpc,
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