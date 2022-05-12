using System;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    { 
        public string BackLink(PageName page, UserDataModel userData, PageName? entryPoint = null, string from = null);
        
        public string ForwardLink(PageName page, UserDataModel userData, PageName? entryPoint = null);
    }

    public class PageLinker: IPageLinker
    {
        private readonly LinkGenerator linkGenerator;

        public PageLinker(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }
        
        public string BackLink(
            PageName page, 
            UserDataModel userData, 
            PageName? entryPoint = null,
            string from = null)
        {
            return page switch
            {
                PageName.NewOrReturningUser => NewOrReturningUserBackLink(),
                PageName.OwnershipStatus => OwnershipStatusBackLink(userData, entryPoint),
                PageName.Country => CountryBackLink(userData, entryPoint),
                PageName.ServiceUnsuitable => ServiceUnsuitableBackLink(userData, from),
                PageName.AskForPostcode => AskForPostcodeBackLink(userData),
                PageName.ConfirmAddress => ConfirmAddressBackLink(userData),
                PageName.PropertyType => PropertyTypeBackLink(userData, entryPoint),
                PageName.HouseType => HouseTypeBackLink(userData, entryPoint),
                PageName.BungalowType => BungalowTypeBackLink(userData, entryPoint),
                PageName.FlatType => FlatTypeBackLink(userData, entryPoint),
                PageName.HomeAge => HomeAgeBackLink(userData, entryPoint),
                PageName.WallConstruction => WallConstructionBackLink(userData, entryPoint),
                PageName.CavityWallsInsulated => CavityWallsInsulatedBackLink(userData, entryPoint),
                PageName.SolidWallsInsulated => SolidWallsInsulatedBackLink(userData, entryPoint),
                PageName.FloorConstruction => FloorConstructionBackLink(userData, entryPoint),
                PageName.FloorInsulated => FloorInsulatedBackLink(userData, entryPoint),
                PageName.RoofConstruction => RoofConstructionBackLink(userData, entryPoint),
                PageName.AccessibleLoftSpace => AccessibleLoftSpaceBackLink(userData, entryPoint),
                PageName.RoofInsulated => RoofInsulatedBackLink(userData, entryPoint),
                PageName.OutdoorSpace => OutdoorSpaceBackLink(userData, entryPoint),
                PageName.GlazingType => GlazingTypeBackLink(userData, entryPoint),
                PageName.HeatingType => HeatingTypeBackLink(userData, entryPoint),
                PageName.OtherHeatingType => OtherHeatingTypeBackLink(userData, entryPoint),
                PageName.HotWaterCylinder => HotWaterCylinderBackLink(userData, entryPoint),
                PageName.NumberOfOccupants => NumberOfOccupantsBackLink(userData, entryPoint),
                PageName.HeatingPattern => HeatingPatternBackLink(userData, entryPoint),
                PageName.Temperature => TemperatureBackLink(userData, entryPoint),
                PageName.EmailAddress => EmailAddressBackLink(userData, entryPoint),
                PageName.AnswerSummary => AnswerSummaryBackLink(userData),
                PageName.YourRecommendations => YourRecommendationsBackLink(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string ForwardLink(PageName page, UserDataModel userData, PageName? entryPoint = null)
        {
            return page switch
            {
                PageName.NewOrReturningUser => NewOrReturningUserForwardLink(),
                PageName.OwnershipStatus => OwnershipStatusForwardLink(userData, entryPoint),
                PageName.Country => CountryForwardLink(userData, entryPoint),
                PageName.AskForPostcode => AskForPostcodeForwardLink(userData),
                PageName.ConfirmAddress => ConfirmAddressForwardLink(userData),
                PageName.PropertyType => PropertyTypeForwardLink(userData, entryPoint),
                PageName.HouseType => HouseTypeForwardLink(userData, entryPoint),
                PageName.BungalowType => BungalowTypeForwardLink(userData, entryPoint),
                PageName.FlatType => FlatTypeForwardLink(userData, entryPoint),
                PageName.HomeAge => HomeAgeForwardLink(userData, entryPoint),
                PageName.WallConstruction => WallConstructionForwardLink(userData, entryPoint),
                PageName.CavityWallsInsulated => CavityWallsInsulatedForwardLink(userData, entryPoint),
                PageName.SolidWallsInsulated => SolidWallsInsulatedForwardLink(userData, entryPoint),
                PageName.FloorConstruction => FloorConstructionForwardLink(userData, entryPoint),
                PageName.FloorInsulated => FloorInsulatedForwardLink(userData, entryPoint),
                PageName.RoofConstruction => RoofConstructionForwardLink(userData, entryPoint),
                PageName.AccessibleLoftSpace => AccessibleLoftSpaceForwardLink(userData, entryPoint),
                PageName.RoofInsulated => RoofInsulatedForwardLink(userData, entryPoint),
                PageName.OutdoorSpace => OutdoorSpaceForwardLink(userData, entryPoint),
                PageName.GlazingType => GlazingTypeForwardLink(userData, entryPoint),
                PageName.HeatingType => HeatingTypeForwardLink(userData, entryPoint),
                PageName.OtherHeatingType => OtherHeatingTypeForwardLink(userData, entryPoint),
                PageName.HotWaterCylinder => HotWaterCylinderForwardLink(userData, entryPoint),
                PageName.NumberOfOccupants => NumberOfOccupantsForwardLink(userData, entryPoint),
                PageName.HeatingPattern => HeatingPatternForwardLink(userData, entryPoint),
                PageName.Temperature => TemperatureForwardLink(userData),
                PageName.EmailAddress => EmailAddressForwardLink(userData),
                PageName.ServiceUnsuitable or PageName.AnswerSummary or PageName.YourRecommendations => throw new InvalidOperationException(),
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }

        private string NewOrReturningUserBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }

        private string OwnershipStatusBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.OwnershipStatus
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string CountryBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.Country
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        private string ServiceUnsuitableBackLink(UserDataModel userData, string from)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction($"{from}_Get", "EnergyEfficiency", new { reference });
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

        private string PropertyTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.PropertyType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HouseTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string BungalowTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string FlatTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HomeAgeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.HomeAge
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

        private string WallConstructionBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.WallConstruction
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string CavityWallsInsulatedBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.CavityWallsInsulated
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string SolidWallsInsulatedBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.SolidWallsInsulated
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

        private string FloorConstructionBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.FloorConstruction
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

        private string FloorInsulatedBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.FloorInsulated
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string RoofConstructionBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is PageName.RoofConstruction)
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

        private string AccessibleLoftSpaceBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.AccessibleLoftSpace
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string RoofInsulatedBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.RoofInsulated 
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string GlazingTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            if (entryPoint is PageName.GlazingType)
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

        private string OutdoorSpaceBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.OutdoorSpace
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HeatingTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.HeatingType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string OtherHeatingTypeBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.OtherHeatingType
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string HotWaterCylinderBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.HotWaterCylinder
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference }) 
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string NumberOfOccupantsBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.NumberOfOccupants
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

        private string HeatingPatternBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.HeatingPattern
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string TemperatureBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.Temperature
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference, entryPoint });
        }

        private string EmailAddressBackLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is PageName.EmailAddress
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
        
        private string CountryForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.Country is not Country.England && userData.Country is not Country.Wales)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "Country", reference });
            }

            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string OwnershipStatusForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            if (userData.OwnershipStatus is OwnershipStatus.PrivateTenancy)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "OwnershipStatus", reference });
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

        private string PropertyTypeForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string HouseTypeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string BungalowTypeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string FlatTypeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference });
        }

        private string HomeAgeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency",new { reference });
        }

        private string WallConstructionForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string CavityWallsInsulatedForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            
            if (entryPoint is PageName.CavityWallsInsulated)
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

        private string SolidWallsInsulatedForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string FloorConstructionForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string FloorInsulatedForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string RoofConstructionForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string AccessibleLoftSpaceForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string RoofInsulatedForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference});
        }

        private string GlazingTypeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency",new { reference});
        }

        private string OutdoorSpaceForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingTypeForwardLink(UserDataModel userData, PageName? entryPoint)
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

        private string OtherHeatingTypeForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference });
        }

        private string HotWaterCylinderForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference});
        }

        private string NumberOfOccupantsForwardLink(UserDataModel userData, PageName? entryPoint)
        {
            var reference = userData.Reference;
            return entryPoint is not null
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingPatternForwardLink(UserDataModel userData, PageName? entryPoint)
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

    public enum PageName
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