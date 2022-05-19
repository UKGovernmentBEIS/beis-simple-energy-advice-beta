using System;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Services
{
    public interface IQuestionFlowService
    { 
        public PathByActionArguments BackLinkArguments(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
        
        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
        public PathByActionArguments SkipLinkArguments(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
    }

    public class QuestionFlowService: IQuestionFlowService
    {
        public PathByActionArguments BackLinkArguments(
            QuestionFlowPage page, 
            UserDataModel userData, 
            QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserBackLinkArguments(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.Country => CountryBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.ServiceUnsuitable => ServiceUnsuitableBackLinkArguments(userData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeBackLinkArguments(userData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressBackLinkArguments(userData),
                QuestionFlowPage.PropertyType => PropertyTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.AccessibleLoftSpace => AccessibleLoftSpaceBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.EmailAddress => EmailAddressBackLinkArguments(userData, entryPoint),
                QuestionFlowPage.AnswerSummary => AnswerSummaryBackLinkArguments(userData),
                QuestionFlowPage.YourRecommendations => YourRecommendationsBackLinkArguments(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public PathByActionArguments ForwardLinkArguments(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserForwardLinkArguments(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.Country => CountryForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.AskForPostcode => AskForPostcodeForwardLinkArguments(userData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressForwardLinkArguments(userData),
                QuestionFlowPage.PropertyType => PropertyTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.AccessibleLoftSpace => AccessibleLoftSpaceForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternForwardLinkArguments(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureForwardLinkArguments(userData),
                QuestionFlowPage.EmailAddress => EmailAddressForwardLinkArguments(userData),
                QuestionFlowPage.ServiceUnsuitable or QuestionFlowPage.AnswerSummary or QuestionFlowPage.YourRecommendations => throw new InvalidOperationException(),
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }

        public PathByActionArguments SkipLinkArguments(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.AskForPostcode => AskForPostcodeSkipLinkArguments(userData),
                QuestionFlowPage.HomeAge => HomeAgeSkipLinkArguments(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsSkipLinkArguments(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureSkipLinkArguments(userData),
                QuestionFlowPage.EmailAddress => EmailAddressSkipLinkArguments(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private PathByActionArguments NewOrReturningUserBackLinkArguments()
        {
            return new PathByActionArguments("Index", "EnergyEfficiency");
        }

        private PathByActionArguments OwnershipStatusBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OwnershipStatus
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("Country_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments CountryBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.Country
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        private PathByActionArguments ServiceUnsuitableBackLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return userData switch
            {
                { Country: not Country.England and not Country.Wales }
                    => new PathByActionArguments("Country_Get", "EnergyEfficiency", new { reference }),
                { OwnershipStatus: OwnershipStatus.PrivateTenancy }
                    => new PathByActionArguments("OwnershipStatus_Get", "EnergyEfficiency", new { reference }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private PathByActionArguments AskForPostcodeBackLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments ConfirmAddressBackLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments PropertyTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.PropertyType
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("AskForPostcode_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HouseTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments BungalowTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments FlatTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HomeAgeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HomeAge
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.PropertyType switch
                {
                    PropertyType.House => 
                        new PathByActionArguments("HouseType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.Bungalow => 
                        new PathByActionArguments("BungalowType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.ApartmentFlatOrMaisonette => 
                        new PathByActionArguments("FlatType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments WallConstructionBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.WallConstruction
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HomeAge_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments CavityWallsInsulatedBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.CavityWallsInsulated
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments SolidWallsInsulatedBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.SolidWallsInsulated
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Mixed => 
                        new PathByActionArguments("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Solid => 
                        new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments FloorConstructionBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.FloorConstruction
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        new PathByActionArguments("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Cavity =>
                        new PathByActionArguments("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
        }

        private PathByActionArguments FloorInsulatedBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.FloorInsulated
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofConstructionBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is QuestionFlowPage.RoofConstruction)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        new PathByActionArguments("FloorInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    new PathByActionArguments("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    new PathByActionArguments("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private PathByActionArguments AccessibleLoftSpaceBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.AccessibleLoftSpace
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments RoofInsulatedBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.RoofInsulated 
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments GlazingTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is QuestionFlowPage.GlazingType)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return userData switch
                {
                    { RoofConstruction: RoofConstruction.Flat }
                        => new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    { AccessibleLoftSpace: not AccessibleLoftSpace.Yes }
                        => new PathByActionArguments("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments("RoofInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        new PathByActionArguments("FloorInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    new PathByActionArguments("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    new PathByActionArguments("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private PathByActionArguments OutdoorSpaceBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OutdoorSpace
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("GlazingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HeatingTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HeatingType
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("OutdoorSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments OtherHeatingTypeBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OtherHeatingType
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments HotWaterCylinderBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HotWaterCylinder
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : new PathByActionArguments("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments NumberOfOccupantsBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.NumberOfOccupants
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.HeatingType switch
                {
                    HeatingType.Storage or HeatingType.DirectActionElectric or HeatingType.HeatPump
                        or HeatingType.DoNotKnow
                        => new PathByActionArguments("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler
                        => new PathByActionArguments("HotWaterCylinder_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.Other
                        => new PathByActionArguments("OtherHeatingType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private PathByActionArguments HeatingPatternBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HeatingPattern
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("NumberOfOccupants_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments TemperatureBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.Temperature
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HeatingPattern_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments EmailAddressBackLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.EmailAddress
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("Temperature_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private PathByActionArguments AnswerSummaryBackLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments YourRecommendationsBackLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NewOrReturningUserForwardLinkArguments()
        {
            // TODO: Routing for the first step?
            throw new InvalidOperationException();
        }
        
        private PathByActionArguments CountryForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.Country is not Country.England and not Country.Wales)
            {
                return new PathByActionArguments("ServiceUnsuitable", "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments OwnershipStatusForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.OwnershipStatus is OwnershipStatus.PrivateTenancy)
            {
                return new PathByActionArguments("ServiceUnsuitable", "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments AskForPostcodeForwardLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("ConfirmAddress_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments ConfirmAddressForwardLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("PropertyType_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments PropertyTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return userData.PropertyType switch
            {
                PropertyType.House =>
                    new PathByActionArguments("HouseType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.Bungalow =>
                    new PathByActionArguments("BungalowType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.ApartmentFlatOrMaisonette =>
                    new PathByActionArguments("FlatType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private PathByActionArguments HouseTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments BungalowTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FlatTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments HomeAgeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments WallConstructionForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;

            if (userData.WallConstruction is WallConstruction.Cavity or WallConstruction.Mixed)
            {
                return new PathByActionArguments("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (userData.WallConstruction == WallConstruction.Solid)
            {
                return new PathByActionArguments("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
            if (HasFloor(userData))
            {
                return new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments CavityWallsInsulatedForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            
            if (entryPoint is QuestionFlowPage.CavityWallsInsulated)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (userData.WallConstruction is WallConstruction.Mixed)
            {
                return new PathByActionArguments("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
            if (HasFloor(userData))
            {
                return new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency",new { reference });
            }

            if (HasRoof(userData))
            {
                return new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments SolidWallsInsulatedForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasFloor(userData))
            {
                return new PathByActionArguments("FloorConstruction_Get", "EnergyEfficiency",new { reference });
            }

            if (HasRoof(userData))
            {
                return new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FloorConstructionForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;

            if (userData.FloorConstruction is FloorConstruction.SolidConcrete or FloorConstruction.SuspendedTimber or FloorConstruction.Mix ) 
            {
                return new PathByActionArguments("FloorInsulated_Get", "EnergyEfficiency",new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments FloorInsulatedForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return new PathByActionArguments("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments RoofConstructionForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.RoofConstruction is RoofConstruction.Mixed or RoofConstruction.Pitched)
            {
                return new PathByActionArguments("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments AccessibleLoftSpaceForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.AccessibleLoftSpace is AccessibleLoftSpace.Yes)
            {
                return new PathByActionArguments("RoofInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            return new PathByActionArguments("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments RoofInsulatedForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("GlazingType_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments GlazingTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("OutdoorSpace_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments OutdoorSpaceForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("HeatingType_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments HeatingTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.HeatingType == HeatingType.Other)
            {
                return new PathByActionArguments("OtherHeatingType_Get", "EnergyEfficiency", new { reference, entryPoint});
            }

            if (userData.HeatingType is HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler)
            {
                return new PathByActionArguments("HotWaterCylinder_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference});
            }

            return new PathByActionArguments("NumberOfOccupants_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments OtherHeatingTypeForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("NumberOfOccupants_Get", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments HotWaterCylinderForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("NumberOfOccupants_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments NumberOfOccupantsForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("HeatingPattern_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments HeatingPatternForwardLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference})
                : new PathByActionArguments("Temperature_Get", "EnergyEfficiency",new { reference});
        }

        private PathByActionArguments TemperatureForwardLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AnswerSummary", "EnergyEfficiency",new { reference });
        }

        private PathByActionArguments EmailAddressForwardLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AnswerSummary", "EnergyEfficiency",new { reference });
        }
        
        private PathByActionArguments AskForPostcodeSkipLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("PropertyType_Get", "EnergyEfficiency", new { reference  });
        }

        private PathByActionArguments HomeAgeSkipLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("WallConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments NumberOfOccupantsSkipLinkArguments(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference })
                : new PathByActionArguments("HeatingPattern_Get", "EnergyEfficiency", new { reference });
        }

        private PathByActionArguments TemperatureSkipLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference  });
        }

        private PathByActionArguments EmailAddressSkipLinkArguments(UserDataModel userData)
        {
            var reference = userData.Reference;
            return new PathByActionArguments("AnswerSummary", "EnergyEfficiency", new { reference  });
        }

        private bool HasFloor(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor) => true,
                _ => false
            };
        }

        private bool HasRoof(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.TopFloor) => true,
                _ => false
            };
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
        AccessibleLoftSpace,
        RoofInsulated,
        GlazingType,
        OutdoorSpace,
        HeatingType,
        OtherHeatingType,
        HotWaterCylinder,
        NumberOfOccupants,
        HeatingPattern,
        Temperature,
        EmailAddress,
        AnswerSummary,
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