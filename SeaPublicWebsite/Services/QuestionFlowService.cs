using System;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Services
{
    public interface IQuestionFlowService
    { 
        public string BackLink(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
        
        public string ForwardLink(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
        public string SkipLink(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null);
    }

    public class QuestionFlowService: IQuestionFlowService
    {
        private readonly LinkGenerator linkGenerator;

        public QuestionFlowService(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }
        
        public string BackLink(
            QuestionFlowPage page, 
            UserDataModel userData, 
            QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserBackLink(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusBackLink(userData, entryPoint),
                QuestionFlowPage.Country => CountryBackLink(userData, entryPoint),
                QuestionFlowPage.ServiceUnsuitable => ServiceUnsuitableBackLink(userData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeBackLink(userData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressBackLink(userData),
                QuestionFlowPage.PropertyType => PropertyTypeBackLink(userData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeBackLink(userData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeBackLink(userData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeBackLink(userData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeBackLink(userData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionBackLink(userData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedBackLink(userData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedBackLink(userData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionBackLink(userData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedBackLink(userData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionBackLink(userData, entryPoint),
                QuestionFlowPage.AccessibleLoftSpace => AccessibleLoftSpaceBackLink(userData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedBackLink(userData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceBackLink(userData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeBackLink(userData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeBackLink(userData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeBackLink(userData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderBackLink(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsBackLink(userData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternBackLink(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureBackLink(userData, entryPoint),
                QuestionFlowPage.EmailAddress => EmailAddressBackLink(userData, entryPoint),
                QuestionFlowPage.AnswerSummary => AnswerSummaryBackLink(userData),
                QuestionFlowPage.YourRecommendations => YourRecommendationsBackLink(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string ForwardLink(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserForwardLink(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusForwardLink(userData, entryPoint),
                QuestionFlowPage.Country => CountryForwardLink(userData, entryPoint),
                QuestionFlowPage.AskForPostcode => AskForPostcodeForwardLink(userData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressForwardLink(userData),
                QuestionFlowPage.PropertyType => PropertyTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.FlatType => FlatTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeForwardLink(userData, entryPoint),
                QuestionFlowPage.WallConstruction => WallConstructionForwardLink(userData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedForwardLink(userData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedForwardLink(userData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionForwardLink(userData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedForwardLink(userData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionForwardLink(userData, entryPoint),
                QuestionFlowPage.AccessibleLoftSpace => AccessibleLoftSpaceForwardLink(userData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedForwardLink(userData, entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceForwardLink(userData, entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeForwardLink(userData, entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderForwardLink(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsForwardLink(userData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternForwardLink(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureForwardLink(userData),
                QuestionFlowPage.EmailAddress => EmailAddressForwardLink(userData),
                QuestionFlowPage.ServiceUnsuitable or QuestionFlowPage.AnswerSummary or QuestionFlowPage.YourRecommendations => throw new InvalidOperationException(),
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }

        public string SkipLink(QuestionFlowPage page, UserDataModel userData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.AskForPostcode => AskForPostcodeSkipLink(userData),
                QuestionFlowPage.HomeAge => HomeAgeSkipLink(userData, entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsSkipLink(userData, entryPoint),
                QuestionFlowPage.Temperature => TemperatureSkipLink(userData),
                QuestionFlowPage.EmailAddress => EmailAddressSkipLink(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string NewOrReturningUserBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }

        private string OwnershipStatusBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OwnershipStatus
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string CountryBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.Country
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        private string ServiceUnsuitableBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return userData switch
            {
                { Country: not Country.England and not Country.Wales }
                    => linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference }),
                { OwnershipStatus: OwnershipStatus.PrivateTenancy }
                    => linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string AskForPostcodeBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string ConfirmAddressBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string PropertyTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.PropertyType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HouseTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string BungalowTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string FlatTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HomeAgeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HomeAge
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.PropertyType switch
                {
                    PropertyType.House => 
                        linkGenerator.GetPathByAction("HouseType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.Bungalow => 
                        linkGenerator.GetPathByAction("BungalowType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    PropertyType.ApartmentFlatOrMaisonette => 
                        linkGenerator.GetPathByAction("FlatType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private string WallConstructionBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.WallConstruction
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string CavityWallsInsulatedBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.CavityWallsInsulated
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string SolidWallsInsulatedBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.SolidWallsInsulated
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Mixed => 
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Solid => 
                        linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private string FloorConstructionBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.FloorConstruction
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    WallConstruction.Cavity =>
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
        }

        private string FloorInsulatedBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.FloorInsulated
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string RoofConstructionBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is QuestionFlowPage.RoofConstruction)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private string AccessibleLoftSpaceBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.AccessibleLoftSpace
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string RoofInsulatedBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.RoofInsulated 
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string GlazingTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is QuestionFlowPage.GlazingType)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return userData switch
                {
                    { RoofConstruction: RoofConstruction.Flat }
                        => linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    { AccessibleLoftSpace: not AccessibleLoftSpace.Yes }
                        => linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                WallConstruction.Cavity =>
                    linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint }),
            };
        }

        private string OutdoorSpaceBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OutdoorSpace
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HeatingTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HeatingType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string OtherHeatingTypeBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.OtherHeatingType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HotWaterCylinderBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HotWaterCylinder
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string NumberOfOccupantsBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.NumberOfOccupants
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.HeatingType switch
                {
                    HeatingType.Storage or HeatingType.DirectActionElectric or HeatingType.HeatPump
                        or HeatingType.DoNotKnow
                        => linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler
                        => linkGenerator.GetPathByAction("HotWaterCylinder_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    HeatingType.Other
                        => linkGenerator.GetPathByAction("OtherHeatingType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private string HeatingPatternBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.HeatingPattern
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string TemperatureBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.Temperature
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string EmailAddressBackLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is QuestionFlowPage.EmailAddress
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string AnswerSummaryBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private string YourRecommendationsBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
        }

        private string NewOrReturningUserForwardLink()
        {
            // TODO: Routing for the first step?
            throw new InvalidOperationException();
        }
        
        private string CountryForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.Country is not Country.England and not Country.Wales)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string OwnershipStatusForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.OwnershipStatus is OwnershipStatus.PrivateTenancy)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new { reference });
            }

            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string AskForPostcodeForwardLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("ConfirmAddress_Get", "EnergyEfficiency", new { reference });
        }

        private string ConfirmAddressForwardLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference });
        }

        private string PropertyTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return userData.PropertyType switch
            {
                PropertyType.House =>
                    linkGenerator.GetPathByAction("HouseType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.Bungalow =>
                    linkGenerator.GetPathByAction("BungalowType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                PropertyType.ApartmentFlatOrMaisonette =>
                    linkGenerator.GetPathByAction("FlatType_Get", "EnergyEfficiency", new { reference, entryPoint }),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private string HouseTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string BungalowTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string FlatTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string HomeAgeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency",new { reference });
        }

        private string WallConstructionForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;

            if (userData.WallConstruction is WallConstruction.Cavity or WallConstruction.Mixed)
            {
                return linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (userData.WallConstruction == WallConstruction.Solid)
            {
                return linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
            if (HasFloor(userData))
            {
                return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private string CavityWallsInsulatedForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            
            if (entryPoint is QuestionFlowPage.CavityWallsInsulated)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (userData.WallConstruction is WallConstruction.Mixed)
            {
                return linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
            if (HasFloor(userData))
            {
                return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",new { reference });
            }

            if (HasRoof(userData))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private string SolidWallsInsulatedForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasFloor(userData))
            {
                return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",new { reference });
            }

            if (HasRoof(userData))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private string FloorConstructionForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;

            if (userData.FloorConstruction is FloorConstruction.SolidConcrete or FloorConstruction.SuspendedTimber or FloorConstruction.Mix ) 
            {
                return linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency",new { reference, entryPoint });
            }
            
            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private string FloorInsulatedForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private string RoofConstructionForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.RoofConstruction is RoofConstruction.Mixed or RoofConstruction.Pitched)
            {
                return linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private string AccessibleLoftSpaceForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.AccessibleLoftSpace is AccessibleLoftSpace.Yes)
            {
                return linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private string RoofInsulatedForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference});
        }

        private string GlazingTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency",new { reference});
        }

        private string OutdoorSpaceForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.HeatingType == HeatingType.Other)
            {
                return linkGenerator.GetPathByAction("OtherHeatingType_Get", "EnergyEfficiency", new { reference, entryPoint});
            }

            if (userData.HeatingType is HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler)
            {
                return linkGenerator.GetPathByAction("HotWaterCylinder_Get", "EnergyEfficiency", new { reference, entryPoint });
            }

            if (entryPoint is not null)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference});
            }

            return linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference });
        }

        private string OtherHeatingTypeForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference });
        }

        private string HotWaterCylinderForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference});
        }

        private string NumberOfOccupantsForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingPatternForwardLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency",new { reference});
        }

        private string TemperatureForwardLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency",new { reference });
        }

        private string EmailAddressForwardLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency",new { reference });
        }
        
        private string AskForPostcodeSkipLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference  });
        }

        private string HomeAgeSkipLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string NumberOfOccupantsSkipLink(UserDataModel userData, QuestionFlowPage? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference });
        }

        private string TemperatureSkipLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference  });
        }

        private string EmailAddressSkipLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference  });
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
}